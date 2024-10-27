using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAgreement : MonoBehaviour
{
	public void ReadPolicy(){
		Application.OpenURL("https://docs.qq.com/doc/DTVh4SHRaQUVST3hK");
	}

	public void Cancel(){
		Application.Quit();
	}

	public void RequestPermission(){
        PlayerPrefs.SetInt("AgreePolicy",1);
		MobileAdManager.ins.RequestPermission();
	}
}
