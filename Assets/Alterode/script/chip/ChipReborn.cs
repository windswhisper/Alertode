using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipReborn : BaseChip
{
	public int hpRate;

	bool isTrigger;

	public override void Init(ChipData data){
		base.Init(data);

		this.hpRate = data.paras[0];
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		isTrigger = false;
	}

	public override void OnDie(BaseUnit damageSource){
		if(!isTrigger){
			isTrigger = true;

		    var rebornBullet = BattleManager.ins.CreateBullet("BulletReborn");
		    ((BulletReborn)rebornBullet).hpRate = hpRate;	
		    rebornBullet.Enter(build.position,build,null,build,build.position);
		}
	}

	public override void OnDayStart(){
		isTrigger = false;
	}
}
