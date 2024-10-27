using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffElectric : BaseBulletBuff
{
	public int damage;

	public BulletBuffElectric(int damage){
		this.damage = damage;
	}

	public override void OnHit(BaseUnit target){
    	target.Hurt(damage,bullet.unit);
		BattleManager.ins.PlayEffect("EffectElectricity",Utils.TSVecToVec3(target.position));
	}

}
