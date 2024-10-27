using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeeder : MonoBehaviour
{
	public Animation animation;
	int speed = 1;

    void Start()
    {
        if(animation == null){
        	animation = GetComponent<Animation>();
        }
    }


    void Update()
    {
        if(BattleManager.ins.GetGameSpeed() != speed){
        	speed = BattleManager.ins.GetGameSpeed();
        	foreach (AnimationState state in animation) {
	            state.speed = speed;
	        }
        }
    }
}
