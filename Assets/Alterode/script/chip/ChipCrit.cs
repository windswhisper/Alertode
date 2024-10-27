using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipCrit : BaseChip
{
	public int critRate;
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.critRate = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override void OnShotBullet(BaseBullet bullet){
		if(TSRandom.instance.Next()%100<critRate){
			bullet.damage = bullet.damage*(damageRate+100)/100;
			bullet.explodeFX = "ExplosionStars";
	    }
    }
}
