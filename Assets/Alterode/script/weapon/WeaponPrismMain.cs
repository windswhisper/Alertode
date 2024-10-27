using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class WeaponPrismMain : BaseWeapon
{
    int supportDamage = 0;

    public override void FireUp(BaseUnit target){
        var r = GetRange();
        List<BaseUnit> targetList = new List<BaseUnit>();
        var count = 0;

        foreach(var u in BattleManager.ins.engine.unitList){
            if(u.team==unit.team && u!=unit && !u.isDead && !u.isTrap){
                if(u.type != UnitType.Monster && u.weapons.Count>0 &&  u.weapons[0].name == "PrismSupportWeapon" && ((WeaponPrismSupport)u.weapons[0]).isReady){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<r*r){
                        u.weapons[0].FireUp(unit);
                        count++;
                        if(count>=3)break;
                    }
                }
            }
        }

        base.FireUp(target);
    }


    public override int GetDamage(){
        return base.GetDamage()+supportDamage;
    }

    public override void Fire(BaseUnit target){
        var r = GetRange();
        List<BaseUnit> targetList = new List<BaseUnit>();
        var count = 0;

        foreach(var u in BattleManager.ins.engine.unitList){
            if(u.team==unit.team && u!=unit && !u.isDead && !u.isTrap){
                if(u.type != UnitType.Monster && u.weapons.Count>0 &&  u.weapons[0].name == "PrismSupportWeapon" && ((WeaponPrismSupport)u.weapons[0]).isCharged && u.fireCd < 0){
                    var d = (u.position - unit.position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<r*r){
                        u.weapons[0].Fire(unit);
                        supportDamage+=u.weapons[0].GetDamage();
                        count++;
                        if(count>=3)break;
                    }
                }
            }
        }

        base.Fire(target);

        supportDamage = 0;
    }
}
