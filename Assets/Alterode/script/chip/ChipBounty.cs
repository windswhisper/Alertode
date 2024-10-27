using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipBounty : BaseChip
{	
	int amount;

	int bountCountFrame = 0;

	public override void Init(ChipData data){
		base.Init(data);
		
		amount = data.paras[0];
	}

	public override void OnStep(FP dt){
		if(bountCountFrame>0){
			BattleManager.ins.PlayCoinEffect(bountCountFrame,Utils.TSVecToVec3(build.position));
			bountCountFrame=0;
		}
	}


	public override void OnKill(BaseUnit target){
		BattleManager.ins.engine.AddCoin(amount);
		bountCountFrame += amount;
		recordParas[0] += amount;
	}
}
