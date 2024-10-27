using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//移动方式
public enum MoveType{
	//地面
	Walk,
	//空中，无视任何地形
	Fly,
	//水陆两栖，从斜坡处登陆
	Swim,
	//悬浮，可以从任意位置强行登陆
	Hover,
	//跳跃，可以跨越地面障碍
	Jump,
}

//基础怪物类
public class BaseMonster : BaseUnit
{
	//显示名称
	public string uiName = "";
	//移动速度
	public FP speed = 1;
	//武器注册名列表
	public List<string> weaponNames = new List<string>();
	//移动方式
	public MoveType moveType = MoveType.Walk;
	//移动攻击
	public bool mobileFire = false;

    //视图层对象
    public MonsterView view;
    //是否在移动
    public bool isMoving;
    //是否是boss
    public bool isBoss;

    /*初始化*/
    /*参数列表：配置数据*/
    public virtual void Init(MonsterData data){
    	this.type = UnitType.Monster;

    	this.name = data.name;
    	this.hp = this.maxHp = data.strength;
    	this.radius = new FP(data.radius)/100;
        this.fireUp = new FP(data.fireUp)/100;
    	this.uiName = data.uiName;
    	this.hasTurret = data.hasTurret;
    	this.speed = new FP(data.speed)/100;
    	this.moveType = (MoveType)data.moveType;
    	this.isMoving = false;

    	base.Init();

    	foreach(var weaponName in data.weapons){
	 		var weapon = ObjectFactory.CreateWeapon(weaponName);
	 		weapons.Add(weapon);
	 		weapon.Equiped(this);
    	}
    	
    	foreach(var skillName in data.skills){
	 		var skill = ObjectFactory.CreateSkill(skillName);
	 		skillList.Add(skill);
	 		skill.OnAttach(this);
    	}
    	
    }

    public virtual FP GetSpeed(){
    	var increase = GetMoveSpeedIncreasePercent();
    	return speed*(100+increase)/100;
    }

    /*逻辑帧运算*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
    	base.Step(dt);

        if(IsStunned()){
            return;
        }

    	if(mobileFire || (fireCd<=0 && fireUpTime == 0)){
    		isMoving = true;
    		Move(dt);
    	}
    	else{
    		isMoving = false;
    	}
    }

	/*移动*/
	public virtual void Move(FP dt){
		
	}

    public virtual TSVector ForwardPosition(FP t)
    {
        if (!isMoving) return position;

        return position + Utils.AngleToVector(yRotation) * speed * t;
    }

	public override void Die(BaseUnit damageSource){
        if(isDead)return;
        
		base.Die(damageSource);

		if(TSRandom.instance.Next()%1000<Configs.commonConfig.chipDropMPercent){
			BattleManager.ins.CreateChip(position);
		}
		if(TSRandom.instance.Next()%1000<Configs.commonConfig.monsterBountyMPercent){
			BattleManager.ins.CreateDropCoin(position,Configs.commonConfig.monsterBountyAmount);
		}

        if(view!=null)view.PlayDieAnim();

        AchievementManager.OnKillMonster();
        if(damageSource!=null && damageSource.type == UnitType.Building && ((BaseBuild)damageSource).isHero)AchievementManager.OnHeroKillMonster();
	}


    /*撤退*/
    public void Retreat(){
        isDead = true;
        if(view!=null)view.PlayRetreatAnim();
    }
}
