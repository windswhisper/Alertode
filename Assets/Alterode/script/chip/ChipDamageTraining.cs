using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipDamageTraining : BaseChip
{
	public int upgradeNum;
	public int damageRate;
	public int killNum;
	public int totaldamageRate;

	public override void Init(ChipData data){
		base.Init(data);

		this.upgradeNum = data.paras[0];
		this.damageRate = data.paras[1];
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		killNum = 0;
		totaldamageRate = 0;
	}


	public override void OnKill(BaseUnit target){
		killNum++;
		if(killNum>=upgradeNum){
			killNum = 0;
			totaldamageRate += damageRate;
			recordParas[0] = totaldamageRate;
		}
	}

	public override int OnGetDamageIncreasePercent(){
		return totaldamageRate;
	}
}
