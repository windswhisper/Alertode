using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCheap : BaseChip
{
	int decreaseRate;

	public override void Init(ChipData data){
		base.Init(data);
		
		decreaseRate = data.paras[0];
	}

	public override int OnGetSelfCostMutipleFactorGlobal(){
		return -decreaseRate;
	}
}
