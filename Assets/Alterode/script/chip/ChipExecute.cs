using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipExecute : BaseChip
{
	public int hpRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.hpRate = data.paras[0];
	}

    public override void OnHitTarget(BaseUnit target){
    	if(target.hp>0 && target.hp*100/target.GetMaxHp()<=hpRate){
    		target.hp=0;
    		target.Die(build);
    	}
    }
}
