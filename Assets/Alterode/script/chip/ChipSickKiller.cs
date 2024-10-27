using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSickKiller : BaseChip
{
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.damageRate = data.paras[0];
	}

	public override void OnShotBullet(BaseBullet bullet){
		bullet.AddBuff(new BulletBuffSickKiller(damageRate));
	}
}
