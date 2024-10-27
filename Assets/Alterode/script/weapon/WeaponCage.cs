using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class WeaponCage : BaseWeapon
{
	bool isPreparing = false;

    public override void Step(FP dt){
    	unit.fireCd-=dt;
    	if(unit.fireCd<0){
    		if(isPreparing){
    			if(((BaseBuild)unit).view!=null)((BuildViewCage)(unit.view)).Reset();
    			isPreparing = false;
    		}
    		var target = SearchTarget();
    		if(target!=null){
                Fire(target);
                unit.fireCd = 1/GetFireSpeed();
            	if(((BaseBuild)unit).view!=null)((BaseBuild)unit).view.Fire();
    		}
    	}
    	else{
    		isPreparing = true;
    	}
    }

    public override BaseUnit SearchTarget(){
        var r = GetRange();
        List<BaseUnit> targetList = new List<BaseUnit>();

    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=unit.team && !u.isDead && !u.IsImmune()){
                if(u.type != UnitType.Monster || ( ( ((BaseMonster)u).moveType == MoveType.Fly && aimType == AimType.Air ) || ( ((BaseMonster)u).moveType != MoveType.Fly && aimType == AimType.Ground ) || aimType == AimType.All)){
                    if(u.hp <= GetDamage()){
	                    var d = (u.position - unit.position);
	                    d.y = 0;
	                    var disSQ = d.sqrMagnitude;
	                    if(disSQ<r*r)return u;
                    }
                }
    		}
    	}

    	return null;
    }

    public override void Fire(BaseUnit target){
    	target.isDead = true;
    	((MonsterView)(target.view)).RemoveDelay(2);
    	((MonsterView)(target.view)).isCaged = true;

        var coin = unit.GetMaxHp();

        var plusNum = 0;
        var factor = 100;
        foreach(var chip in ((BaseBuild)unit).chips){
            factor+=chip.OnGetMoneyIncreaseFactor();
            plusNum+=chip.OnGetMoneyIncreaseNum();
        }
        
        coin = (coin+plusNum)*factor/100;

		BattleManager.ins.engine.AddCoin(coin);
		BattleManager.ins.PlayCoinEffect(coin,Utils.TSVecToVec3(unit.position));
    }
}
