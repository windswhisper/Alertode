using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//子弹类型
public enum BulletType {
	//无弹道
	Inviso,
	//直线弹道
	Straight,
	//抛物线弹道
	Arcing,
	//追踪导弹
	Missile,
	//喷射弹道（瞬发直线）
	Jet,
}

//基础子弹类
public class BaseBullet
{
    public BulletData data;
	//名称
	public string name = "Bullet0";
	//子弹类型
	public BulletType type = BulletType.Inviso;
	//子弹半径
	public FP radius = 0;
	//子弹寿命
	public FP life = 1;
	//子弹飞行速度
	public FP speed = 4;
    //爆炸半径
    public FP explodeRadius = 0;
    //命中动画
    public string hitFX = "";
    //爆炸动画
    public string explodeFX = "";
    //是否影响敌军
    public bool affectsEnemies = true;
    //是否影响友军
    public bool affectsAllies = false;

    //视图层对象
    public BulletView view;
	//伤害值
	public int damage = 0;
    //射击者
    public BaseUnit unit;
    //射击武器
    public BaseWeapon weapon;
    //目标类型
    public AimType aimType;
    //射击目标
    public BaseUnit target;
    //射击目标位置
    public TSVector targetPos;
    //所属方
    public TeamType team = TeamType.Self;
    //当前坐标
    public TSVector position = new TSVector(0,0,0);
 	//已流逝时间
 	public FP t = 0;
 	//是否已消失
 	public bool isMissed = false;
    //放大比率
    public int largePercent = 0;
    //Buff列表
    public List<BaseBulletBuff> buffs;
    //忽略目标列表
    public List<BaseUnit> ignoreList;
    //不会命中目标
    public bool isFake = false;

    /*初始化*/
    /*参数列表：配置数据*/
    public virtual void Init(BulletData data){
        this.data = data;
    	this.name = data.name;
    	this.type = (BulletType)data.type;
    	this.radius = new FP(data.radius)/100;
    	this.life = new FP(data.life)/100;
    	this.speed = new FP(data.speed)/100;
    	this.explodeRadius = new FP(data.explodeRadius)/100;
    	this.hitFX = data.hitFX;
    	this.explodeFX = data.explodeFX;
    	this.affectsEnemies = data.affectsEnemies;
    	this.affectsAllies = data.affectsAllies;

        buffs = new List<BaseBulletBuff>();
        ignoreList = new List<BaseUnit>();
    }

    /*进入场景*/
    /*参数列表：位置，射击者，目标*/
    public virtual void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
    	this.position = p;
    	this.unit = unit;
    	this.team = unit.team;
    	this.target = target;
        this.targetPos = targetPos;
        this.weapon = weapon;
        if(weapon==null)this.aimType = AimType.Ground;
        else this.aimType = weapon.aimType;
    	t = 0;
    	isMissed = false;

        largePercent = largePercent + unit.GetBulletRadiusIncreasePercent();
        radius = radius*(100 + largePercent)/100;
        explodeRadius = explodeRadius*(100 + largePercent)/100;

        buffs.Clear();
        ignoreList.Clear();

        if(view!=null)view.Enter();
    }

    public void AddBuff(BaseBulletBuff buff){
        this.buffs.Add(buff);
        buff.Attach(this);
    }

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public virtual void Step(FP dt){
    	t+=dt;

        foreach(var buff in buffs){
            buff.OnStep(dt);
        }

    	if(t>life)Miss();
    }

    /*碰撞检测*/
    public virtual void DetectCollision(){
        foreach(var buff in buffs){
            if(buff.IsDisCollide()){
                return;
            }
        }

        foreach(var target in BattleManager.ins.engine.unitList){
            if(!target.isDead && !target.IsImmune() && !ignoreList.Contains(target)){
                if((target.team!=unit.team  && affectsEnemies)||(target.team==unit.team  && affectsAllies)){
                    if(TSVector.DistanceSQ(position,target.position + new TSVector(0,target.radius,0)) <= (radius+target.radius)*(radius+target.radius) ){
                        if(explodeRadius == 0)Hit(target);
                        Explodes();
                        return;
                    }
                }
            }
        }
    }

    /*命中目标*/
    /*参数列表：目标单位*/
    public virtual void Hit(BaseUnit target){
        if(isFake)return;
        if(target==null||target.isDead)return;
        var factor = 100;
        foreach(var buff in buffs){
            factor += buff.OnGetHitDamageIncrease(target);
        }
        var dmg = damage*factor/100;
        foreach(var buff in buffs){
            buff.OnHit(target);
        }
    	target.Hurt(dmg,unit);

        if(unit!=null)unit.OnHitTarget(target);
        
        if(view!=null)view.Hit(target);
    }

    /*爆炸*/
    public virtual void Explodes(){
        if(explodeRadius!=0){
            foreach(var target in BattleManager.ins.engine.unitList){
                if(!target.isDead && !target.IsImmune()){
                    if((target.team!=unit.team  && affectsEnemies)||(target.team==unit.team  && affectsAllies)){
                        var p=target.position+new TSVector(0,target.radius,0);
                        if(aimType == AimType.UnderGround && target.IsUnderGround())p.y=position.y;
                        if(TSVector.DistanceSQ(position,p) <= (explodeRadius+target.radius)*(explodeRadius+target.radius) ){
                            Hit(target);
                        }
                    }
                }
            }
        }

        foreach(var buff in buffs){
            buff.OnExplode();
        }

    	if(view!=null)view.Explodes();
    	Miss();
    }

    public virtual FP PredictDelay(TSVector selfPos,TSVector targetPos)
    {
        return 0;
    }

    /*消失*/
    public virtual void Miss(){
    	isMissed = true;
    }
}
