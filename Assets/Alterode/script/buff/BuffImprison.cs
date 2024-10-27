using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffImprison : BaseBuff
{
	public override void Init(BuffData data,string source){
		base.Init(data,source);
	}

	public override void OnAttach(BaseUnit unit,FP duration){
		if(unit.type == UnitType.Monster && ((BaseMonster)unit).isBoss)duration/=3;
		
		base.OnAttach(unit,duration);
	}

	public override int OnGetMoveSpeedIncreasePercent(){
		return -100;
	}
}
