using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillDeathSummon : BaseSkill
{
	public int monsterCount;
	public FP summonDelta;
	public string monsterName;

	int summonCount;
	FP t;

	public override void Init(SkillData data){
		base.Init(data);

		monsterCount = data.paras[0];
		summonDelta = new FP(data.paras[1])/100;
		monsterName = data.namePara;

		summonCount = 0;
	}

	public override bool OnDie(BaseUnit damageSource){
		for(var i=0;i<monsterCount;i++){
			Summon(summonDelta*i+2);
		}

		return base.OnDie(damageSource);
	}

	public void Summon(FP delta){
		var bullet = BattleManager.ins.CreateBullet("BulletSummon2");
		bullet.life = delta;
	    bullet.Enter(unit.position,unit,null,null,unit.position);
		((BulletSummon)bullet).monsterName = monsterName;
	}
}
