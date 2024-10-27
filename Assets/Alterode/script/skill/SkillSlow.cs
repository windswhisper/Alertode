using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillSlow : BaseSkill
{
	public int fireSlowRate;
	public int moveSlowRate;
	public FP duration;

	public override void Init(SkillData data){
		base.Init(data);

		this.fireSlowRate = data.paras[0];
		this.moveSlowRate = data.paras[1];
		this.duration = new FP(data.paras[2])/100;
	}

    public override void OnHitTarget(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Slow","");
    	((BuffSlow)buff).fireSlowRate = fireSlowRate;
    	((BuffSlow)buff).moveSlowRate = moveSlowRate;
    	target.AddBuff(buff,duration);
    }
}
