using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//基础建筑类
public class BaseBuild : BaseUnit
{
	//显示名称
	public string uiName = "";
	//拔起时间,目前固定为1
	public FP buildUpTime = 1;

    //建筑数据
    public BuildData data;
    //视图层对象
    public BuildView view;
	//流逝时间
	public FP t = 0;
    //芯片
    public List<BaseChip> chips = new List<BaseChip>();
    //建筑等级
    public int level = 1;
    //是否是英雄
    public bool isHero = false;
    //正在建造中
    public bool inBuild = false;
    //主动技能
    public List<BaseUltimateSkill> utmSkills = new List<BaseUltimateSkill>();

    /*初始化*/
    /*参数列表：配置数据*/
    public virtual void Init(BuildData data){
        this.type = UnitType.Building;

        this.data = data;
    	this.name = data.name;
    	this.maxHp = data.strength;
    	this.radius = new FP(data.radius)/100;
        this.fireUp = new FP(data.fireUp)/100;
    	this.uiName = data.uiName;
        this.isTrap = data.isTrap;
    	this.hasTurret = data.hasTurret;

        this.level = 1;
    	this.t = 0;

    	base.Init();
        foreach(var weaponName in data.weapons){
            var weapon = ObjectFactory.CreateWeapon(weaponName);
            weapon.Equiped(this);
            weapons.Add(weapon);
        }
        
        foreach(var skillName in data.skills){
            var skill = ObjectFactory.CreateSkill(skillName);
            skillList.Add(skill);
            skill.OnAttach(this);
        }

        foreach(var ultimateSkillName in data.ultimateSkills){
            var utmSkill = ObjectFactory.CreateUltimateSkill(ultimateSkillName);
            utmSkills.Add(utmSkill);
            utmSkill.Equiped(this);
        }
    }

    /*能否被放置于某处*/
    /*参数列表：地图坐标*/
    public virtual bool CanPlaceOn(int x,int y){
    	return true;
    }

    /*进入场景*/
    /*参数列表：位置，所属方*/
    public override void Enter(TSVector p,TeamType team){
    	base.Enter(p,team);
        if(view!=null)view.BuildUp();
        inBuild = true;
    }

    /*逻辑帧运算*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
    	t+=dt;
    	if(t<buildUpTime)return;
        if(inBuild){
            inBuild = false;
            OnBuildDone();
        }

    	base.Step(dt);

        foreach(var chip in chips){
            chip.OnStep(dt);
        }
        foreach(var utmSkill in utmSkills){
            utmSkill.Step(dt);
        }
    }

    public virtual void OnBuildDone(){
        foreach(var chip in chips){
            chip.OnBuildDone();
        }
    }

    public override int Hurt(int damage, BaseUnit damageSource){
        var dmg = base.Hurt(damage,damageSource);

        foreach(var chip in chips){
            chip.OnHurt(dmg,damageSource);
        }

        return dmg;
    }

    public override void Die(BaseUnit damageSource){
        base.Die(damageSource);

        foreach(var chip in chips){
            chip.OnDie(damageSource);
        }

        if(view!=null)view.Destroyed();

        BattleManager.ins.noBuildLose = false;
    }

    public void LevelUp(){
        var oldHp = GetMaxHp();
        this.level++;
        this.hp = this.hp + GetMaxHp() - oldHp;

        if(view!=null)view.Upgrade();
    }

    public void Sell(){
        BattleManager.ins.SellBuild(this);

        foreach(var chip in chips){
            BattleManager.ins.CreateChip(position,chip.data.name);
        }

        isDead = true;

        this.view.Sell();
    }

    public void Rebuild(){
        hp = GetMaxHp();
        isDead = false;
        yRotation = 0;
        if(data.settingType!=1)turretRotation = 0;
        inBuild = true;

        if(view!=null)view.Rebuild();
    }

    public void AddChip(BaseChip chip){
        chip.OnEquiped(this);
    }

    public void RemoveChip(string chipID){
        for(var i=0;i<chips.Count;i++){
            if(chips[i].data.name ==  chipID){
                chips.Remove(chips[i]);
                break;
            }
        }
    }

    public void CastSkill(int index){
        if(utmSkills.Count>index){
            utmSkills[index].Cast(new TSVector());
        }
    }

    public virtual MapBuildData ToData(){
        var dat = new MapBuildData();
        dat.buildId = data.name;
        var c = new Coord(position.x,position.z);
        dat.x = c.x;
        dat.y = c.y;
        dat.isHero = isHero;
        dat.level = level;
        dat.targetPrefer = (int)targetPrefer;
        dat.extraParas = new List<int>();
        foreach(var chip in chips){
            if(chip.HasExPara())dat.extraParas.Add(chip.GetExtraPara());
        }
        return dat;
    }

    public virtual void FromData(MapBuildData dat){
        level = dat.level;
        hp = GetMaxHp();
        targetPrefer = (TargetPrefer)dat.targetPrefer;
        var n = 0;
        foreach(var chip in chips){
            if(chip.HasExPara())chip.SetExtraPara(dat.extraParas[n++]);
        }
    }

    public virtual bool IsSelectable(){
        return !isDead;
    }

    public override int GetMaxHp(){
        var percent = 0;
        var number = 0;
        foreach(var chip in chips){
            percent+=chip.OnGetStrengthIncreasePercent();
        }
        number = base.GetMaxHp()*(Configs.commonConfig.upgradeStrengthRate[level] - 100)/100;
        foreach(var chip in chips){
            number+=chip.OnGetStrengthIncreaseNumber();
        }
        return (base.GetMaxHp() + number)*(100+percent)/100;
    }
    
    public override int GetFireSpeedIncreasePercent(){
        var percent = base.GetFireSpeedIncreasePercent();
        foreach(var chip in chips){
            percent+=chip.OnGetFireSpeedIncreasePercent();
        }
        percent = (100 + percent)*Configs.commonConfig.upgradeFireSpeedRate[level]/100 - 100;
        return percent;
    }

    public override int GetDamageIncreasePercent(){
        var percent = base.GetDamageIncreasePercent();
        foreach(var chip in chips){
            percent+=chip.OnGetDamageIncreasePercent();
        }
        percent = (100 + percent)*Configs.commonConfig.upgradeDamageRate[level]/100 - 100;
        return percent;
    }
    
    public override int GetRangeIncreasePercent(){
        var percent = base.GetRangeIncreasePercent();
        foreach(var chip in chips){
            percent+=chip.OnGetRangeIncreasePercent();
        }
        percent = (100 + percent)*Configs.commonConfig.upgradeRangeRate[level]/100 - 100;
        return percent;
    }

    public override int GetHurtMultipleFactor(int damage, BaseUnit damageSource){
        var multipleFactor = base.GetHurtMultipleFactor(damage,damageSource);

        foreach(var chip in chips){
            multipleFactor+=chip.OnGetHurtMultipleFactor(damage,damageSource);
        }
        return multipleFactor;
    }

    public override int GetBulletRadiusIncreasePercent(){
        var percent = base.GetBulletRadiusIncreasePercent();
        foreach(var chip in chips){
            percent+=chip.OnGetBulletRadiusIncreasePercent();
        }
        return percent;
    }

    public override int GetBurstCount(){
        var count = base.GetBurstCount();
        foreach(var chip in chips){
            count+=chip.OnGetBurstCount();
        }
        return count;
    }

    public override int GetBlossomCount(){
        var count = base.GetBlossomCount();
        foreach(var chip in chips){
            count+=chip.OnGetBlossomCount();
        }
        return count;
    }

    public override int GetPierceCount(){
        var count = base.GetPierceCount();
        foreach(var chip in chips){
            count+=chip.OnGetPierceCount();
        }
        return count;
    }

    public override int GetChainCount(){
        var count = base.GetChainCount();
        foreach(var chip in chips){
            count+=chip.OnGetChainCount();
        }
        return count;
    }

    public override void OnHitTarget(BaseUnit target){
        base.OnHitTarget(target);
        foreach(var chip in chips){
            chip.OnHitTarget(target);
        }
    }

    public override void OnDealDamage(int damage, BaseUnit target){
        base.OnDealDamage(damage,target);
        foreach(var chip in chips){
            chip.OnDealDamage(damage,target);
        }
    }

    public override void OnShotBullet(BaseBullet bullet){
        base.OnShotBullet(bullet);
        foreach(var chip in chips){
            chip.OnShotBullet(bullet);
        }
    }

    public override void OnKill(BaseUnit target){
        base.OnKill(target);
        foreach(var chip in chips){
            chip.OnKill(target);
        }
    }

    public override void OnDayStart(){
        base.OnDayStart();
        foreach(var chip in chips){
            chip.OnDayStart();
        }
    }

    public override void OnNightEnd(){
        base.OnNightEnd();
        foreach(var chip in chips){
            chip.OnNightEnd();
        }
    }
}
