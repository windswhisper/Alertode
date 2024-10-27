using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipRangeArmor : BaseChip
{
	public int armorRate;
	public int exArmorRate;
	public FP range;

	public override void Init(ChipData data){
		base.Init(data);

		this.armorRate = data.paras[0];
		this.exArmorRate = data.paras[1];
		this.range = new FP(data.paras[2])/100;
	}

	public override int OnGetHurtMultipleFactor(int damage, BaseUnit damageSource){
		var d = (damageSource.position - build.position);
        d.y = 0;
        var disSQ = d.sqrMagnitude;
        if(disSQ>range*range)return -exArmorRate;
		return -armorRate;
	}
}
