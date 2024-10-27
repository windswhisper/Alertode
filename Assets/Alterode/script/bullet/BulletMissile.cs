using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletMissile : BaseBullet
{
	public TSVector direction;
	public BaseUnit target;

	public FP lockTime = 0.25;
	public FP rotateSpeed = 5;

    public int pierceCount;

	public override void Init(BulletData data){
		base.Init(data);

		if(data.paras.Count>0)lockTime = new FP(data.paras[0])/100;
		if(data.paras.Count>1)rotateSpeed = data.paras[1];
	}
    
	public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
        base.Enter(p,unit,weapon,target,targetPos);

		direction = targetPos - p ;
        direction = direction.normalized;

        this.target = target;
        pierceCount = unit.GetPierceCount();
	}


    /*搜索目标*/
    public virtual BaseUnit SearchTarget(){
        FP minDis = 65536;
        BaseUnit targetUnit = null;
    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=unit.team && !u.isDead && !u.IsImmune() && !u.IsUnderGround() && !ignoreList.Contains(u) ){ 
                if(u.type != UnitType.Monster || ( ( ((BaseMonster)u).moveType == MoveType.Fly && aimType == AimType.Air ) || ( ((BaseMonster)u).moveType != MoveType.Fly && aimType == AimType.Ground ) || aimType == AimType.All)){
                    var d = (u.position - position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<minDis){
                        minDis = disSQ;
                        targetUnit = u;
                    }
                }
    		}
    	}
    	return targetUnit;
    }


    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
        base.Step(dt);
        if(isMissed)return;

        if(t>lockTime){
        	if(target!=null && !target.isDead)
        	{
        		direction = Utils.TSRotate(direction,target.position - position + new TSVector(0,target.radius,0),rotateSpeed*dt).normalized;
        	}
        	else{
                target = SearchTarget();
                if(target==null){
                    isMissed = true;
            		return;
                }
        	}
        }

    	position = position + speed*dt*direction;
    	DetectCollision();
    }

    public override void Hit(BaseUnit target){
        base.Hit(target);

        ignoreList.Add(target);
    }
}
