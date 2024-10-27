using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillFrostExplode : BaseSkill
{
	FP frostDuration;
	int spikeCount;
	int spikeDamageRate;

	public override void Init(SkillData data){
		base.Init(data);

		this.frostDuration = new FP(data.paras[0])/100;
		this.spikeCount = data.paras[1];
		this.spikeDamageRate = data.paras[2];
	}

	public override bool OnDie(BaseUnit damageSource){
		var bullet = BattleManager.ins.CreateBullet("FrostBomb");
        bullet.Enter(unit.position+new TSVector(0,unit.radius,0),unit,null,null,unit.position);
		bullet.AddBuff(new BulletBuffFrozen(frostDuration));
        bullet.damage = unit.weapons[0].GetDamage()*spikeDamageRate/100;
	    bullet.AddBuff(new BulletBuffFrostExpSpike(4,50));


		return false;
	}
}
