using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillNukeLauncher : BaseUltimateSkill
{


	public override void Equiped(BaseUnit unit){
		base.Equiped(unit);

		isReady = true;
	}

	public override void Step(FP dt){
		bool ready = isReady;
		base.Step(dt);

		if(!ready&&isReady){
        	if(((BaseBuild)unit).view!=null)((BuildViewCage)((BaseBuild)unit).view).Reset();
		}
	}

	public override void Cast(TSVector position){
		base.Cast(position);
		t = cd*100/(100+unit.GetFireSpeedIncreasePercent());
        if(((BaseBuild)unit).view!=null)((BaseBuild)unit).view.Fire();
		
	    var bullet = BattleManager.ins.CreateBullet("NukeMissile");
	 	bullet.damage = unit.weapons[0].GetDamage();	
	    bullet.Enter(position,unit,null,null,position);
	    unit.OnShotBullet(bullet);
	}
    
}
