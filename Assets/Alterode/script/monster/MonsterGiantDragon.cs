using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterGiantDragon : MonsterWalk
{
	public TSVector targetPos;

	public FP flyHeight;

	public int hpRate = 70;
	public int hpRate2 = 20;

	bool isCrashed = false;
	bool isCrashing = false;
	bool isRefly = false;
	FP crashTime = 0;

    public override void Init(MonsterData data){
    	base.Init(data);

    	hpRate = data.paras[0];
    	hpRate2 = data.paras[1];
    }

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
    	if(!isCrashed && hp*100/GetMaxHp()<hpRate){
    		var terrain = BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByCoord(new Coord(position.x,position.z))];
    		if(terrain == TerrainType.Ground){
	    		Crash();
	    		return;
    		}
    	}

    	if(isCrashed && !isRefly &&  hp*100/GetMaxHp()<hpRate2){
    		isRefly = true;
			moveType = MoveType.Fly;
			((MonsterViewGiantDragon)view).ReFly();
    	}

		if(!isCrashed || isRefly){
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
				position.y+=dt*GetSpeed()*2;
				return;
			}
		}

		if(isCrashing){
			crashTime+=dt;
			if(position.y>0)position.y-=dt*GetSpeed()*5;
			if(crashTime>1){
				CrashDone();
			}
			return;
		}

    	base.Step(dt);
    }

    void Crash(){
    	isCrashed = true;
    	isCrashing = true;
		fireUpTime = 3;
		if(view!=null){
			((MonsterViewGiantDragon)view).Hurt();
		}
    }

    void CrashDone(){
    	isCrashing = false;
		if(view!=null){
			((MonsterViewGiantDragon)view).Crash();
		}
		fireUpTime = 3;
		moveType = MoveType.Walk;
    }

	public override void Move(FP dt){
		if(!isCrashed || isRefly){
			Fly(dt);
		}
		else{
			base.Move(dt);
		}
	}


	void Fly(FP dt){
		if(targetPos!=null){
			var dir = targetPos - position;
	        yRotation = Utils.VectorToAngle(dir);
			dir.y = 0;
			dir = dir.normalized;
	    	position = position+dir*dt*GetSpeed();
		}
	}

}
