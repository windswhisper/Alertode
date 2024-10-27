using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{
	public bool isShort = false;
	public float delay = 0;

    void OnEnable()
    {
    	if(delay==0){
        	CameraEffect.ins.Shake(isShort);
    	}
    	else{
    		StartCoroutine(Shake());
    	}
    }

    IEnumerator Shake(){
    	yield return new WaitForSeconds(delay/BattleManager.ins.GetGameSpeed());
        CameraEffect.ins.Shake(isShort);
    }

}
