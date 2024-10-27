using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletPersistent : BulletInviso
{
    public FP damageCd = 0;

    public override void Step(FP dt){
        base.Step(dt);

        if(!isMissed){
            damageCd-=dt;
            if(damageCd<=0){
                damageCd = 0.5;
                DetectCollision();
            }
        }
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
                        Hit(target);
                    }
                }
            }
        }
    }
}
