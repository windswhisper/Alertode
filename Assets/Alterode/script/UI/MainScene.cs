using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

public class MainScene : MonoBehaviour
{
	public SettingPanel settingPanel;

	float escTime = -1;

	void Start(){
        VolumeManager.ins.PlayBgm("The Medieval Sailor - SkyhammerSound");

		#if UNITY_ANDROID
			MobileAdManager.ins.InitAd();
		#endif
	}

	public void StartGame(){
		LoadingMask.ins.LoadSceneAsync("StageScene");
	}

	public void StartSurvive(){
		if(UserData.data.stageProgress["0-2"]==0){
			ToastBar.ShowMsg(I18N.instance.getValue("@UI.CompleteGuideToUnlck"));
			return;
		}

		LoadingMask.ins.LoadSceneAsync("SurviveScene");
	}

	void Update(){
		#if UNITY_ANDROID
		    if(Input.GetKeyUp(KeyCode.Escape))
		    {
		    	if(escTime > 0){
		    		Quit(); 
		    	}
		    	else{
		    		escTime = 2;
		    		ToastBar.ShowMsg(I18N.instance.getValue("@UI.ConfirmQuit"));
		    	}
		    }
		    
		    if(escTime > 0)escTime-=Time.deltaTime;
		#endif
	}

	public void Settings(){
		settingPanel.Show();
	}

	public void Quit(){
		Application.Quit();
	}
}
