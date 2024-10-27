using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class AllyUAV : MonsterFly
{
    BaseBuild build;

    public override void Enter(TSVector p,TeamType team){
        base.Enter(p,team);

        flyHeight = new FP(Configs.commonConfig.flyHeight + 20)/100 ;
        targetPos = p;
        mobileFire = true;
    }

    public void BindBuild(BaseBuild build){
        this.build = build;
    }

    public void SetTargetPos(TSVector pos){
        targetPos = pos;
    }


    public override void Step(FP dt){
        base.Step(dt);
    }
}
