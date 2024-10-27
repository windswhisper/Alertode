using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBlossom : BaseChip
{
	public int bulletCount;
	public int damageDecrease;

	public override void Init(ChipData data){
		base.Init(data);

		this.bulletCount = data.paras[0];
		this.damageDecrease = data.paras[1];
	}

	public override int OnGetBlossomCount(){
		return bulletCount;
	}

	public override int OnGetDamageIncreasePercent(){
		return -damageDecrease;
	}
}
