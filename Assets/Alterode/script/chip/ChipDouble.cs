using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDouble : BaseChip
{
	public int burstCount;
	public int damageDecrease;

	public override void Init(ChipData data){
		base.Init(data);

		this.burstCount = data.paras[0];
		this.damageDecrease = data.paras[1];
	}

	public override int OnGetBurstCount(){
		return burstCount;
	}

	public override int OnGetDamageIncreasePercent(){
		return -damageDecrease;
	}
}
