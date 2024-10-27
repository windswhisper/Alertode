using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipPatch : BaseChip
{
	public FP buffDuration;

	public override void Init(ChipData data){
		base.Init(data);

		this.buffDuration = new FP(data.paras[0])/100;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		if(!BattleManager.ins.isLoadGame)BattleManager.ins.AddNewTech("#"+build.name,data.name);
	}

	public override void OnUpgradeBuildGlobal(BaseBuild build){
		if(build.isHero)return;
		build.AddBuff(BattleManager.ins.CreateBuff("OverClocking",""),buffDuration);
		build.Healed(build.GetMaxHp());
	}
}
