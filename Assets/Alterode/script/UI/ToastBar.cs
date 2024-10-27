using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastBar : MonoBehaviour
{
	public static ToastBar ins;

	public Text txtMsg;

	void Awake(){
		ins = this;
		gameObject.SetActive(false);
	}

	public static void ShowMsg(string msg){
		ins.Show(msg);		
	}

	void Show(string msg){
		txtMsg.text = msg;

		gameObject.SetActive(true);
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
