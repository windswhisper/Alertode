using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStop : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.enabled=false;
    }
}
