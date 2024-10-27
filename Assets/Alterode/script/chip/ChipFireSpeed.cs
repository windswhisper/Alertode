using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipFireSpeed : BaseChip
{
	public int fireSpeedRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.fireSpeedRate =data.paras[0];
	}

	public override int OnGetFireSpeedIncreasePercent(){
		return fireSpeedRate;
	}
}
