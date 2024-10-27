using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public enum TargetType{
	None,
	Any,
	Ground,
	Allies
}

public class BaseUltimateSkill
{
	public UltimateSkillData data;
	public BaseUnit unit;
	public FP cd;
	public TargetType targetType;

	public FP t = 0;
	public bool isReady = false;

	public virtual void Init(UltimateSkillData data){
		this.data = data;
		this.cd = new FP(data.cd)/100;
		this.targetType = (TargetType)data.targetType;

		t=cd/2;
	}

	public virtual void Step(FP dt){
		t-=dt;

		if(t<=0)isReady = true;
	}

	public virtual void Equiped(BaseUnit unit){
		this.unit =  unit;
	}

	public virtual void Cast(TSVector position){
		t=cd;
		isReady = false;

		if(((BaseBuild)unit).isHero){
			if(unit.view!=null){
				((HeroView)unit.view).PlaySkillVoice(data.name);
				BattleManager.ins.ultimateSkillShow.Play(((BaseBuild)unit).data.name);
			}
		}
	}
}
