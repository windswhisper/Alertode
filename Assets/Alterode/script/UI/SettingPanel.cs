using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class PlaySetting{
	public static int FXLevel;

	public static void Init(){
		FXLevel =  PlayerPrefs.GetInt("FXLevel",3);
	}
	public static void Save(){
		PlayerPrefs.SetInt("FXLevel",FXLevel);
	}
}

public class SettingPanel : MonoBehaviour
{
	public Slider sliderMusic;
	public Slider sliderSound;

	public GameObject[] languageSigns;
	public string[] languageName;
	public GameObject btnHotKey;
	public GameObject btnDataExport;
	public GameObject dataLoader;
	public Text txtFxLevel;

	static string[] strFXLevel = {"@UI.Low","@UI.Medium","@UI.High","@UI.VeryHigh"};

	int cc = 0;

	void Start(){
		#if UNITY_ANDROID || UNITY_IPHONE
			btnHotKey.SetActive(false);
		#else
			if(btnDataExport!=null)btnDataExport.SetActive(false);
		#endif
	}

	public void Show(){
		gameObject.SetActive(true);

		sliderMusic.value = VolumeManager.ins.musicVolume;
		sliderSound.value = VolumeManager.ins.soundVolume;

		if(languageSigns!=null && languageSigns.Length!=0){
			foreach(var sign in languageSigns){
				sign.SetActive(false);
			}
			languageSigns[(int)(I18N.instance.gameLang)].SetActive(true);
		}

		if(txtFxLevel != null){
			txtFxLevel.text = I18N.instance.getValue(strFXLevel[PlaySetting.FXLevel]);
		}
	}

	public void Hide(){
		gameObject.SetActive(false);
	}

	public void OnDragMusicBar(){
		VolumeManager.ins.SetMusicVolume(sliderMusic.value);
	}
	public void OnDragSoundBar(){
		VolumeManager.ins.SetSoundVolume(sliderSound.value);
	}

	public void SetLanguage(int langCode){
		I18N.instance.setLanguage(languageName[langCode]);
		foreach(var sign in languageSigns){
			sign.SetActive(false);
		}
		languageSigns[langCode].SetActive(true);
	}

	public void Done(){
		VolumeManager.ins.Save();
		Hide();
	}

	public void ShowDataLoader(){
		cc++;
		if(cc>10){
			dataLoader.SetActive(true);
		}
	}

	public void LoadDatas(){
		UserData.LoadFromString(GUIUtility.systemCopyBuffer);
	}

	public void ExportData(){
		if(GlobalData.tapUserProfile!=null)UserData.data.openId=GlobalData.tapUserProfile.openid;
		GUIUtility.systemCopyBuffer = UserData.Export();

		ToastBar.ShowMsg("已导出至粘贴板");
	}

	public void SwitchFxLevel(){
		PlaySetting.FXLevel = (PlaySetting.FXLevel-1);
		if(PlaySetting.FXLevel<0)PlaySetting.FXLevel=3;
		txtFxLevel.text = I18N.instance.getValue(strFXLevel[PlaySetting.FXLevel]);
		PlaySetting.Save();
	}
}
