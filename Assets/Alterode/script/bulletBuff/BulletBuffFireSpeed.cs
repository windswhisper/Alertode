using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffFireSpeed : BaseBulletBuff
{
	int fireSpeedRate;
	FP duration;
	string buffSource;

	public BulletBuffFireSpeed(int fireSpeedRate, FP duration,string buffSource){
		this.duration = duration;
		this.fireSpeedRate = fireSpeedRate;
		this.buffSource = buffSource;
	}

	public override void OnHit(BaseUnit target){
        if(target.team == bullet.team){
	    	var buff = BattleManager.ins.CreateBuff("FireSpeed", buffSource);
	    	target.AddBuff(buff,duration);
	    }
	}

    public override int OnGetHitDamageIncrease(BaseUnit target){
        if(target.team == bullet.team){
            return -9999;
        }
        return 0;
    }
}