using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffStun : BaseBulletBuff
{
	FP duration;

	public BulletBuffStun(FP duration){
		this.duration = duration;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Stunned","");
    	target.AddBuff(buff,duration);
	}
}
