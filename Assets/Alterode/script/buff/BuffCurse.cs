using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCurse : BaseBuff
{
	public int damageDecreaseRate;
	public int fireSpeedDecreaseRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.damageDecreaseRate = data.paras[0];
		this.fireSpeedDecreaseRate = data.paras[1];
	}

	public override int OnGetDamageIncreasePercent(){
		return -damageDecreaseRate;
	}
	public override int OnGetFireSpeedIncreasePercent(){
		return -fireSpeedDecreaseRate;
	}
}
