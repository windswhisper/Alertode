using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipClear : BaseChip
{
	int rangeRate;
	int fireSpeedRate;
	FP range;
	FP t;

	public override void Init(ChipData data){
		base.Init(data);
		
		rangeRate = data.paras[0];
		fireSpeedRate = data.paras[1];
		range = new FP(data.paras[2])/100;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		t=1;
		AttachBuff();
	}

	public override int OnGetRangeIncreasePercent(){
		return rangeRate;
	}
	
	public override void OnStep(FP dt){
		t-=dt;
		if(t<0){
			t=1;
			AttachBuff();
		}
	}

	public void AttachBuff(){
		bool isClear = true;

	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(!u.isDead && !u.isTrap && u!=build){
	    		var d = (u.position - build.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<range*range){
	            	isClear = false;
	            }
	   		}
	    }

	    if(isClear){
			var buff = BattleManager.ins.CreateBuff("FireSpeed","ChipClear");
			((BuffFireSpeed)buff).increaseRate = fireSpeedRate;
			build.AddBuff(buff,2);
	    }
	}
}
