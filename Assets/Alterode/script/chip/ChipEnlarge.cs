using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipEnlarge : BaseChip
{
	public int largeRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.largeRate = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override int OnGetBulletRadiusIncreasePercent(){
		return largeRate;
	}
	
	public override int OnGetDamageIncreasePercent(){
		return damageRate;
	}
}
