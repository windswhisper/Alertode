using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipDamage : BaseChip
{
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.damageRate = data.paras[0];
	}

	public override int OnGetDamageIncreasePercent(){
		return damageRate;
	}
}
