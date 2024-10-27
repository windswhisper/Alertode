using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//直线子弹
public class BulletStraightDown : BulletStraight
{
	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
		base.Enter(p,unit,weapon,target,targetPos);

		direction = targetPos - p;
        direction = direction.normalized;
	}

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
        if(position.y<0){
            Explodes();
            return;
        }

        base.Step(dt);

    }

}
