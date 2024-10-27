using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Honeti;

public class PausePanel : MonoBehaviour
{
	public Text txtStage;

	void Start(){
		if(BattleManager.ins.gameMode <= 1){
			var data = Configs.GetStage(GlobalData.selectedChapter,GlobalData.selectedStage);
			txtStage.text = I18N.instance.getValue(data.uiName)+" "+string.Format(I18N.instance.getValue("@UI.DifficultInGame"),GlobalData.selectedDifficult)+" "+I18N.instance.getValue(StageScene.modeName[GlobalData.selectedMode]);
		}
		else if(BattleManager.ins.gameMode == 3){
			txtStage.text = I18N.instance.getValue("@UI.SurviveEvo");
		}
		else if(BattleManager.ins.gameMode == 2){
			txtStage.text = I18N.instance.getValue("@UI.FieldRunner");
		}
	}

 	public void Home(){
		if(BattleManager.ins.gameMode <= 1){
	        LoadingMask.ins.LoadSceneAsync("StageScene");
	    }
	    else{
	    	if(BattleManager.ins.engine.wave>1){
	    		BattleManager.ins.reviveCount = -1;
	 			BattleManager.ins.Fail();
	 			gameObject.SetActive(false);
	    	}
 			else{
	        	LoadingMask.ins.LoadSceneAsync("SurviveScene");
 			}
	    }
 	}

 	public void Restart(){
        LoadingMask.ins.LoadSceneAsync("BattleScene");
 	}
}
