using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipExpert : BaseChip
{
	public int triggerRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.triggerRate = data.paras[0];
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		if(!BattleManager.ins.isLoadGame)BattleManager.ins.AddNewTech("#"+build.name,data.name);
	}

	public override void OnBuyBuildGlobal(BaseBuild build){
		if(build.isHero)return;
		if(TSRandom.Range(0,100)<triggerRate){
			recordParas[0] += 1;
			build.LevelUp();
		}
	}
}
