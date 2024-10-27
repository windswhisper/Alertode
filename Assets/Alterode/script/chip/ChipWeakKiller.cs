using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipWeakKiller : BaseChip
{
	public int hpRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.hpRate = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override void OnShotBullet(BaseBullet bullet){
		bullet.AddBuff(new BulletBuffWeakKiller(hpRate,damageRate));
	}
}
