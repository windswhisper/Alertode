using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffPoison : BaseBulletBuff
{
	int damage;
	int maxLevel;
	FP duration;

	public BulletBuffPoison(int damage, int maxLevel, FP duration){
		this.duration = duration;
		this.damage = damage;
		this.maxLevel = maxLevel;
	}

	public override void OnHit(BaseUnit target){
    	var buff = BattleManager.ins.CreateBuff("Poison","");
	    ((BuffPoison)buff).damage = damage;
	    ((BuffPoison)buff).maxLevel = maxLevel;
    	target.AddBuff(buff,duration);
	}
}