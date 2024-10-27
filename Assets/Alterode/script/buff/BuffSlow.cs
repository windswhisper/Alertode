using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffSlow : BaseBuff
{
	public int fireSlowRate;
	public int moveSlowRate;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.fireSlowRate = data.paras[0];
		this.moveSlowRate = data.paras[1];
	}

	public override void OnAttach(BaseUnit unit,FP duration){
		if(unit.type == UnitType.Monster && ((BaseMonster)unit).isBoss)duration/=3;
		
		base.OnAttach(unit,duration);
	}


	public override int OnGetFireSpeedIncreasePercent(){
		return -fireSlowRate;
	}
	public override int OnGetMoveSpeedIncreasePercent(){
		return -moveSlowRate;
	}
}
