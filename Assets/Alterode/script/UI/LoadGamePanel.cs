using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class LoadGamePanel : MonoBehaviour
{
	public Text txtInfo;

	public void Show(int type){
		gameObject.SetActive(true);
		
		GlobalData.saveGameType = type;

		var data = SaveGame.Load(type);

		if(data.gameMode<2){
			var stage = Configs.GetStage(data.chapterId,data.stageId);
			txtInfo.text = I18N.instance.getValue(stage.uiName)+" "+string.Format(I18N.instance.getValue("@UI.DifficultInGame"),data.difficult)+" "+I18N.instance.getValue(StageScene.modeName[data.gameMode]);
		}
		else if(data.gameMode == 3){
			txtInfo.text = I18N.instance.getValue("@UI.SurviveEvo");
		}
		else if(data.gameMode == 2){
			txtInfo.text = I18N.instance.getValue("@UI.FieldRunner");
		}
		txtInfo.text += " "+string.Format(I18N.instance.getValue("@UI.Wave2"),data.wave);
	}

	public void Load(){
		GlobalData.isLoadGame = true;

        LoadingMask.ins.LoadSceneAsync("BattleScene");
	}
}
