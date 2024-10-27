using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffFireSpeed : BaseBuff
{
	public int increaseRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.increaseRate = data.paras[0];
	}
	
	public override int OnGetFireSpeedIncreasePercent(){
		return increaseRate;
	}

	public override void OnRepeatAttach(BaseBuff newBuff,FP duration){
		base.OnRepeatAttach(newBuff,duration);

		if(((BuffFireSpeed)newBuff).increaseRate > increaseRate){
			increaseRate = ((BuffFireSpeed)newBuff).increaseRate;
		}
	}
}
