using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipRange : BaseChip
{
	public int rangeRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.rangeRate = data.paras[0];	
	}

	public override int OnGetRangeIncreasePercent(){
		return rangeRate;
	}
	
}
