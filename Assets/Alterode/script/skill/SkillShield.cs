using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillShield : BaseSkill
{
	public int decreaseRate;

	public override void Init(SkillData data){
		base.Init(data);
		
		decreaseRate = data.paras[0];
	}

	public override int OnGetHurtIncrease(int damage, BaseUnit damageSource){
		if(damageSource==null)return 0;
		var angle = Utils.VectorToAngle(damageSource.position - unit.position);
		if(TSMath.Abs(((angle - unit.turretRotation)+2*TSMath.Pi)%(2*TSMath.Pi))<TSMath.Pi/4){
			return -decreaseRate;
		}
		return 0;
	}

}
