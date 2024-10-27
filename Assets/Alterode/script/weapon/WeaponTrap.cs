using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class WeaponTrap : BaseWeapon
{
	bool isPreparing = false;

    public override void Step(FP dt){
    	if(unit.fireCd<0){
    		if(isPreparing){
    			if(((BaseBuild)unit).view!=null)((BuildViewCage)(unit.view)).Reset();
    			isPreparing = false;
    		}
    	}
    	else{
    		isPreparing = true;
    	}

    	base.Step(dt);
    }
}
