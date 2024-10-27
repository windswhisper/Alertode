using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterWerewolf : MonsterWalk
{

	public int rampageHpRate = 40;
	public int jumpDistance = 5;
	public int speedIncreaseRate = 60;

	bool usedSkill = false;

    public override void Init(MonsterData data){
    	base.Init(data);

    	rampageHpRate = data.paras[0];
    	jumpDistance = data.paras[1];
    	speedIncreaseRate = data.paras[2];
    }

	public override void Step(FP dt){
		base.Step(dt);

		if(!usedSkill && hp<GetMaxHp()*rampageHpRate/100){
			Jump();
			usedSkill = true;
		}
	}

	public void Jump(){
	    fireCd = 2;

	    var splitBullet = BattleManager.ins.CreateBullet("BulletShadowCry");
	    splitBullet.damage = 20;	
	    splitBullet.Enter(position,this,null,null,position);

    	var buff = BattleManager.ins.CreateBuff("FireSpeed","MonsterWerewolf");
    	((BuffFireSpeed)buff).increaseRate = speedIncreaseRate;
    	AddBuff(buff,10);
    	var buff2 = BattleManager.ins.CreateBuff("Speed","MonsterWerewolf");
    	((BuffSpeed)buff2).increaseRate = speedIncreaseRate;
    	AddBuff(buff2,10);

	    if(view!=null)view.PlaySpellAnim();
	}
}
