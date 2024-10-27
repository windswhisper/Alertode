using UnityEngine.Networking;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

[Serializable]
public class GiftResponseData{
	public string activity_id;
	public List<GiftContentData> content_obj;
	public int error;
}

[Serializable]
public class GiftContentData{
	public string name;
	public int number;
    public GiftContentData(string name,int number){
        this.name = name;
        this.number = number;
    }
}


public class GiftCodePanel : MonoBehaviour
{
	public InputField input;
	public GameObject rewardPanelPrefab;

    private const string apiUrl = "https://poster-api.xd.cn/api/v1.0/cdk/game/submit-simple";
    private const string contentType = "application/json";
    private const string clientId = "3wwmg3bmbm5kkpuzcl";

	public void Redeem(){
		var code = input.text;
		if(code=="")return;
		StartCoroutine(StartRequest(code));
	}


    private IEnumerator StartRequest(string giftCode)
    {
        // 获取当前时间戳（秒）
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;

        string nonceStr = Utils.CreateRandomStr(5);

        // 拼接并加密 sign 参数
        string sign = GetSign(timestamp, nonceStr, clientId);

        // 构建请求参数
        string requestBody = "{\"client_id\":\"" + clientId + "\",\"gift_code\":\"" + giftCode + "\",\"character_id\":\"" + UserData.GetUserId() + "\",\"nonce_str\":\"" + nonceStr + "\",\"timestamp\":" + timestamp + ",\"sign\":\"" + sign + "\"}";

        // 创建 UnityWebRequest 对象
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // 设置请求头
        request.SetRequestHeader("Content-Type", contentType);

        // 设置请求体
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 发送请求
        yield return request.SendWebRequest();

        // 处理响应
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("API request succeeded");
            Debug.Log(request.downloadHandler.text);

			var data = JsonUtility.FromJson<GiftResponseData> (request.downloadHandler.text);
			GetReward(data.content_obj);
        }
        else
        {
            Debug.Log("API request failed: " + request.error);
            Debug.Log(request.downloadHandler.text);
			var data = JsonUtility.FromJson<GiftResponseData> (request.downloadHandler.text);
			if(data.error == 100025 || data.error == 100017){
				ToastBar.ShowMsg(I18N.instance.getValue("@UI.GiftCodeUsed"));
			}
			else if(data.error == 100016){
				ToastBar.ShowMsg(I18N.instance.getValue("@UI.GiftCodeWrong"));
			}
			else if(data.error == 100006){
				ToastBar.ShowMsg(I18N.instance.getValue("@UI.GiftCodeNoMore"));
			}
			else{
				ToastBar.ShowMsg(I18N.instance.getValue("@UI.GiftCodeError")+data.error);
			}
        }
    }

    private string GetSign(int timestamp, string nonceStr, string clientId)
    {
        // 拼接参数并进行 SHA1 加密
        string signString = timestamp.ToString() + nonceStr + clientId;
        byte[] signBytes = Encoding.UTF8.GetBytes(signString);
        byte[] signHash = new SHA1CryptoServiceProvider().ComputeHash(signBytes);
        string sign = BitConverter.ToString(signHash).Replace("-", "").ToLowerInvariant();

        return sign;
    }

    void GetReward(List<GiftContentData> rewards){
    	foreach(var reward in rewards){
    		if(reward.name == "gem"){
    			UserData.data.gem+=reward.number;
    		}
    		else if(reward.name == "metal"){
    			UserData.data.metal+=reward.number;
    		}
    		else if(reward.name == "silicon"){
    			UserData.data.silicon+=reward.number;
    		}
    	}

    	UserData.Save();

    	var rewardPanel = Instantiate(rewardPanelPrefab);
    	rewardPanel.GetComponent<RewardPanel>().Init(rewards);
    	rewardPanel.transform.SetParent(transform.parent,false);

    	input.text = "";
    	gameObject.SetActive(false);
    }
}
