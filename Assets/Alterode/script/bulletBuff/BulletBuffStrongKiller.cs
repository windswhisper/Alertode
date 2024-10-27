using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuffStrongKiller : BaseBulletBuff
{
	int hpRate;
	int damageRate;

	public BulletBuffStrongKiller(int hpRate,int damageRate){
		this.hpRate = hpRate;
		this.damageRate = damageRate;
	}

	public override int OnGetHitDamageIncrease(BaseUnit target){
        if(target.team == bullet.team)return 0;
		if(target.hp > target.GetMaxHp()*hpRate/100){
			return damageRate;
		}
		return 0;
	}

}
