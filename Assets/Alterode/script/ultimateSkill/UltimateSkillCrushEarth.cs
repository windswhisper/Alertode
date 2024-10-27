using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillCrushEarth : BaseUltimateSkill
{
	public int damageRate;
	public FP stunTime;

	bool casting = false;
	FP preTime;
	TSVector originPos;
	TSVector goalPos;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		damageRate = data.paras[0];
		stunTime = new FP(data.paras[1])/100;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 1;
		casting = true;
		if(unit.view!=null)((HeroView)unit.view).CastSkill();
		preTime = 0;
		goalPos = position;
		originPos = unit.position;
	}

	public override void Step(FP dt){
		base.Step(dt);

		if(casting){
			if(preTime<0.15&&preTime+dt>0.15){
	    		if(!BattleManager.ins.battleModifier.isPBMode){
		        	BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(unit.position.x,unit.position.z))] = 0;
	        		BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(goalPos.x,goalPos.z))] = 1;
	        	}

				unit.position = goalPos;
				unit.position.y = 4;
				if(unit.view!=null)((HeroView)unit.view).MoveImmediately();
			}
			preTime+=dt;

			if(preTime<0.15){
				unit.position.y += dt*20;
				return;
			}

			unit.position.y -= dt*20;
			if(unit.position.y<0){
				unit.position.y = 0;
				casting = false;
				Crush();
			}
		}

	}

	void Crush(){
	    var crushBullet = BattleManager.ins.CreateBullet("BulletCrushEarth");
	    crushBullet.damage = unit.weapons[0].GetDamage()*damageRate/100;	
	    crushBullet.aimType = AimType.UnderGround;
	    crushBullet.Enter(unit.position,unit,null,null,unit.position);
	    crushBullet.AddBuff(new BulletBuffStun(stunTime));

	    if(BattleManager.ins.battleModifier.isPBMode){
			unit.position = originPos;
	    }

	}
}
