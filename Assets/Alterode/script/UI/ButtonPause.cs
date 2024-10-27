using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : MonoBehaviour
{
	public GameObject activeLight;
	bool isPause = false;

	public void Click(){
		if(!isPause){
			BattleManager.ins.isPause2 = true;
			activeLight.SetActive(true);
		}
		else{
			BattleManager.ins.isPause2 = false;
			activeLight.SetActive(false);
		}
		isPause = !isPause;
	}

	void Update(){
		if(Input.GetKeyUp(KeyCode.P)){
			Click();
		}
	}
}
