using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletRock : BulletStraight
{
    public override void Step(FP dt){
    	var c0 = new Coord(position.x,position.z);
    	base.Step(dt);

    	var c = new Coord(position.x,position.z);
    	if(!BattleMap.ins.IsTileCanWalk(c.x,c.y)){
    		if(c0.x!=c.x)direction.x = -direction.x;
    		if(c0.y!=c.y)direction.z = -direction.z;

        	if(view!=null)view.Hit(null);
    	}
    }

    public override void Miss(){
    	base.Miss();
    	if(view!=null)view.Explodes();
    }
}
