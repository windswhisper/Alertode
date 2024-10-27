using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillPoison : BaseSkill
{
	public int damage;
	public int maxLevel;
	public FP duration;

	public override void Init(SkillData data){
		base.Init(data);

		this.damage = data.paras[0];
		this.maxLevel = data.paras[1];
		this.duration = new FP(data.paras[2])/100;
	}

    public override void OnHitTarget(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Poison","Common");
    	((BuffPoison)buff).damage = damage;
    	((BuffPoison)buff).maxLevel = maxLevel;
    	target.AddBuff(buff,duration);
    }
}
