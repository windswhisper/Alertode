using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExplode : BaseSkill
{
	int damageRate;
	string bulletName;

	public override void Init(SkillData data){
		base.Init(data);

		this.damageRate = data.paras[0];
		bulletName = data.namePara;
	}

	public override bool OnDie(BaseUnit damageSource){
		var bullet = BattleManager.ins.CreateBullet(bulletName);
        bullet.Enter(unit.position,unit,null,null,unit.position);
        bullet.damage = unit.weapons[0].GetDamage()*damageRate/100;
		return false;
	}
}
