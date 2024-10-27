using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipLens : BaseChip
{
    int radiusRate;
    int damageRate;
    int linkRange;

    public override void Init(ChipData data){
        base.Init(data);

        this.radiusRate = data.paras[0];
        this.damageRate = data.paras[1];
        this.linkRange = data.paras[2];
    }

    public override void OnBuildDone(){
        CombineLens();
    }
    public override void OnDayStart(){
        CombineLens();
    }

    public void CombineLens(){
        foreach(var unit in BattleManager.ins.engine.unitList){
            if(unit.type == UnitType.Building && ((BaseBuild)unit).data.name == build.data.name){
                var c = new Coord(build.position.x,build.position.z);
                var c2 = new Coord(unit.position.x,unit.position.z);
                if((c.x == c2.x && Math.Abs(c.y-c2.y)<=linkRange && Math.Abs(c.y-c2.y)>1) || (c.y == c2.y && Math.Abs(c.x-c2.x)<linkRange && Math.Abs(c.x-c2.x)>1)) {
                    bool isRepeat = false;
                    foreach(var b in BattleManager.ins.engine.bulletList){
                        if(b.data.name == "LensWall" && !b.isMissed){
                            var otherLens = (BulletLensWall)b;
                            if((otherLens.c1.EqualTo(c) && otherLens.c2.EqualTo(c2)) || (otherLens.c1.EqualTo(c2) && otherLens.c2.EqualTo(c))){
                                isRepeat = true;
                                break;
                            }
                        }
                    }
                    if(isRepeat)continue;
                    var bullet = BattleManager.ins.CreateBullet("LensWall");
                    bullet.Enter(build.position+new TSVector(0,1,0),build,null,unit,unit.position);
                    ((BulletLensWall)bullet).InitLens(damageRate,radiusRate,(BaseBuild)unit,build);
                }
            }
        }
    }
}
