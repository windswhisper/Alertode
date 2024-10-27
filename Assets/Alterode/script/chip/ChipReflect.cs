using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipReflect : BaseChip
{
	public int reflectRate;

	public override void Init(ChipData data){
		base.Init(data);
		
		reflectRate = data.paras[0];
	}

	public override void OnHurt(int damage, BaseUnit damageSource){
		if(damageSource!=null && damageSource!=build){
			damageSource.Hurt(damage*reflectRate/100,damageSource);
		}
	}
}
