using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//所属方
public enum TeamType{
    //友方
    Self,
    //敌方
    Enemy,
    //中立方
    Neutral,
}

//单位类型
public enum UnitType{
    //建筑
    Building,
    //怪物
    Monster,
}

//优先选择目标
public enum TargetPrefer{
    //最近
    Closest,
    //最强
    Strongest,
    //最弱
    Weakest,
    //最远
    Farest
}

//基础单位类
public class BaseUnit
{
    //名称
    public string name = "Unit0";
    //最大生命值
	public int maxHp = 50;
    //体积半径
    public FP radius = 0.5;
    //攻击前摇
    public FP fireUp = 0;
    //是否是陷阱
    public bool isTrap = false;

    //当前生命值
    public int hp = 50;
    //是否已死亡
    public bool isDead = false;
    //当前坐标
    public TSVector position = new TSVector(0,0,0);
    //旋转角度,起始角度为z轴
    public FP yRotation = 0;
    //是否拥有炮塔
    public bool hasTurret = false;
    //炮塔旋转角度
    public FP turretRotation = 0;
    //所属方
    public TeamType team = TeamType.Self;
    //武器
    public List<BaseWeapon> weapons = new List<BaseWeapon>();
    //装填冷却计时
    public FP fireCd = 0;
    //开火前摇计时
    public FP fireUpTime = 0;
    //视图对象
    public UnitView view;
    //单位类型
    public UnitType type;
    //优先选择目标
    public TargetPrefer targetPrefer;
    //状态列表
    public List<BaseBuff> buffList;
    //被动技能列表
    public List<BaseSkill> skillList;
  
    /*初始化*/
    public virtual void Init(){
        hp = maxHp;
        isDead = false;
        yRotation = 0;
        weapons = new List<BaseWeapon>();
        fireCd = 0;
        fireUpTime = 0;
        buffList = new List<BaseBuff>();
        skillList = new List<BaseSkill>();
    }

    /*进入场景*/
    /*参数列表：位置，所属方*/
    public virtual void Enter(TSVector p,TeamType team){
        this.position = new TSVector(p.x,p.y,p.z);
        this.team = team;

        targetPrefer = TargetPrefer.Closest;
    }

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public virtual void Step(FP dt){

        foreach(var skill in skillList){
            skill.Step(dt);
        }
        foreach(var buff in buffList){
            buff.Step(dt);
        }
        for(var i=buffList.Count-1;i>=0;i--)
        {
            if(buffList[i].isRemove){
                RemoveBuff(buffList[i]);
            }
        }

        if(IsStunned()){
            return;
        }

        fireCd-=dt;
        bool isFireUp = false;
        for(var i = weapons.Count-1;i>=0;i--){
            weapons[i].Step(dt);
            if(weapons[i].isFireUp)isFireUp = true;
        }
        if(!isFireUp)fireUpTime = 0;
        else fireUpTime+=dt;
    }

    /*被治疗结算*/
    /*参数列表： 伤害量*/
    public virtual void Healed(int value){
        foreach(var skill in skillList){
            if(skill.IsBanHealed())return;;
        }

        hp+=value;
        var mhp = GetMaxHp();
        if(hp>mhp)hp = mhp;
    }

    public virtual int GetHurtPlusFactor(int damage, BaseUnit damageSource){
        var plusFactor = 0;
        foreach(var skill in skillList){
            plusFactor += skill.OnGetHurtIncrease(damage,damageSource);
        }

        return plusFactor;
    }

    public virtual int GetHurtMultipleFactor(int damage, BaseUnit damageSource){
        var multipleFactor = 0;
        foreach(var buff in buffList){
            multipleFactor += buff.OnGetHurtIncreasePercent(damage,damageSource);
        }

        return multipleFactor;
    }

    /*受伤结算*/
    /*参数列表： 伤害量，伤害来源*/
    public virtual int Hurt(int damage, BaseUnit damageSource){
        if(isDead)return 0;
        
        if(damage>0){
            var plusFactor = GetHurtPlusFactor(damage,damageSource);
            var multipleFactor = GetHurtMultipleFactor(damage,damageSource);

            if(multipleFactor<-90)multipleFactor=-90;

            damage = damage*(100+multipleFactor) / 100 + plusFactor ;
            if(damage<1)damage = 1;
        }

        if(damage>0){
            hp-=damage;

            foreach(var skill in skillList){
                skill.OnHurt(damage,damageSource);
            }

            if(damageSource!=null)damageSource.OnDealDamage(damage,this);
            if(view!=null){
                view.PlayHurtAnim();
                BattleManager.ins.PlayHitLabel(damage,0,Utils.TSVecToVec3(GetCenterPos()));
            }
        }


        if(hp<=0){
            hp=0;
            Die(damageSource);
        }


        return damage;
    }

    public virtual void OnDealDamage(int damage, BaseUnit target){
        foreach(var skill in skillList){
            skill.OnDealDamage(damage,target);
        }
    }

    /*死亡结算*/
    /*参数列表： 伤害来源*/
    public virtual void Die(BaseUnit damageSource){
        isDead = true;

        foreach(var skill in skillList){
            if(skill.OnDie(damageSource)){
                isDead = false;
                break;
            }
        }

        for(var i=buffList.Count-1;i>=0;i--)
        {
            if(!buffList[i].isRemove && !buffList[i].isForever)
                buffList[i].Remove();
        }

        if(isDead){
            if(damageSource!=null)damageSource.OnKill(this);
        }
    }

    public virtual void OnKill(BaseUnit target){
    }

    public virtual int GetMaxHp(){
        return maxHp;
    }
    
    public virtual FP GetFireUp(){
        return fireUp*100/(100+GetFireSpeedIncreasePercent());
    }

    public virtual bool IsWeaponDisable(){
        return false;
    }

    public virtual int GetFireSpeedIncreasePercent(){
        var percent = 0;
        foreach(var buff in buffList){
            percent+=buff.OnGetFireSpeedIncreasePercent();
        }
        return percent;
    }

    public virtual int GetDamageIncreasePercent(){
        var percent = 0;
        foreach(var buff in buffList){
            percent+=buff.OnGetDamageIncreasePercent();
        }
        return percent;
    }
    
    public virtual int GetRangeIncreasePercent(){
        var percent = 0;
        foreach(var buff in buffList){
            percent+=buff.OnGetRangeIncreasePercent();
        }
        return percent;
    }

    public virtual int GetMoveSpeedIncreasePercent(){
        var percent = 0;
        foreach(var buff in buffList){
            percent+=buff.OnGetMoveSpeedIncreasePercent();
        }
        return percent;
    }

    public virtual int GetBulletRadiusIncreasePercent(){
        return 0;
    }

    public virtual int GetBurstCount(){
        return 0;
    }

    public virtual int GetBlossomCount(){
        var count = 0;
        foreach(var skill in skillList){
            count+=skill.OnGetBlossomCount();
        }
        foreach(var buff in buffList){
            count+=buff.OnGetBlossomCount();
        }
        return count;
    }
    
    public virtual int GetPierceCount(){
        var count = 0;
        foreach(var skill in skillList){
            count+=skill.OnGetPierceCount();
        }
        foreach(var buff in buffList){
            count+=buff.OnGetPierceCount();
        }
        return count;
    }
    
    public virtual int GetChainCount(){
        return 0;
    }

    public virtual void OnFire(BaseUnit target){
        
    }

    public virtual void OnHitTarget(BaseUnit target){
        foreach(var skill in skillList){
            skill.OnHitTarget(target);
        }
    }
    public virtual void OnShotBullet(BaseBullet bullet){
        foreach(var skill in skillList){
            skill.OnShotBullet(bullet);
        }
        foreach(var buff in buffList){
            buff.OnShotBullet(bullet);
        }
    }

    public bool AddBuff(BaseBuff buff,FP duration){
        foreach(var b in buffList){
            if(b.data.name == buff.data.name && buff.source == b.source){
                b.OnRepeatAttach(buff,duration);
                buff.Remove();
                return false;
            }
        }
        buffList.Add(buff);
        buff.OnAttach(this,duration);

        foreach(var skill in skillList){
            skill.OnUnitGetBuff(buff);
        }
        return true;
    }

    public virtual void OnDayStart(){
        
    }
    public virtual void OnNightEnd(){
        
    }

    public TSVector GetCenterPos(){
        return position+new TSVector(0,radius,0);
    }

    public bool IsUnderGround(){
        return position.y<-0.6;
    }

    public bool IsOnGround(){
        return position.y>-0.6 && position.y<1;
    }

    public bool IsImmune(){
        foreach(var b in  buffList){
            if(b.OnGetImmune())return true;
        }
        foreach(var skill in  skillList){
            if(skill.OnGetImmune())return true;
        }

        return isTrap;
    }

    public bool IsInviso(){
        foreach(var b in  buffList){
            if(b.OnGetInviso())return true;
        }

        return false;
    }

    public bool IsStunned(){
        foreach(var buff in buffList){
            if(buff.OnGetStunned()){
                return true;
            }
        }

        return false;
    }

    public bool HaveBuff(string name){
        foreach(var buff in buffList){
            if(buff.data.name == name){
                return true;
            }
        }
        return false;
    }

    public void RemoveBuff(BaseBuff buff){
        buffList.Remove(buff);
    }

}
