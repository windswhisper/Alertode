using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOverClocking : BaseBuff
{
	public int fireSpeedRate;
	public int armorRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.fireSpeedRate = data.paras[0];
		this.armorRate = data.paras[1];
	}

	public override int OnGetFireSpeedIncreasePercent(){
		return fireSpeedRate;
	}

	public override int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
		return -armorRate;
	}
}

