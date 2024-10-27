using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipPoison : BaseChip
{
	public int triggerRate;
	public int damage;
	public int maxLevel;
	public FP duration;

	public override void Init(ChipData data){
		base.Init(data);

		this.triggerRate = data.paras[0];
		this.damage = data.paras[1];
		this.maxLevel = data.paras[2];
		this.duration = new FP(data.paras[3])/100;
	}

    public override void OnHitTarget(BaseUnit target){
		if(TSRandom.instance.Next()%100<triggerRate){
	    	var buff = BattleManager.ins.CreateBuff("Poison","");
	    	((BuffPoison)buff).damage = damage;
	    	((BuffPoison)buff).maxLevel = maxLevel;
	    	target.AddBuff(buff,duration);
	    }
    }
}
