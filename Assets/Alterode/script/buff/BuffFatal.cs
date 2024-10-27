using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFatal : BaseBuff
{
	public int increaseRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.increaseRate = data.paras[0];
	}

	public override int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
		return increaseRate;
	}
}
