using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletViewStraightLaser : BulletView
{
	public Vector3 startPos;
	public LineRenderer line;

	public virtual void Enter(){
		transform.localPosition =  Utils.TSVecToVec3(bullet.targetPos);
	}

    public void SetStartPos(TSVector p)
    {	
    	startPos = Utils.TSVecToVec3(p);

    }

    public override void Update(){
    	base.Update();

		line.SetPosition(0,startPos);
		line.SetPosition(1,transform.position);
    }
}
