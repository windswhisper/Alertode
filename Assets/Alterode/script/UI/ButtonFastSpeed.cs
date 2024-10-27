using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFastSpeed : MonoBehaviour
{
	public GameObject activeLight;
	bool isFaster = false;

	public void Click(){
		if(!isFaster){
			BattleManager.ins.SetGameSpeed(3);
			activeLight.SetActive(true);
		}
		else{
			BattleManager.ins.SetGameSpeed(1);
			activeLight.SetActive(false);
		}
		isFaster = !isFaster;
	}

	void Update(){
		if(Input.GetKeyUp(KeyCode.Space)){
			Click();
		}
	}
}
