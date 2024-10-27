using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletStraightPierce : BulletStraight
{
	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
		base.Enter(p,unit,weapon,target,targetPos);

        pierceCount = 9999;
	}

}
