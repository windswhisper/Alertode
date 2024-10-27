using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipExplosion : BaseChip
{
	int damageRate;

	public override void Init(ChipData data){
		base.Init(data);
		
		damageRate = data.paras[0];
	}

	public override void OnDie(BaseUnit damageSource){
		base.OnDie(damageSource);

	    var expBullet = BattleManager.ins.CreateBullet("TerrorBomb");
	    expBullet.damage = build.GetMaxHp()*damageRate/100;	
	    expBullet.Enter(build.position,build,null,null,build.position);
	}

}
