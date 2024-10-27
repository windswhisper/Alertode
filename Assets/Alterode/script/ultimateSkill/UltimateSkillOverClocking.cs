using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillOverClocking : BaseUltimateSkill
{
	public int targetCount;
	public FP buffDuration;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		targetCount = data.paras[0];
		buffDuration = new FP(data.paras[1])/100;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 1;

		var count = 0;

		foreach(var u in BattleManager.ins.engine.unitList){
			if(u.team == TeamType.Self && u.type == UnitType.Building){
				if(((BaseBuild)u).isHero)continue;
				if(count > targetCount)continue;
				if(TSVector.Distance(unit.position,u.position)>3)continue;
				if(u.isDead){
					((BaseBuild)u).Rebuild();
				}
				u.Healed(u.GetMaxHp());
		    	var buff = BattleManager.ins.CreateBuff("OverClocking","");
		    	u.AddBuff(buff,buffDuration);
				count++;
			}
		}

	}

}
