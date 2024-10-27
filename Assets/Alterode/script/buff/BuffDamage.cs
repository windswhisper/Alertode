using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffDamage : BaseBuff
{
	public int increaseRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.increaseRate = data.paras[0];
	}

	public override void OnAttach(BaseUnit unit,FP duration){
		base.OnAttach(unit,duration);

		if(unit.weapons.Count == 0)Remove();
	}

	public override int OnGetDamageIncreasePercent(){
		return increaseRate;
	}

	public override void OnRepeatAttach(BaseBuff newBuff,FP duration){
		base.OnRepeatAttach(newBuff,duration);

		if(((BuffDamage)newBuff).increaseRate > increaseRate){
			increaseRate = ((BuffDamage)newBuff).increaseRate;
		}
	}
}
