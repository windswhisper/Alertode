using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterRush : MonsterWalk
{
    int rushSpeedRate;
    FP rushPrepareCd;

    FP prepareTime;

    public bool isRushing;

    public override void Init(MonsterData data){
        base.Init(data);

        rushSpeedRate = data.paras[0];
        rushPrepareCd = new FP(data.paras[1])/100;
    }

    public override void Enter(TSVector p,TeamType team){
        base.Enter(p,team);

        prepareTime = rushPrepareCd;
        isRushing = false;
    }

    public override void Step(FP dt){
        base.Step(dt);

        prepareTime-=dt;

        if(prepareTime<0 && !isRushing){
            isRushing = true;
        }
    }

    public override int Hurt(int damage, BaseUnit damageSource){
        var dmg = base.Hurt(damage,damageSource);

        if(dmg>0){
            prepareTime = rushPrepareCd;
            isRushing = false;
        }
        return dmg;
    }

    public override FP GetSpeed(){
        if(isRushing)
            return base.GetSpeed()*rushSpeedRate/100;

        return base.GetSpeed();
    }
}
