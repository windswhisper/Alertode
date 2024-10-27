using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuildAirport : BaseBuild
{
    AllyUAV myUAV;
    Coord targetPos;

    public override void OnBuildDone(){
        base.OnBuildDone();

        targetPos = new Coord(position);
        if(myUAV==null || myUAV.isDead)
        {
            SummonUAV();
        }
    }

    public override void Step(FP dt){
        fireCd = 999;
        base.Step(dt);
    }

    void SummonUAV(){
        var coord = new Coord(position);
        myUAV = (AllyUAV)(BattleManager.ins.PutAlly("UAV",coord.x,coord.y));
        myUAV.BindBuild(this);
        myUAV.SetTargetPos(new TSVector(targetPos.x,0,targetPos.y));
    }

    public void SetTarget(Coord pos){
        targetPos = pos;
        if(myUAV!=null && !myUAV.isDead)
        {
            myUAV.SetTargetPos(new TSVector(pos.x,0,pos.y));
        }
    }

    public override void OnDayStart(){
        base.OnDayStart();

        if(myUAV==null || myUAV.isDead)
        {
            SummonUAV();
        }
    }
}
