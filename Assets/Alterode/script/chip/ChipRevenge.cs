using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipRevenge : BaseChip
{
	public int fireSpeedRate;
	public FP duration;
	public int maxLevel;

	public override void Init(ChipData data){
		base.Init(data);

		this.fireSpeedRate = data.paras[0];
		this.duration = new FP(data.paras[1])/100;
		this.maxLevel = data.paras[2];
	}

    public override void OnHurt(int damage, BaseUnit damageSource){
	    var buff = BattleManager.ins.CreateBuff("Revenge","ChipRevenge");
	    ((BuffRevenge)buff).increaseRate = fireSpeedRate;
	    ((BuffRevenge)buff).maxLevel = maxLevel;
	    build.AddBuff(buff,duration);
    }
}