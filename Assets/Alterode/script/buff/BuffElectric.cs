using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffElectric : BaseBuff
{
	public int damage;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.damage = data.paras[0];
	}

	public override void OnShotBullet(BaseBullet bullet){
		bullet.AddBuff(new BulletBuffElectric(damage));
	}
}