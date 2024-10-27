using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillWalk : BaseUltimateSkill
{


	public override void Equiped(BaseUnit unit){
		base.Equiped(unit);

		isReady = true;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		((BuildWalker)unit).MoveTo(new Coord(position.x,position.z));
	}
    
}
