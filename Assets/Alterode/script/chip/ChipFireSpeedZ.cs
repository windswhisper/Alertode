using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipFireSpeedZ : BaseChip
{
	public int fireSpeedRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.fireSpeedRate = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override int OnGetFireSpeedIncreasePercent(){
		return fireSpeedRate;
	}
	public override int OnGetDamageIncreasePercent(){
		return -damageRate;
	}
}
