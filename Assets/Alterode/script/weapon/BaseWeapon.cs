using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//可攻击目标
public enum AimType{
	//仅地面
	Ground,
	//仅空军
	Air,
	//地面和空军
	All,
    //地表和地下
    UnderGround,
    //水下
    UnderWater,
}

public class BaseWeapon
{
	//名称
	public string name = "Weapon0";
	//伤害
	public int damage = 1;
	//射击速度，单位为次每秒
	public FP fireSpeed = 1;
	//射程
	public FP range = 2;
    //最小射程
    public FP minRange = 0;
    //落点偏差
    public FP scatterRadius = 0;
	//开火位置
	public List<TSVector> muzzles = new List<TSVector>();
    //开火倾角
    public FP fireAngle = 0;
    //朝脚下开火
    public bool areaFire = false;
	//开火动画
	public string muzzleFX = "";
	//目标类型
	public AimType aimType = AimType.Ground;
	//子弹类名
	public string bulletName;
    //连发次数
    public int burst = 1;

	//武器主人
	public BaseUnit unit;
	//开火坐标序号
	public int muzzleIndex = 0;
    //子弹类型
    public BulletType bulletType;
    //连发剩余次数
    public int burstCount = 0;
    //连发冷却计时
    public FP burstTime = 1;
    //连发间隔
    public FP burstDelay = 0.15;

    public bool isFireUp = false;

    /*初始化*/
    /*参数列表：武器数据配置*/
    public virtual void Init(WeaponData data){
    	this.name = data.name;
    	this.damage = data.damage;
    	this.fireSpeed = new FP(data.fireSpeed)/100;
    	this.range = new FP(data.range)/100;
    	this.minRange = new FP(data.minRange)/100;
        this.scatterRadius = new FP(data.scatterRadius)/100;
    	foreach(var v in data.muzzles){
    		this.muzzles.Add(new TSVector(new FP(v.x)/100,new FP(v.y)/100,new FP(v.z)/100));
    	}
        this.fireAngle = data.fireAngle*TSMath.Pi/180;
        this.areaFire = data.areaFire;
        this.muzzleFX = data.muzzleFX;
    	this.aimType = (AimType)data.aimType;
    	this.bulletName = data.bulletName;
        this.bulletType = (BulletType)Configs.GetBullet(bulletName).type;
    }

    /*装配*/
    /*参数列表：武器主人*/
    public virtual void Equiped(BaseUnit unit){
    	this.unit = unit;
    }

    /*逻辑帧更新*/
    /*参数列表：时间间隔*/
    public virtual void Step(FP dt){
        if(unit.IsWeaponDisable())return;
    	if(unit.fireCd<0){
    		var target = SearchTarget();
    		if(target!=null){
                if(unit.fireUpTime == 0){
                    FireUp(target);
                }
                if(unit.fireUpTime > unit.GetFireUp() && isFireUp){
                    Fire(target);
                    unit.fireUpTime = 0;
                    unit.fireCd = 1/GetFireSpeed();
                    isFireUp = false;

                    if(GetBurstCount() > 1){
                        burstCount = GetBurstCount()-1;
                        burstTime = 0;
                    }
                }
    		}
            else{
                isFireUp = false;
            }
    	}

        if(burstCount > 0){
            burstTime+=dt;
            if(burstTime>burstDelay){
                var target = SearchTarget();
                if(target!=null){
                    Fire(target);
                    burstCount--;
                    burstTime = 0;
                }
                else{
                    burstCount = 0;
                    burstTime = 0;
                }
            }
        }
    }

    /*搜索目标*/
    public virtual BaseUnit SearchTarget(){
        var r = GetRange();
        List<BaseUnit> targetList = new List<BaseUnit>();

    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=unit.team && !u.isDead && !u.IsImmune() && !u.IsInviso() && (!unit.IsUnderGround() || (BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByPosition(u.position)] == TerrainType.Water && BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByPosition(unit.position)] == TerrainType.Water) || (unit.type == UnitType.Building && ((BaseBuild)unit).data.isNaval))){
                if(u.type == UnitType.Monster && ( ( ((BaseMonster)u).moveType == MoveType.Fly && aimType == AimType.Air ) || ( ((BaseMonster)u).moveType != MoveType.Fly && aimType == AimType.Ground && !u.IsUnderGround()) || (aimType == AimType.All && !u.IsUnderGround()) || (aimType == AimType.UnderGround && ((BaseMonster)u).moveType == MoveType.Walk || (((BaseMonster)u).moveType == MoveType.Swim && !u.IsUnderGround())) || (aimType == AimType.UnderWater && u.IsUnderGround() && ((BaseMonster)u).moveType == MoveType.Swim))){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<(r+u.radius)*(r+u.radius))targetList.Add(u);
                }
                else if(u.type == UnitType.Building && (u.IsOnGround() || (u.IsUnderGround() && unit.IsUnderGround()))){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<(r+u.radius)*(r+u.radius))targetList.Add(u);
                }
    		}
    	}

        if(targetList.Count==0)return null;
        if(targetList.Count==1)return targetList[0];

        BaseUnit targetUnit = targetList[0];
        switch(unit.targetPrefer){
            case TargetPrefer.Closest:
                FP minDis = 65536;
                foreach(var u in targetList){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<minDis){
                        minDis = disSQ;
                        targetUnit = u;
                    }
                }
                return targetUnit;
            case TargetPrefer.Strongest:
                int highestHp = targetList[0].hp;
                foreach(var u in targetList){
                    if(u.hp>highestHp){
                        highestHp = u.hp;
                        targetUnit = u;
                    }
                }
                return targetUnit;
            case TargetPrefer.Weakest:
                int lowestHp = targetList[0].hp;
                foreach(var u in targetList){
                    if(u.hp<lowestHp){
                        lowestHp = u.hp;
                        targetUnit = u;
                    }
                }
                return targetUnit;
            case TargetPrefer.Farest:
                FP maxDis = 0;
                foreach(var u in targetList){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ>maxDis){
                        maxDis = disSQ;
                        targetUnit = u;
                    }
                }
                return targetUnit;
        }
    	return null;
    }

    /*开火前摇抬手*/
    /*参数列表：目标单位*/
    public virtual void FireUp(BaseUnit target){
        isFireUp = true;
        unit.turretRotation = Utils.VectorToAngle(target.position - unit.position);
        if(unit.type == UnitType.Monster && !unit.hasTurret){
            if(!((BaseMonster)unit).mobileFire){
                unit.yRotation = Utils.VectorToAngle(target.position - unit.position);
                if(((BaseMonster)unit).view!=null)((BaseMonster)unit).view.PlayAttackAnim();
            }
        }
        if(unit.type == UnitType.Building){
            unit.yRotation = Utils.VectorToAngle(target.position - unit.position);
            if(((BaseBuild)unit).view!=null)((BaseBuild)unit).view.Fire();
        }
    }

    /*朝目标单位开火*/
    /*参数列表：目标单位*/
    public virtual void Fire(BaseUnit target){
        unit.turretRotation = Utils.VectorToAngle(target.position - unit.position);
        if(unit.type == UnitType.Monster && !unit.hasTurret){
            if(!((BaseMonster)unit).mobileFire){
                unit.yRotation = Utils.VectorToAngle(target.position - unit.position);
            }
        }
        var muzzleOffset = Utils.VectorRotate(muzzles[muzzleIndex],-unit.turretRotation);

        var bulletCount = unit.GetBlossomCount()+1;
        if(bulletType == BulletType.Inviso)bulletCount = 1;
        for(var i=0;i<bulletCount;i++){
            var targetPos = target.position;
            var bullet = BattleManager.ins.CreateBullet(bulletName);
            //提前量修正
            if (target.type == UnitType.Monster) targetPos = ((BaseMonster)target).ForwardPosition(bullet.PredictDelay(unit.position + muzzleOffset,targetPos));
            var dir = targetPos - unit.position;
            dir = Utils.VectorRotate(dir,(bulletCount - 1 - i * 2)*TSMath.Pi/360*Configs.commonConfig.blossomDegree);
            if(areaFire) {
                targetPos = new TSVector(unit.position.x,unit.position.y,unit.position.z);
            }
            else{
                targetPos = dir+unit.position;
            }
            if(scatterRadius!=0){
                var w = TSRandom.instance.NextFP()*2*TSMath.Pi;
                targetPos = targetPos + new TSVector(scatterRadius*TSMath.Sin(w),0,scatterRadius*TSMath.Cos(w)) * TSRandom.instance.NextFP();
            }
            if(fireAngle!=0){
                targetPos = unit.position+muzzleOffset + Utils.VectorRaise(targetPos - unit.position+muzzleOffset,fireAngle);
            }
            bullet.Enter(unit.position+muzzleOffset,unit,this,target,targetPos);
            bullet.damage = GetDamage();
            if(bullet.type == BulletType.Straight || bullet.type == BulletType.Missile)bullet.life *= GetLifeRate();
            unit.OnShotBullet(bullet);
        }
    	
    	if(unit.view!=null)unit.view.PlayMuzzleAnim(muzzleFX,Utils.TSVecToVec3(muzzles[muzzleIndex]),Utils.TSVecToVec3(muzzleOffset),(float)unit.turretRotation*180/Mathf.PI);

    	muzzleIndex = (muzzleIndex+1)%muzzles.Count;
    }

    public void Stop(){
        unit.fireUpTime = 0;
        burstCount = 0;
        burstTime = 0;
    }

    public virtual int GetDamage(){
        var d = (int)TSMath.Round(damage*(100+unit.GetDamageIncreasePercent())/100);
        if(d<1)d = 1;
        return d;
    }
    public virtual FP GetFireSpeed(){
        var s = fireSpeed*(100+unit.GetFireSpeedIncreasePercent())/100;
        if(s>20)s = 20;
        if(s*20<1)s = new FP(1)/20;
        return s;
    }
    public virtual int GetBurstCount(){
        return burst+unit.GetBurstCount();
    }
    public virtual FP GetRange(){
        var r = range*(100+unit.GetRangeIncreasePercent())/100;
        if(r<0)r = 0;
        return r;
    }
    public virtual FP GetLifeRate(){
       return new FP(1)*(100+unit.GetRangeIncreasePercent())/100;
    }

}
