using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillSpike : BaseSkill
{
	public int damageRate;
	public string bulletName;

	FP t;
	FP cd;

	public override void Init(SkillData data){
		base.Init(data);
		
		damageRate = data.paras[0];
		bulletName = data.namePara;
		cd = new FP(1)/4;
	}

	public override void Step(FP dt){
		if(t>0)t-=dt;
	}

	public override void OnHurt(int damage, BaseUnit damageSource){	
		if(t>0)return;
		t=cd;
		var angle = TSRandom.instance.NextFP()*2*TSMath.Pi;

		var offset = Utils.VectorRotate(new TSVector(0,0,30)/100,angle);
		var aimPos = Utils.VectorRotate(new TSVector(0,0,150)/100,angle);

	    var bullet = BattleManager.ins.CreateBullet(bulletName);
	    bullet.damage = unit.weapons[0].damage*damageRate / 100;	
        bullet.Enter(unit.position+new TSVector(0,unit.radius,0),unit,null,null,unit.position+aimPos);
        unit.OnShotBullet(bullet);
	}
}
