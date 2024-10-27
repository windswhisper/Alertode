using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpeeder : MonoBehaviour
{
	ParticleSystem[] list;
	int speed = 1;


    void Start()
    {
        if(list == null || list.Length == 0){
        	list = gameObject.GetComponentsInChildren<ParticleSystem>();
        }
    }

    void Update()
    {
        if(BattleManager.ins.GetGameSpeed() != speed){
        	speed = BattleManager.ins.GetGameSpeed();
	        foreach(var particleSystem in list){
	        	particleSystem.playbackSpeed = speed;
	        }
        }
    }
}
