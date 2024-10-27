using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffSlow : BaseBulletBuff
{
	public int fireSlowRate;
	public int moveSlowRate;
	FP duration;

	public BulletBuffSlow(FP duration, int fireSlowRate, int moveSlowRate){
		this.duration = duration;
		this.fireSlowRate = fireSlowRate;
		this.moveSlowRate = moveSlowRate;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Slow","");
	    ((BuffSlow)buff).fireSlowRate = fireSlowRate;
	    ((BuffSlow)buff).moveSlowRate = moveSlowRate;
    	target.AddBuff(buff,duration);
	}
}
