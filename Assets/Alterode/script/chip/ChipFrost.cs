using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipFrost : BaseChip
{
	public int triggerRate;
	public int fireSlowRate;
	public int moveSlowRate;
	public FP duration;

	public override void Init(ChipData data){
		base.Init(data);

		this.triggerRate = data.paras[0];
		this.fireSlowRate = data.paras[1];
		this.moveSlowRate = data.paras[2];
		this.duration = new FP(data.paras[3])/100;
	}

    public override void OnHitTarget(BaseUnit target){
		if(TSRandom.instance.Next()%100<triggerRate){
	    	var buff = BattleManager.ins.CreateBuff("Slow","");
	    	((BuffSlow)buff).fireSlowRate = fireSlowRate;
	    	((BuffSlow)buff).moveSlowRate = moveSlowRate;
	    	target.AddBuff(buff,duration);
	    }
    }
}
