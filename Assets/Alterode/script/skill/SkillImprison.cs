using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillImprison : BaseSkill
{
	public FP duration;

	public override void Init(SkillData data){
		base.Init(data);

		this.duration = new FP(data.paras[0])/100;
	}

    public override void OnHitTarget(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Imprison","Common");
    	target.AddBuff(buff,duration);
    }
}
