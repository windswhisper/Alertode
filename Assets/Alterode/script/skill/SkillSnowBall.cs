using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillSnowBall : BaseSkill
{
	public int fireSlowRate;
	public int moveSlowRate;
	public FP slowTime;
	public int frozenRate;
	public FP frozenTime;

	int frozenCount = 0;

	public override void Init(SkillData data){
		base.Init(data);
		
		fireSlowRate = data.paras[0];
		moveSlowRate = data.paras[1];
		slowTime = new FP(data.paras[2])/100;
		frozenRate = data.paras[3];
		frozenTime =  new FP(data.paras[4])/100;
	}

	public override void OnShotBullet(BaseBullet bullet){
		if(TSRandom.instance.Next()%100<frozenRate){
			bullet.AddBuff(new BulletBuffFrozen(frozenTime));
		}
		bullet.AddBuff(new BulletBuffSlow(slowTime,fireSlowRate,moveSlowRate));
	}
}
