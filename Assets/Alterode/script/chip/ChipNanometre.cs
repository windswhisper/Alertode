using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipNanometre : BaseChip
{
	public int healRate;
	public FP healCd;

	FP t = 0;

	public override void Init(ChipData data){
		base.Init(data);
		
		healRate = data.paras[0];
		healCd = new FP(data.paras[1])/100;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);
		
		if(!BattleManager.ins.isLoadGame)BattleManager.ins.AddNewTech("#"+build.name,data.name);
	}

	public override void OnStepGlobal(FP dt){
		t-=dt;

		if(t<0){
			t=healCd;
			foreach(var u in BattleManager.ins.engine.unitList){
				if(u.team == TeamType.Self && !u.isDead && u.type == UnitType.Building && !((BaseBuild)u).isHero){
					u.Healed(u.GetMaxHp()*healRate/100);
				}
			}
		}
	}
}
