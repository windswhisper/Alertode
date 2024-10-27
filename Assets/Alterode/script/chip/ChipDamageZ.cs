using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipDamageZ : BaseChip
{
	public int damageRate;
	public int rangeRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.damageRate = data.paras[0];
		this.rangeRate = data.paras[1];
	}

	public override int OnGetDamageIncreasePercent(){
		return damageRate;
	}

	public override int OnGetRangeIncreasePercent(){
		return -rangeRate;
	}
}
