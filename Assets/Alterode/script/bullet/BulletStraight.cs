using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//直线子弹
public class BulletStraight : BaseBullet
{
	public TSVector direction;
    public int pierceCount;

	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
		base.Enter(p,unit,weapon,target,targetPos);

		direction = targetPos - p;
        direction.y = 0;
        direction = direction.normalized;

        pierceCount = unit.GetPierceCount();
	}

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
        base.Step(dt);
        if(isMissed)return;

    	position = position + speed*dt*direction;
    	DetectCollision();
    }

    public override void DetectCollision(){
        foreach(var buff in buffs){
            if(buff.IsDisCollide()){
                return;
            }
        }

        foreach(var target in BattleManager.ins.engine.unitList){
            if(!target.isDead && !target.IsImmune() && !ignoreList.Contains(target)){
                if((target.team!=unit.team  && affectsEnemies)||(target.team==unit.team  && affectsAllies)){
                    var p = target.position;
                    if(target.IsOnGround())p.y=0;
                    if(TSVector.DistanceSQ(position,p + new TSVector(0,target.radius,0)) <= (radius+target.radius)*(radius+target.radius) ){
                        if(explodeRadius == 0)Hit(target);
                        if(ignoreList.Count>pierceCount)Explodes();
                        return;
                    }
                }
            }
        }
    }

    public override void Hit(BaseUnit target){
        base.Hit(target);

        ignoreList.Add(target);
    }

    public override FP PredictDelay(TSVector selfPos, TSVector targetPos)
    {
        return TSVector.Distance(selfPos, targetPos) / speed;
    }
}
