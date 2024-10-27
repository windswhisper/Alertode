using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipHeat : BaseChip
{
	public int damageRate;

	public override void Init(ChipData data){
		base.Init(data);
		
		damageRate = data.paras[0];
	}

	public override void OnKill(BaseUnit target){

	    var expBullet = BattleManager.ins.CreateBullet("HeatExpBomb");
	    expBullet.damage = target.GetMaxHp()*damageRate/100;	
	    expBullet.Enter(target.position,build,null,null,target.position);
	}
}
