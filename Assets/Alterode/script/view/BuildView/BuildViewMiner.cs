using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildViewMiner : BuildView
{
	public void ProductCoin(int amount){
		BattleManager.ins.PlayCoinEffect(amount,transform.position);
	}
}
