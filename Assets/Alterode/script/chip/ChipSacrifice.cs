using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipSacrifice : BaseChip
{
	int healRate;
	FP range;

	public override void Init(ChipData data){
		base.Init(data);
		
		healRate = data.paras[0];
		range = new FP(data.paras[1])/100;
	}

	public override void OnDie(BaseUnit damageSource){
		base.OnDie(damageSource);

		BattleManager.ins.PlayEffect("EffectHealRing",Utils.TSVecToVec3(build.position));

	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(u.team==build.team && !u.isDead && !u.isTrap && u!=build){
	    		var d = (u.position - build.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<range*range){
	            	u.Healed(u.GetMaxHp()*healRate/100);
	            }
	   		}
	    }
	}

}
