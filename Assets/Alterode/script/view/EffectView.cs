using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectView : MonoBehaviour
{

	public float removeDelay;
	public string effectName;
	float t;

	public void Init(string name){
		effectName = name;
		t = 0;
	}

    void Update()
    {
        t+=Time.deltaTime;
        if(removeDelay!=-1 && t>removeDelay)Remove();
    }

    void Remove(){
    	ObjectPool.ins.PutEffect(this);
    }
}
