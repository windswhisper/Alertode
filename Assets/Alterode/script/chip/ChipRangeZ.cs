using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipRangeZ : BaseChip
{
	public int rangeRate;
	public int fireSpeedRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.rangeRate = data.paras[0];	
		this.fireSpeedRate = data.paras[1];	
		this.damageRate = data.paras[2];	
	}

	public override int OnGetRangeIncreasePercent(){
		return rangeRate;
	}
	
	public override int OnGetFireSpeedIncreasePercent(){
		return -fireSpeedRate;
	}

	public override int OnGetDamageIncreasePercent(){
		return damageRate;
	}
}
