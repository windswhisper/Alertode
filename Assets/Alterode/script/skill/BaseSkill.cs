using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseSkill
{
	public BaseUnit unit;

	public SkillData data;

	public virtual void Init(SkillData data){
		this.data = data;
	}

	public virtual void OnAttach(BaseUnit unit){
		this.unit = unit;
	}

	public virtual void Step(FP dt){
	}

	public virtual int OnGetMaxHpIncreasePercent(){
		return 0;
	}

	public virtual int OnGetDamageIncreasePercent(){
		return 0;
	}

	public virtual int OnGetShotSpeedIncreasePercent(){
		return 0;
	}

	public virtual int OnGetRangeIncreasePercent(){
		return 0;
	}

	public virtual int OnGetSpeedIncreasePercent(){
		return 0;
	}

	public virtual int OnGetBlossomCount(){
		return 0;
	}
	
	public virtual int OnGetPierceCount(){
		return 0;
	} 

	public virtual bool OnGetImmune(){
		return false;
	}

    public virtual void OnHitTarget(BaseUnit target){
    }

    public virtual void OnDealDamage(int damage,BaseUnit target){
    }

	public virtual void OnShotBullet(BaseBullet bullet){
	}
	
	public virtual int OnGetHurtIncrease(int damage, BaseUnit damageSource){
		return 0;
	}
	
	public virtual int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
		return 0;
	}

	public virtual void OnUnitGetBuff(BaseBuff buff){
	}

	public virtual void OnHurt(int damage, BaseUnit damageSource){	
	}

	/*返回值为真无法被治疗*/
	public virtual bool IsBanHealed(){
		return false;
	}

	/*返回值为真代表免疫一次死亡*/
	public virtual bool OnDie(BaseUnit damageSource){
		return false;
	}
}
