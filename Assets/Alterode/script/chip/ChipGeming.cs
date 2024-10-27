using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipGeming : BaseChip
{
	public int gemCount;

	public override void Init(ChipData data){
		base.Init(data);

		this.gemCount = data.paras[0];
	}


	public override void OnNightEnd(){
		UserData.data.gem+=gemCount;
		BattleManager.ins.PlayGemEffect(gemCount,Utils.TSVecToVec3(build.position));
	}

}
