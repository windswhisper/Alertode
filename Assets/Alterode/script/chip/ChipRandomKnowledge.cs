using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipRandomKnowledge : BaseChip
{
    public string[] chips;

    public override void Init(ChipData data){
        base.Init(data);
        
        chips = data.namePara.Split('/');
    }

    public override void OnBuildOccupied(){
        ((BuildObelisk)build).tech = (chips[(BattleManager.ins.engine.seed+(int)build.position.x+(int)build.position.y)%chips.Length]);
    }
}
