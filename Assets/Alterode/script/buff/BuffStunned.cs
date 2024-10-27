using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffStunned : BaseBuff
{
	public override void OnAttach(BaseUnit unit,FP duration){
		if(unit.type == UnitType.Monster && ((BaseMonster)unit).isBoss)duration/=3;
		
		base.OnAttach(unit,duration);

		if(unit.type == UnitType.Building && unit.weapons.Count == 0)Remove();
	}

	public override bool OnGetStunned(){
		return true;
	}
}
