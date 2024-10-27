using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSplit : BaseChip
{
	public int splitCount;
	public int damagePercent;


	public override void Init(ChipData data){
		base.Init(data);

		this.splitCount = data.paras[0];
		this.damagePercent = data.paras[1];
	}

	public override void OnShotBullet(BaseBullet bullet){
		bullet.AddBuff(new BulletBuffSplit(splitCount,damagePercent));
	}

}
