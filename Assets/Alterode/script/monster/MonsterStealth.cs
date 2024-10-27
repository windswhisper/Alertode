using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterStealth : MonsterWalk
{
    bool isAppearing = false;
    FP appearTime = 0;

    public override void Enter(TSVector p,TeamType team){
    	base.Enter(p,team);

    	position.y = -2;
        isAppearing = false;
        appearTime = 0;
    }

	public override void Move(FP dt){
    	var y = position.y;
    	position.y = 0;
    	
    	base.Move(dt);

        if(isAppearing && appearTime<1){
            appearTime += dt;
            return;
        }

        var map = BattleMap.ins;

    	if(nextStep==null && y<0){
            Jump();
    	}
        else if( nextStep!=null && map.pathMapWalk[map.IndexByCoord(nextStep.x,nextStep.y)]<5){
            Jump();
        }
    	else{
    		position.y = y;
    	}
    }

    public override int Hurt(int damage, BaseUnit damageSource){
        var dmg = base.Hurt(damage,damageSource);

        if(dmg!=0)Jump();

        return dmg;
    }

    public void Jump(){
        if(isAppearing){
            position.y = 0;
        }
        else{
            ((MonsterViewStealth)view).Jump();
            fireCd = 1;
            isAppearing = true;
        }
    }
}
