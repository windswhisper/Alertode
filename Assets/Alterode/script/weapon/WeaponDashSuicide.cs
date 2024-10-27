using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class WeaponDashSuicide : BaseWeapon
{
	public bool isDashing = false;
	BaseUnit dashTarget;
	FP dashSpeed = 5;

    public override void Step(FP dt){
    	if(isDashing){
			var dir = dashTarget.position - unit.position;
			if(dir.sqrMagnitude<dashTarget.radius*dashTarget.radius){
            	var bullet = BattleManager.ins.CreateBullet(bulletName);
	            bullet.Enter(unit.position,unit,this,dashTarget,unit.position);
	            bullet.damage = GetDamage();
	            unit.OnShotBullet(bullet);
	            unit.Die(null);
			}
			else{
				dir = dir.normalized;
		    	unit.position = unit.position+dir*dt*dashSpeed;
			}
    		return;
    	}
    	base.Step(dt);
    }

    public override BaseUnit SearchTarget(){
        var r = GetRange();
        BaseUnit target = null;
    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=unit.team && !u.isDead && !u.IsImmune() && !u.IsInviso() && !unit.IsUnderGround()){
                if(u.type == UnitType.Building && ((BaseBuild)u).name=="Base"){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<(r+u.radius)*(r+u.radius))target = u;
                }
    		}
    	}
    	return target;
    }

    public override void Fire(BaseUnit target){
    	isDashing = true;
    	dashTarget= target;
    	if(((BaseMonster)unit).moveType==MoveType.Fly){
    		((MonsterFly)unit).flyHeight = 0;
    	}
    }
}
