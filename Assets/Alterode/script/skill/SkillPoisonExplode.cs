using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillPoisonExplode : BaseSkill
{
	int damage;
	int maxLevel;
	FP duration;

	public override void Init(SkillData data){
		base.Init(data);

		this.damage = data.paras[0];
		this.maxLevel = data.paras[1];
		this.duration = new FP(data.paras[2])/100;
	}

	public override bool OnDie(BaseUnit damageSource){
		var bullet = BattleManager.ins.CreateBullet("PoisonBomb");
        bullet.Enter(unit.position,unit,null,null,unit.position);
		bullet.AddBuff(new BulletBuffPoison(damage,maxLevel,duration));
        bullet.damage = unit.GetMaxHp()*(unit.GetDamageIncreasePercent()+100)/200;
		return false;
	}
}
