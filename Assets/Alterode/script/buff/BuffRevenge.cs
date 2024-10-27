using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffRevenge : BaseBuff
{
	public int increaseRate;
	public int maxLevel;
	public int level;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.increaseRate = data.paras[0];
		this.maxLevel = data.paras[1];
	}
	
	public override void OnAttach(BaseUnit unit,FP duration){
		base.OnAttach(unit,duration);

		level = 0;
	}

	public override void OnRepeatAttach(BaseBuff newBuff,FP duration){
		base.OnRepeatAttach(newBuff,duration);

		if(level<maxLevel)level++;
	}

	public override int OnGetFireSpeedIncreasePercent(){
		return increaseRate*level;
	}
}
