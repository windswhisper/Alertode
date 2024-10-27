using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipManaDrain : BaseChip
{
	int second;

	public override void Init(ChipData data){
		base.Init(data);
		
		second = data.paras[0];
	}

	public override void OnKill(BaseUnit target){
		foreach(var utmSkill in build.utmSkills){
			utmSkill.t-=second;
		}

		BattleManager.ins.PlayEffect("EffectDrainMana",Utils.TSVecToVec3(build.position));
	}
}

