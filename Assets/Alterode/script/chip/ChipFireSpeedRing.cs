using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipFireSpeedRing : BaseChip
{
	public int fireSpeedRate;
	public FP ringRange;

	FP t;

	public override void Init(ChipData data){
		base.Init(data);

		this.fireSpeedRate = data.paras[0];
		this.ringRange = (new FP(1)*data.paras[1])/100;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		t=1;
		AttachBuff();
	}

	public override void OnStep(FP dt){
		t-=dt;
		if(t<0){
			t=1;
			AttachBuff();
		}

	}

	public void AttachBuff(){
	    var buffAnim = BattleManager.ins.CreateBuff("PureAnim","ChipBlueRing");
	    ((BuffPureAnim)buffAnim).SetAnim("EffectRing");
	    build.AddBuff(buffAnim,2);

	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(u.team==build.team && !u.isDead && !u.isTrap){
				if(u.weapons.Count == 0)continue;
	    		var d = (u.position - build.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<ringRange*ringRange){
					var buff = BattleManager.ins.CreateBuff("FireSpeed","ChipFireSpeedRing");
					((BuffFireSpeed)buff).increaseRate = fireSpeedRate;
					u.AddBuff(buff,2);
	            }
	   		}
	    }
	}

}
