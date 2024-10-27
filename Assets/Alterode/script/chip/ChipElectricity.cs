using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipElectricity : BaseChip
{
	public int damage;
	public FP cd;
	public FP duration;
	public FP range;

	FP t;

	public override void Init(ChipData data){
		base.Init(data);

		this.damage = data.paras[0];
		this.cd = new FP(data.paras[1])/100;
		this.duration = new FP(data.paras[2])/100;
		this.range = new FP(data.paras[3])/100;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

		t = 1;
	}

	public override void OnStep(FP dt){
		t-=dt;
		if(t<0){
			if(AttachBuff()){
				t=cd;
			}
		}

	}

	public bool AttachBuff(){
		var list = new List<BaseUnit>();
	    foreach(var u in BattleManager.ins.engine.unitList){
	    	if(u.team==build.team && !u.isDead && !u.isTrap){
				if(u.weapons.Count == 0 || u==build)continue;
	    		var d = (u.position - build.position);
	            d.y = 0;
	            var disSQ = d.sqrMagnitude;
	            if(disSQ<range*range){
	            	if(!u.HaveBuff("Electric"))list.Add(u);
	            }
	   		}
	    }

	    if(list.Count == 0)return false;

		var buff = BattleManager.ins.CreateBuff("Electric","ChipElectricity");
		((BuffElectric)buff).damage = damage;
		list[TSRandom.Range(0,list.Count)].AddBuff(buff,duration);
		return true;
	}
}
