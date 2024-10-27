using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArmor : BaseSkill
{
	public int armor;

	public override void Init(SkillData data){
		base.Init(data);
		
		armor = data.paras[0];
	}

	public override int OnGetHurtIncrease(int damage, BaseUnit damageSource){
		return -armor;
	}
}
