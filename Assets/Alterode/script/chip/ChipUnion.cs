using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipUnion : BaseChip
{
    public int armorRate;

    public override void Init(ChipData data){
        base.Init(data);

        armorRate = data.paras[0];
    }

    public override int OnGetHurtMultipleFactor(int damage, BaseUnit damageSource){

        if(damageSource==null)return 0;
        int count = 0;
        foreach(var u in BattleManager.ins.engine.unitList){
            if(u.type == UnitType.Building && u!=build){
                if(((BaseBuild)u).data.name == build.data.name){
                    var d = (u.position - build.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<1.1)count++;
                }
            }
        }

        return -armorRate*count;
    }
}
