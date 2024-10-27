using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseBuff
{
	public BaseUnit unit;

	public BuffData data;

	public FP duration;

	public bool isRemove = false;

	public bool isForever = false;

	public BuffView view;

	public string source;

	public virtual void Init(BuffData data,string source){
		this.data = data;
		this.source = source;
		isForever = data.isForever;
	}

	public virtual void OnAttach(BaseUnit unit,FP duration){
		this.unit = unit;
		this.duration = duration;
		this.isRemove = false;

		if(view!=null)view.OnAttach();
	}

	public virtual void OnRepeatAttach(BaseBuff newBuff,FP duration){
		this.duration = duration;
	}

	public virtual void Step(FP dt){
		if(isRemove)return;
		if(!isForever)duration -= dt;
		OnStep(dt);
		if(!isForever&&duration<0)Remove();
	}

	public virtual void OnStep(FP dt){
	}

	public virtual bool OnGetStunned(){
		return false;
	}

	public virtual void OnShotBullet(BaseBullet bullet){	
	}

	public virtual int OnGetMaxHpIncreasePercent(){
		return 0;
	}

	public virtual int OnGetDamageIncreasePercent(){
		return 0;
	}

	public virtual int OnGetFireSpeedIncreasePercent(){
		return 0;
	}

	public virtual int OnGetRangeIncreasePercent(){
		return 0;
	}

	public virtual int OnGetMoveSpeedIncreasePercent(){
		return 0;
	}

	public virtual int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
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

	public virtual bool OnGetInviso(){
		return false;
	}

	public virtual void Remove(){
		isRemove = true;
		if(view!=null)view.Remove();
	}

}
