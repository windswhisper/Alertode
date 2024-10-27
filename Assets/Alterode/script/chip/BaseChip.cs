using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseChip
{
	public BaseBuild build;

	public ChipData data;

	public List<int> recordParas = new List<int>{0,0,0,0,0};

	public virtual void Init(ChipData data){
		this.data = data;
	}

	public virtual void OnEquiped(BaseBuild build){
		this.build = build;
        build.chips.Add(this);
	}

	public virtual void OnBuildDone(){}

	public virtual void OnStep(FP dt){
	}
	public virtual void OnStepGlobal(FP dt){
	}
	public virtual void OnBuildOccupied(){
	}

	public virtual int OnGetStrengthIncreasePercent(){
		return 0;
	}

	public virtual int OnGetStrengthIncreaseNumber(){
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

	public virtual int OnGetBulletRadiusIncreasePercent(){
		return 0;
	}

	public virtual int OnGetMoneyIncreaseNum(){
		return 0;
	}

	public virtual int OnGetMoneyIncreaseFactor(){
		return 0;
	}

	public virtual int OnGetBurstCount(){
		return 0;
	}

	public virtual int OnGetBlossomCount(){
		return 0;
	}

	public virtual int OnGetBounceCount(){
		return 0;
	}

	public virtual int OnGetPierceCount(){
		return 0;
	}

	public virtual int OnGetChainCount(){
		return 0;
	}

    public virtual void OnHitTarget(BaseUnit target){
    }   

    public virtual void OnDealDamage(int damage,BaseUnit target){
    }

	public virtual int OnGetHurtMultipleFactor(int damage, BaseUnit damageSource){
		return 0;
	}
	
	public virtual void OnHurt(int damage, BaseUnit damageSource){
	}

    public virtual void OnKill(BaseUnit target){
    }

	public virtual void OnDie(BaseUnit damageSource){
	}

	public virtual void OnShotBullet(BaseBullet bullet){	
	}

	public virtual int OnGetSelfCostMutipleFactorGlobal(){
		return 0;
	}

	public virtual int OnGetCostMutipleFactorGlobal(){
		return 0;
	}

	public virtual void OnBoughtGlobal(BaseBuild build){
	}
	public virtual void OnBuyBuildGlobal(BaseBuild build){
	}
	public virtual void OnUpgradeBuildGlobal(BaseBuild build){
	}

	public virtual void OnDayStart(){
	}

	public virtual void OnNightEnd(){
	}


	public virtual bool HasExPara(){
		return false;
	}
	public virtual int GetExtraPara(){
		return 0;
	}
	public virtual void SetExtraPara(int para){
	}

}
