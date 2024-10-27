using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillDamageRing : BaseSkill
{
	public int damageRage;
	public FP range;

	FP t;

	public override void Init(SkillData data){
		base.Init(data);

		damageRage = data.paras[0];
		range = new FP(data.paras[1])/100;
	}

	public override void OnAttach(BaseUnit unit){
		base.OnAttach(unit);
	}

	public override void Step(FP dt){
	    base.Step(dt);

		t-=dt;
		if(t<0){
			t=1;
			AttachBuff();
		}
	}

	public void AttachBuff(){
	    var buffAnim = BattleManager.ins.CreateBuff("PureAnim","SkillDamageRing");
	    ((BuffPureAnim)buffAnim).SetAnim("EffectRingRed");
	    unit.AddBuff(buffAnim,2);

	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(u.team==unit.team && !u.isDead && !u.isTrap){
				if(u.weapons.Count == 0)continue;
	    		var d = (u.position - unit.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<range*range){
					var buff = BattleManager.ins.CreateBuff("Damage","SkillDamageRing");
					((BuffDamage)buff).increaseRate = damageRage;
					u.AddBuff(buff,2);
	            }
	   		}
	    }
	}
}