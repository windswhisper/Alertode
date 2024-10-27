using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipStrength : BaseChip
{
	public int strengthRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.strengthRate = data.paras[0];
	}

	public override void OnEquiped(BaseBuild build){
        var oldHp = build.GetMaxHp();

		base.OnEquiped(build);

        build.Healed(build.GetMaxHp() - oldHp);
	}

	public override int OnGetStrengthIncreasePercent(){
		return strengthRate;
	}
}
