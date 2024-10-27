using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class WeaponPrismSupport : BaseWeapon
{
    public bool isReady = false;
    public bool isCharged = false;

    public override void Step(FP dt){
        if(unit.IsWeaponDisable())return;

        if(unit.fireCd<0){
            if(unit.fireUpTime == 0){
                isReady = true;
            }
            if(isFireUp){
                if(unit.fireUpTime > unit.GetFireUp()-0.2){
                    isCharged = true;
                    if(unit.fireUpTime > unit.GetFireUp()+0.1){
                        isCharged = false;
                        isFireUp = false;
                        unit.fireUpTime = 0;
                        unit.fireCd = 0;
                    }
                }
            }
        }
    }

    public override void FireUp(BaseUnit target){
        base.FireUp(target);
        isFireUp = true;
        isReady = false;
    }


    public override void Fire(BaseUnit target){
        isCharged = false;
        isFireUp = false;
        unit.fireUpTime = 0;
        unit.fireCd = 1/GetFireSpeed();

        unit.turretRotation = Utils.VectorToAngle(target.position - unit.position);
        var muzzleOffset = Utils.VectorRotate(muzzles[muzzleIndex],-unit.turretRotation);

        var targetPos = target.position;
        targetPos.y=(unit.position+muzzleOffset).y;
        var bullet = BattleManager.ins.CreateBullet(bulletName);
        var dir = targetPos - unit.position;
        bullet.Enter(unit.position+muzzleOffset,unit,this,target,targetPos);
        bullet.damage = 0;
        bullet.isFake = true;

        if(unit.view!=null)unit.view.PlayMuzzleAnim(muzzleFX,Utils.TSVecToVec3(muzzles[muzzleIndex]),Utils.TSVecToVec3(muzzleOffset),(float)unit.turretRotation*180/Mathf.PI);

        muzzleIndex = (muzzleIndex+1)%muzzles.Count;
    }
}
