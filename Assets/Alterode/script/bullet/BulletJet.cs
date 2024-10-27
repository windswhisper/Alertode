using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//喷射型子弹，对一条直线上的敌人造成伤害，不能用于防空和反潜
public class BulletJet : BaseBullet
{
	FP jetRange;

    public override void Init(BulletData data){
    	base.Init(data);
    	jetRange = new FP(data.paras[0])/100;
    }


    public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
        base.Enter(p,unit,weapon,target,targetPos);

        jetRange = jetRange*(100 + largePercent)/100;
    }

    /*逻辑帧更新，立刻到达目标点，并击中目标*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
    	base.Step(dt);
        if(t>=life){
        	DetectCollision();
        	Miss();
        }
    }


    /*碰撞检测*/
    public override void DetectCollision(){
        foreach(var buff in buffs){
            if(buff.IsDisCollide()){
                return;
            }
        }
        
    	var p = new TSVector2(position.x,position.z);
    	var d = targetPos - position;
    	var dir= d.normalized;
    	var dir2 = new TSVector2(dir.z,-dir.x);
    	var p2 = p+new TSVector2(dir.x,dir.z)*jetRange;
    	TSVector2 ra;
    	TSVector2 rb;
    	TSVector2 rc;
    	TSVector2 rd;
    	TSVector2 mp2;
        foreach(var target in BattleManager.ins.engine.unitList){
            if(!target.isDead && !target.IsImmune()){
                if(((target.team!=unit.team  && affectsEnemies)||(target.team==unit.team  && affectsAllies)) && target.IsOnGround() ){
				    ra = p+dir2*(radius+target.radius)/2;
				    rb = p-dir2*(radius+target.radius)/2;
				    rc = p2-dir2*(radius+target.radius)/2;
				    rd = p2+dir2*(radius+target.radius)/2;
				    mp2 = new TSVector2(target.position.x,target.position.z);
					if(Utils.IsPointInRect(mp2,ra,rb,rc,rd)){
                        Hit(target);
                    }
                }
            }
        }

    }
}

