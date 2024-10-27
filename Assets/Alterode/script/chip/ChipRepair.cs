using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipRepair : BaseChip
{
	public FP repairCd;
	public int repairRate;

	FP repairTime;

	public override void Init(ChipData data){
		base.Init(data);

		this.repairCd = new FP(data.paras[0]);
		this.repairRate = data.paras[1];
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		repairTime = 0;
	}

	public override void OnStep(FP dt){
		repairTime+=dt;
		
		var maxHp = build.GetMaxHp();
		if(repairTime>repairCd && build.hp<maxHp)
		{
			repairTime = 0;
			build.Healed(maxHp*repairRate/100);
			BattleManager.ins.PlayEffect("EffectHeal",Utils.TSVecToVec3(build.position));
		}	

	}

}
