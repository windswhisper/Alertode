using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipValue : BaseChip
{
	public int coinRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.coinRate = data.paras[0];
	}


	public override int OnGetMoneyIncreaseFactor(){
		return coinRate;
	}
}
