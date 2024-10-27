using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAllPower : BaseChip
{
	public int strengthRate;
	public int damageRate;
	public int fireSpeedRate;
	public int costRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.strengthRate = data.paras[0];
		this.damageRate = data.paras[1];
		this.fireSpeedRate = data.paras[2];
		this.costRate = data.paras[3];
	}

	public override void OnEquiped(BaseBuild build){
        var oldHp = build.GetMaxHp();

		base.OnEquiped(build);

        build.Healed(build.GetMaxHp() - oldHp);
	}

	public override int OnGetStrengthIncreasePercent(){
		return strengthRate;
	}

	public override int OnGetDamageIncreasePercent(){
		return damageRate;
	}

	public override int OnGetFireSpeedIncreasePercent(){
		return fireSpeedRate;
	}

	public override int OnGetSelfCostMutipleFactorGlobal(){
		return costRate;
	}
}
