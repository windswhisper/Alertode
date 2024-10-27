using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseBulletBuff
{
	public BaseBullet bullet;

	public virtual void Attach(BaseBullet bullet){
		this.bullet = bullet;
	}

	public virtual void OnStep(FP dt){
	}

	public virtual int OnGetHitDamageIncrease(BaseUnit target){
		return 0;
	}

	public virtual void OnHit(BaseUnit target){
	}

	public virtual void OnExplode(){
	}

	public virtual bool IsDisCollide(){
		return false;
	}
}
