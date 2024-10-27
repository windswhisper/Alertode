using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillAcidExplode : BaseSkill
{

    public override void Init(SkillData data){
        base.Init(data);
    }

    public override bool OnDie(BaseUnit damageSource){
        var c = new Coord(unit.position);
        foreach(var obj in BattleMap.ins.mapData.objs){
            if(c.x == obj.x && c.y == obj.y && obj.id == 37)return false;
        }
        BattleMap.ins.AddMapObj(c.x,c.y,37);
        var script = ObjectFactory.CreateMapScript("MapScriptAcidFade");
        script.Init(c.x,c.y,0,"");
        BattleMap.ins.scripts.Add(script);
        return false;
    }
}
