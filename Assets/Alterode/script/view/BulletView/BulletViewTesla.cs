using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletViewTesla : BulletView
{
	public EffectChainLightning chain;

	/*进入场景*/
	public override void Enter(){
		base.Enter();
		chain.Init(Utils.TSVecToVec3(bullet.position),Utils.TSVecToVec3(bullet.targetPos));
	}
}
