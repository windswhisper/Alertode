using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScene : MonoBehaviour
{
	public GameObject agreementPanel;

    public bool TAP_PLAT;
    public bool TAPPLAY_PLAT;

    void Awake()
    {
        ConfigsManager.Load();
        UserData.Load();
        PlaySetting.Init();
        MobileAdManager.Init();
    }

    void Start(){
		#if UNITY_ANDROID
			if(PlayerPrefs.GetInt("AgreePolicy",0)==0){
        		agreementPanel.SetActive(true);
			}
		#endif
    }

    public void EnterMain(){
        #if UNITY_ANDROID
            if(PlayerPrefs.GetInt("AgreePolicy",0)==0){
                agreementPanel.SetActive(true);
                return;
            }
            if(TAP_PLAT){
                TapLoginManager.Init();
                TapLoginManager.CheckLogin(TAPPLAY_PLAT);
                return;
            }
        #endif

        LoadingMask.ins.LoadSceneAsync("MainScene");
        
    }
}
