using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSpeeder : MonoBehaviour
{
	public Animator animator;
    public float speedRate = 1;

    void Start()
    {
        if(animator==null){
        	animator = GetComponent<Animator>();
        }
    }

    void Update()
    {   
        var animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(BattleManager.ins.GetGameSpeed()==0 && (animatorInfo.IsName("BuildUp")||animatorInfo.IsName("Die"))){
            animator.speed = 1;
        }
        else {
            animator.speed = BattleManager.ins.GetGameSpeed()*speedRate;
        }
    }
}
