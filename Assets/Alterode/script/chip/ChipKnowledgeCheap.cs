using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipKnowledgeCheap : BaseChip
{
    int decreaseRate;

    public override void Init(ChipData data){
        base.Init(data);
        
        decreaseRate = data.paras[0];
    }

    public override int OnGetCostMutipleFactorGlobal(){
        return -decreaseRate;
    }
}
