using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipFury : BaseChip
{
	public int hpRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.hpRate = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override int OnGetDamageIncreasePercent(){
		if(build.hp*100/build.GetMaxHp()<hpRate){
			return damageRate;
		}
		return 0;
	}
}
