using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffCurse : BaseBulletBuff
{
	FP duration;

	public BulletBuffCurse(FP duration){
		this.duration = duration;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Curse","");
    	target.AddBuff(buff,duration);
	}
}