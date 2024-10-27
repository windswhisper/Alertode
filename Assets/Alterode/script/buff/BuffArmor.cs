using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffArmor : BaseBuff
{
	public int decreaseRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.decreaseRate = data.paras[0];
	}

	public override int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
		return -decreaseRate;
	}
	public override void OnRepeatAttach(BaseBuff newBuff,FP duration){
		base.OnRepeatAttach(newBuff,duration);

		if(((BuffArmor)newBuff).decreaseRate > decreaseRate){
			decreaseRate = ((BuffArmor)newBuff).decreaseRate;
		}
	}
}
