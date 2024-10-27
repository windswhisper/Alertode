using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDrain : BaseChip
{
	public int drainRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.drainRate = data.paras[0];
	}

    public override void OnDealDamage(int damage,BaseUnit target){
    	build.Healed(damage*drainRate/100);
    }
}
