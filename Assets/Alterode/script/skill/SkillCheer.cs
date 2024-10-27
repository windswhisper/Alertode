using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillCheer : BaseSkill
{
	public FP cheerCd;
	public int speedRate;
	public int fireSpeedRate;
	public FP duration;

	FP t;

	public override void Init(SkillData data){
		base.Init(data);

		cheerCd = new FP(data.paras[0])/100;
		speedRate = data.paras[1];
		fireSpeedRate = data.paras[2];
		duration = new FP(data.paras[3])/100;
	}

	public override void OnAttach(BaseUnit unit){
		base.OnAttach(unit);

		t = cheerCd;
	}

	public override void Step(FP dt){
	    base.Step(dt);

		t-=dt;
		if(t<0){
			t=cheerCd;
			Cry();
		}
	}

	public void Cry(){
		if(unit.view!=null)((BaseMonster)unit).view.PlaySpellAnim();

		unit.fireCd = 2;

	    var bullet = BattleManager.ins.CreateBullet("BulletCheer");
	    bullet.Enter(unit.position,unit,null,null,unit.position);
	    bullet.AddBuff(new BulletBuffCheer(speedRate,fireSpeedRate,duration));
        bullet.affectsAllies = true;
        bullet.affectsEnemies = false;
	}
}
