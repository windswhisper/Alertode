using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildViewWalker : BuildView
{
    protected override void Update()
    {
    	base.Update();
        
        if(build.isDead)return;
        
        Utils.RotateSmooth(transform,new Vector3(0,(float)build.yRotation*180/Mathf.PI,0),Utils.LerpByTime(Time.deltaTime));

    	if(Vector3.Distance(transform.localPosition, Utils.TSVecToVec3(build.position))> 0.01f){
    		transform.localPosition = Vector3.Lerp(transform.localPosition,Utils.TSVecToVec3(build.position),Utils.LerpByTime(Time.deltaTime));
    	}
    	else{
    		transform.localPosition = Utils.TSVecToVec3(build.position);
    	}
        animator.SetBool("moving",((BuildWalker)build).isMoving && !build.IsStunned());
    }

}
