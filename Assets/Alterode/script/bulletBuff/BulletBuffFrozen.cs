using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffFrozen : BaseBulletBuff
{
	FP duration;

	public BulletBuffFrozen(FP duration){
		this.duration = duration;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Frozen","");
    	target.AddBuff(buff,duration);
	}
}