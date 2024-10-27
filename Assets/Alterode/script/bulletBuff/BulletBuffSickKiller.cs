using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuffSickKiller : BaseBulletBuff
{
	int damageRate;

	public BulletBuffSickKiller(int damageRate){
		this.damageRate = damageRate;
	}

	public override int OnGetHitDamageIncrease(BaseUnit target){
        if(target.team != bullet.team){
			foreach(var buff in target.buffList){
				if(buff.data.isNegative)
					return damageRate;
			}
		}

		return 0;
	}
}
