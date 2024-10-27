using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//抛物线子弹，life为总飞行时间，life越小抛得越快
public class BulletArcing : BaseBullet
{
	public TSVector direction;
	public FP flySpeed;
	public FP accVertical;
	public FP velVertical;

	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
		base.Enter(p,unit,weapon,target,targetPos);

		var distanceVec = targetPos - p;

        distanceVec.y = 0;

        flySpeed = TSVector.Distance(new TSVector(0,0,0), distanceVec) / life ;

        direction = distanceVec.normalized;

		accVertical = -speed/(life*life)*2f;
		velVertical = speed/life;
	}

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
    	t+=dt;

		if((aimType != AimType.UnderWater && position.y > 0) || (aimType == AimType.UnderWater && position.y > -1) || velVertical>0 ){
			position += direction * flySpeed * dt;
			position += new TSVector(0,velVertical*dt,0);
			velVertical+=accVertical*dt;
			DetectCollision();
		}
		else{
			Explodes();
		}
	}

	public override FP PredictDelay(TSVector selfPos, TSVector targetPos)
	{
		return life;
	}
}
