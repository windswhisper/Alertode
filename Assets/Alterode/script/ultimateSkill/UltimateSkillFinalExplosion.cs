using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillFinalExplosion :  BaseUltimateSkill
{
	public int damageRate;
	public FP stunDuration;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		damageRate = data.paras[0];
		stunDuration = new FP(data.paras[1])/100;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 3;

		if(unit.view!=null){
			((HeroView)unit.view).CastSkill();
			BattleManager.ins.PlayEffect("EffectMagicCircle",Utils.TSVecToVec3(unit.position));
		}

		var bullet = BattleManager.ins.CreateBullet("FinalExplosion");
	    bullet.damage = unit.weapons[0].GetDamage()*damageRate/100;
	    bullet.Enter(position,unit,null,null,position);
	    
    	var buff = BattleManager.ins.CreateBuff("Stunned","");
    	unit.AddBuff(buff,stunDuration);
	}
}
