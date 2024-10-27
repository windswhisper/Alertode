using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSwallow : BaseChip
{
	public int hpIncrease;

	public int totalHpIncrease;

	public override void Init(ChipData data){
		base.Init(data);

		this.hpIncrease = data.paras[0];

		totalHpIncrease = 0;
	}

	public override int OnGetStrengthIncreaseNumber(){
		return totalHpIncrease;
	}

	public override void OnKill(BaseUnit target){
		totalHpIncrease += hpIncrease;
		recordParas[0] = totalHpIncrease;

		build.Healed(hpIncrease);

		BattleManager.ins.PlayEffect("EffectHeal",Utils.TSVecToVec3(build.position));
	}

	public override bool HasExPara(){
		return true;
	}
	public override int GetExtraPara(){
		return totalHpIncrease;
	}
	public override void SetExtraPara(int para){
		totalHpIncrease = para;
	}
}
