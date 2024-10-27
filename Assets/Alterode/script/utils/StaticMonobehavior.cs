using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMonobehavior : MonoBehaviour
{
	public static StaticMonobehavior ins;

    void Awake()
    {	
    	if(ins==null){
    		ins = this;
         	DontDestroyOnLoad(this);
    	}
    	else{
    		Destroy(gameObject);
    	}
    }

}
