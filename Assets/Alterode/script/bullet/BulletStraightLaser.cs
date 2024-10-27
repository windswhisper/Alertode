using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//扫射激光
public class BulletStraightLaser : BulletStraight
{
    public TSVector startPos;

	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
		base.Enter(p,unit,weapon,target,targetPos);

        pierceCount = 99;
        startPos = p;

        position = targetPos;

        if(view!=null){
            ((BulletViewStraightLaser)view).SetStartPos(startPos);
        }

	}


}
