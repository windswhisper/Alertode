using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Honeti;

public class FailPanel : MonoBehaviour
{
	public Text txtMsg;
	public Text txtGem;

	public GameObject btnGemRevive;
	public GameObject btnAdRevive;

	public void Show(){
		VolumeManager.ins.PauseBgm();
 		gameObject.SetActive(true);

 		txtMsg.text = string.Format(I18N.instance.getValue("@UI.StageLose"),BattleManager.ins.engine.wave,BattleMap.ins.mapData.wave);
 		txtGem.text = UserData.data.gem+"";


        #if UNITY_ANDROID || UNITY_IPHONE

 			btnGemRevive.SetActive(false);
 			btnAdRevive.SetActive(true);

        #endif
        
	}

 	public void Home(){
 		UserData.Save();
        LoadingMask.ins.LoadSceneAsync("StageScene");
 	}

 	public void Restart(){
        LoadingMask.ins.LoadSceneAsync("BattleScene");
 	}

 	public void GemRevive(){
 		if(UserData.data.gem < 50){
 			ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoGem"));
 			return;
 		}

 		UserData.data.gem-=50;
 		Revive();
 	}

 	public void AdRevive(){
 		MobileAdManager.ins.ShowRewardedAd(()=>{
 			Revive();
 			});
 	}

 	public void Revive(){

		VolumeManager.ins.ResumeBgm();
 		BattleManager.ins.Revive();
 		UserData.Save();
 		gameObject.SetActive(false);
 	}
}
