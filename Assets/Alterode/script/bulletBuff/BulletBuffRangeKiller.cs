using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffRangeKiller : BaseBulletBuff
{
	int damageRate;

	public BulletBuffRangeKiller(int damageRate){
		this.damageRate = damageRate;
	}

	public override int OnGetHitDamageIncrease(BaseUnit target){
		if(bullet.unit == null)return 0;
        if(target.team == bullet.team)return 0;

		var d = (target.position - bullet.unit.position);
        d.y = 0;
        FP dis = d.magnitude;
		return FP.ToInt(dis*damageRate);
	}
}
