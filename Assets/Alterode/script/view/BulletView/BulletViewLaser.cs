using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletViewLaser : BulletView
{
	public LineRenderer line;

	/*进入场景*/
	public override void Enter(){
		base.Enter();
		line.SetPosition(0,Utils.TSVecToVec3(bullet.position));
		line.SetPosition(1,Utils.TSVecToVec3(bullet.targetPos));
	}
}
