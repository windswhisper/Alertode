using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillLightningStrike :  BaseUltimateSkill
{
	public int damageRate;
	public int lightningCount;

	FP t = 0;
	FP deltaTime = 0.5;
	int count = 0;
	bool casting = false;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		damageRate = data.paras[0];
		lightningCount = data.paras[1];
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 2;

		if(unit.view!=null){
			((HeroView)unit.view).CastSkill();
			BattleManager.ins.PlayEffect("EffectMagicCircle",Utils.TSVecToVec3(unit.position));
		}


		t=0.5;
		count = 0;
		casting = true;
	}

	public override void Step(FP dt){
		base.Step(dt);

		if(casting){
			t-=dt;
			if(t<0){
				if(count<lightningCount){
					t=deltaTime;
					Strike();
					count++;
					if(count>=lightningCount)casting = false;
				}
			}
		}
	}

	void Strike(){
		BaseUnit target = null;
		int minHp = -1;
    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=unit.team && !u.isDead && !u.IsImmune() && !u.IsUnderGround()){
                if(u.hp<minHp || minHp == -1){
                	target = u;
                	minHp = u.hp;
                }
    		}
    	}
    	if(target!=null){
		    var bullet = BattleManager.ins.CreateBullet("LightningStrike");
		    bullet.damage = unit.weapons[0].GetDamage()*damageRate/100;	
		    bullet.Enter(target.position,unit,null,null,target.position);
    	}
	}
}
