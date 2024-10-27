using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillRollingRock : BaseSkill
{
	public int damageRate;

	public override void Init(SkillData data){
		base.Init(data);
		
		damageRate = data.paras[0];
	}

	public override bool OnDie(BaseUnit damageSource){
		var bullet = BattleManager.ins.CreateBullet("BulletRock");
		if(damageSource!=null){
        	bullet.Enter(unit.position,unit,null,null,damageSource.position);
		}
		else{
        	bullet.Enter(unit.position,unit,null,null,unit.position+new TSVector(0,0,1));
		}
		((BulletStraight)bullet).pierceCount = 10;
        bullet.damage = unit.GetMaxHp()*damageRate/100*(unit.GetDamageIncreasePercent()+100)/100;
		return false;
	}
}
