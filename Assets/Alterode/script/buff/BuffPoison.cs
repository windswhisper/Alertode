using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffPoison : BaseBuff
{
	public int damage;
	public int maxLevel;

	int level;

	FP delta = 1;

	FP t = 0;

	public override void Init(BuffData data,string source){
		base.Init(data,source);

		this.damage = data.paras[0];
		this.maxLevel = data.paras[0];
	}

	public virtual void OnAttach(BaseUnit unit,FP duration){
		base.OnAttach(unit,duration);
		t=0;
		level = 1;
	}

	public override void OnStep(FP dt){
		base.OnStep(dt);

		t-=dt;

		if(t<0){
			t=delta;
			unit.Hurt(damage*level,null);
			if(level<maxLevel)level++;
		}
	}
}
