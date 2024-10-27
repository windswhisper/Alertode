using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipChain : BaseChip
{	
	int count;

	public override void Init(ChipData data){
		base.Init(data);
		
		count = data.paras[0];
	}

	public override int OnGetChainCount(){
		return count;
	}
}
