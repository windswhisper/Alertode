using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffCheer : BaseBulletBuff
{
	int speedRate;
	int fireSpeedRate;
	FP duration;

	public BulletBuffCheer(int speedRate, int fireSpeedRate, FP duration){
		this.duration = duration;
		this.speedRate = speedRate;
		this.fireSpeedRate = fireSpeedRate;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("FireSpeed","BulletBuffCheer");
    	((BuffFireSpeed)buff).increaseRate = fireSpeedRate;
    	target.AddBuff(buff,duration);
    	var buff2 = BattleManager.ins.CreateBuff("Speed","BulletBuffCheer");
    	((BuffSpeed)buff2).increaseRate = fireSpeedRate;
    	target.AddBuff(buff2,duration);
	}
}