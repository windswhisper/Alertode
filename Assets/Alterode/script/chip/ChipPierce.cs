using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipPierce : BaseChip
{
	public int pierceCount;

	public override void Init(ChipData data){
		base.Init(data);

		this.pierceCount = data.paras[0];
	}

	public override int OnGetPierceCount(){
		return pierceCount;
	}
}
