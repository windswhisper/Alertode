using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterShadowGuard : MonsterWalk
{
	public FP skillCd = 25;
	public FP skillTime = 25;
	public FP stunTime = 8;

	public int stunDamage =30;


    public override void Init(MonsterData data){
    	base.Init(data);

    	skillCd = new FP(data.paras[0])/100;
    	stunTime = new FP(data.paras[1])/100;
    	stunDamage = data.paras[2];
    	skillTime = 0;
    }

	public override void Step(FP dt){
		base.Step(dt);

		skillTime-=dt;
		if(skillTime<=0)
		{
			skillTime = skillCd;
			Cry();
		}
	}

	public void Cry(){
	    var splitBullet = BattleManager.ins.CreateBullet("BulletShadowCry");
	    splitBullet.damage = stunDamage;	
	    splitBullet.Enter(position,this,null,null,position);
	    splitBullet.AddBuff(new BulletBuffStun(stunTime));

	    fireCd = 2;

	    if(view!=null)view.PlaySpellAnim();
	}
}
