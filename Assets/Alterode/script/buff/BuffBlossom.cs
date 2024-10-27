using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffBlossom : BaseBuff
{
	public int blossomCount;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.blossomCount = data.paras[0];
	}

	public override void OnAttach(BaseUnit unit,FP duration){
		base.OnAttach(unit,duration);

		if(unit.weapons.Count == 0)Remove();
	}

	public override int OnGetBlossomCount(){
		return blossomCount;
	}
}
