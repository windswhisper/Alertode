using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class SurviveFailPanel : MonoBehaviour
{
	public Text txtMsg;
	public Text txtGem;

	public GameObject rootReward;
	public GameObject rootRevive;

	public GameObject gemRerward;
	public GameObject siliconRerward;
	public GameObject metalRerward;

	public Text gemCount;
	public Text siliconCount;
	public Text metalCount;

	public GameObject tagNewRecord;

	public GameObject btnGemRevive;
	public GameObject btnAdRevive;

    int gem;
    int silicon;
    int metal;

	public void Show(){
		VolumeManager.ins.PauseBgm();
 		gameObject.SetActive(true);

 		txtMsg.text = string.Format(I18N.instance.getValue("@UI.StageLoseSurvive"),BattleManager.ins.engine.wave-1);
 		txtGem.text = UserData.data.gem+"";

 		if(BattleManager.ins.reviveCount>=BattleManager.ins.battleModifier.reviveMaxCount || BattleManager.ins.reviveCount==-1){
 			ShowReward();
 		}


        #if UNITY_ANDROID || UNITY_IPHONE

 			btnGemRevive.SetActive(false);
 			btnAdRevive.SetActive(true);

        #endif
	}

 	public void Home(){
 		UserData.Save();
        LoadingMask.ins.LoadSceneAsync("SurviveScene");
 	}

 	public void Restart(){
        LoadingMask.ins.LoadSceneAsync("BattleScene");
 	}

 	public void ShowReward(){
 		if(BattleManager.ins.gameMode == 2){
 			if(BattleManager.ins.engine.wave-1>UserData.data.highScoreFieldRunner){
 				UserData.data.highScoreFieldRunner = BattleManager.ins.engine.wave-1;
 				tagNewRecord.SetActive(true);
 			}
 		}
 		else if(BattleManager.ins.gameMode == 3){
 			if(BattleManager.ins.engine.wave-1>UserData.data.highScoreSurvive){
 				UserData.data.highScoreSurvive = BattleManager.ins.engine.wave-1;
 				tagNewRecord.SetActive(true);
 			}
 		}

 		rootReward.SetActive(true);
 		rootRevive.SetActive(false);

 		var waveFactor = (BattleManager.ins.engine.wave-1)*5;
        var waveFactor2 = (BattleManager.ins.engine.wave - 1) / 10;


         gem = 2+150*(waveFactor)/100 * (100 + waveFactor2 * 20) / 100;
 		silicon = 1+25*(waveFactor)/100 * (100 + waveFactor2 * 20) / 100;
        metal = 1+20*(waveFactor)/100 * (100 + waveFactor2 * 20) / 100;

        if (BattleManager.ins.engine.wave == 1)
        {
            gem = 0;
            silicon = 0;
            metal = 0;
        }

        #if UNITY_ANDROID || UNITY_IPHONE

            gem/=2;
            silicon = silicon*2/3;
            metal = metal*2/3;

        #endif
        



 		UserData.data.gem+=gem;
 		UserData.data.silicon+=silicon;
 		UserData.data.metal+=metal;

 		gemCount.text = "x"+gem;
 		siliconCount.text = "x"+silicon;
 		metalCount.text = "x"+metal;

 		UserData.Save();
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
