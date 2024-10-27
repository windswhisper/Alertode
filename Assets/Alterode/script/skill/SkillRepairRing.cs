using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillRepairRing : BaseSkill
{
	public int repairRate;
	public FP range;
	public FP repairCd;

	FP t;

	public override void Init(SkillData data){
		base.Init(data);

		repairRate = data.paras[0];
		range = new FP(data.paras[1])/100;
		repairCd = new FP(data.paras[2])/100;
	}

	public override void OnAttach(BaseUnit unit){
		base.OnAttach(unit);

		t = repairCd;
	}

	public override void Step(FP dt){
	    base.Step(dt);

		t-=dt;
		if(t<0){
			t=repairCd;
			Repair();
		}
	}

	public void Repair(){
		BattleManager.ins.PlayEffect("EffectHealRing",Utils.TSVecToVec3(unit.position));

	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(u.team==unit.team && !u.isDead && !u.isTrap){
	    		var d = (u.position - unit.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<range*range){
	            	u.Healed(unit.weapons[0].GetDamage()*repairRate/100);
	            }
	   		}
	    }
	}
}
