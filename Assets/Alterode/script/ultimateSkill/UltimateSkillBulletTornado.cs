using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class UltimateSkillBulletTornado : BaseUltimateSkill
{
	public int fireSpeedRate;
	public int blossomCount;
	public int pierceCount;
	public int damageRate;
	public FP duration;

	public override void Init(UltimateSkillData data){
		base.Init(data);

		fireSpeedRate = data.paras[0];
		blossomCount = data.paras[1];
		pierceCount = data.paras[2];
		damageRate = data.paras[3];
		duration = new FP(data.paras[4])/100;
	}

	public override void Cast(TSVector position){
		base.Cast(position);

		unit.fireCd = 0;

	    var buff = BattleManager.ins.CreateBuff("FireSpeed","BulletTornado");
	    ((BuffFireSpeed)buff).increaseRate = fireSpeedRate;
	    unit.AddBuff(buff,duration);

	    buff = BattleManager.ins.CreateBuff("Blossom","BulletTornado");
	    ((BuffBlossom)buff).blossomCount = blossomCount;
	    unit.AddBuff(buff,duration);

	    buff = BattleManager.ins.CreateBuff("Pierce","BulletTornado");
	    ((BuffPierce)buff).pierceCount = pierceCount;
	    unit.AddBuff(buff,duration);

	    buff = BattleManager.ins.CreateBuff("Damage","BulletTornado");
	    ((BuffDamage)buff).increaseRate = damageRate;
	    unit.AddBuff(buff,duration);


	}
}
