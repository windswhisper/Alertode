using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipFatal : BaseChip
{
	public int triggerRate;
	public int increaseRate;
	public FP duration;

	public override void Init(ChipData data){
		base.Init(data);

		this.triggerRate = data.paras[0];
		this.increaseRate = data.paras[1];
		this.duration = new FP(data.paras[2])/100;
	}

    public override void OnHitTarget(BaseUnit target){
		if(TSRandom.instance.Next()%100<triggerRate){
	    	var buff = BattleManager.ins.CreateBuff("Fatal","ChipFatal");
	    	((BuffFatal)buff).increaseRate = increaseRate;
	    	target.AddBuff(buff,duration);
	    }
    }
}
