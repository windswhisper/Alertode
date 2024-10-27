using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipPurify : BaseChip
{
	public int coinNum;

	public override void Init(ChipData data){
		base.Init(data);

		this.coinNum = data.paras[0];
	}

	public override int OnGetMoneyIncreaseNum(){
		return coinNum;
	}
}
