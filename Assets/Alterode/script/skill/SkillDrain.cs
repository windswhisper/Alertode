using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDrain : BaseSkill
{
	public int drainRate;

	public override void Init(SkillData data){
		base.Init(data);
		
		drainRate = data.paras[0];
	}

    public override void OnDealDamage(int damage,BaseUnit target){
    	unit.Healed(damage*drainRate/100);
    }
}
