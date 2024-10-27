using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillBlizzard :  BaseUltimateSkill
{
	public int damageRate;
	public int radius;
	public int meteorCount;
	public FP freezeDuration;
	public FP slowDuration;
	public int fireSlowRate;
	public int moveSlowRate;

	FP t = 0;
	FP deltaTime = 0.2;
	int count = 0;
	bool casting = false;
	Coord coord;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		damageRate = data.paras[0];
		radius = data.paras[1];
		meteorCount = data.paras[2];
		freezeDuration = new FP(data.paras[3])/100;
		slowDuration = new FP(data.paras[4])/100;
		fireSlowRate = data.paras[5];
		moveSlowRate = data.paras[6];
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 2;
		coord = new Coord(position.x,position.z);

		if(unit.view!=null){
			((HeroView)unit.view).CastSkill();
			BattleManager.ins.PlayEffect("EffectMagicCircle",Utils.TSVecToVec3(unit.position));
		}

		t=0.1;
		count = 0;
		casting = true;
	}

	public override void Step(FP dt){
		base.Step(dt);

		if(casting){
			t-=dt;
			if(t<0){
				if(count<meteorCount){
					t=deltaTime;
					Drop();
					count++;
					if(count>=meteorCount)casting = false;
				}
			}
		}
	}

	void Drop(){
		var pos = new TSVector(TSRandom.Range(0,radius*2+1)+coord.x-radius,0,TSRandom.Range(0,radius*2+1)+coord.y-radius);

		var bullet = BattleManager.ins.CreateBullet("IceMeteor");
	    bullet.damage = unit.weapons[0].GetDamage()*damageRate/100;
	    bullet.Enter(pos+new TSVector(1,5,1),unit,null,null,pos);
		bullet.AddBuff(new BulletBuffFrozen(freezeDuration));
		bullet.AddBuff(new BulletBuffSlow(slowDuration,fireSlowRate,moveSlowRate));
	}
}
