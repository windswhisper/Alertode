using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterFly : BaseMonster
{
	public TSVector targetPos;

	public FP flyHeight;

	public override void Enter(TSVector p,TeamType team){
		base.Enter(p,team);

		flyHeight = new FP(Configs.commonConfig.flyHeight)/100;
		var map = BattleMap.ins;
		FP dis = 65535;

		foreach(var pos in map.basePoints)
		{
			var tp = new TSVector(pos.x,0,pos.y);
			var d = TSVector.Distance(position,tp);
			if(d<dis){
				dis = d;
				targetPos = tp;
			}
		}
	}

    public override void Step(FP dt){
		if(position.y < flyHeight){

	        foreach(var buff in buffList){
	            buff.OnStep(dt);
	        }
	        for(var i=buffList.Count-1;i>=0;i--)
	        {
	            if(buffList[i].isRemove){
	                RemoveBuff(buffList[i]);
	            }
	        }
	        
			if(targetPos!=null){
				var dir = targetPos - position;
		        yRotation = Utils.VectorToAngle(dir);
		    }
			position.y+=dt*GetSpeed();
			return;
		}

    	base.Step(dt);
    }

	/*移动*/
	public override void Move(FP dt){
		Fly(dt);
	}


	void Fly(FP dt){
		if(targetPos!=null){
			var dir = targetPos - position;
	        yRotation = Utils.VectorToAngle(dir);
			dir.y = 0;

			if(TSVector.DistanceSQ(dir,TSVector.zero) < TSMath.Pow(dt*GetSpeed(),2) ){
				position = new TSVector(targetPos.x,position.y,targetPos.z);
				isMoving = false;
			}
			else{
				dir = dir.normalized;
				position = position+dir*dt*GetSpeed();
			}

			//var p = position + dir * dt * GetSpeed();

			//if (BattleMap.ins.crowdedMapFly[BattleMap.ins.IndexByCoord(new Coord(p.x,p.z))] < Configs.commonConfig.crowdedMaxInCoord)
				
		}
	}
}
