using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillCurse : BaseSkill
{
	public FP duration;

	public override void Init(SkillData data){
		base.Init(data);
		
		duration = new FP(data.paras[0])/100;
	}

	public override bool OnDie(BaseUnit damageSource){
		var bullet = BattleManager.ins.CreateBullet("BulletCurse");
        bullet.Enter(unit.position,unit,null,damageSource,unit.position+new TSVector(0,1,0));
        bullet.AddBuff(new BulletBuffCurse(duration));
		return false;
	}
}
