using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillSetTarget : BaseUltimateSkill
{
	public override void Equiped(BaseUnit unit){
		base.Equiped(unit);

		isReady = true;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		((BuildAirport)unit).SetTarget(new Coord(position.x,position.z));
	}
    
}
