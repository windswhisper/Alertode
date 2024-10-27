using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseHero : BaseBuild
{
	public int exp  = 0;

	public bool isMoving = false;
	public Coord moveGoalPos;

	public FP speed = 3;

	List<int> pathMap = new List<int>();
	List<Coord> searchingPoints = new List<Coord>();
	Coord coord;
	Coord nextStep = null;
	bool throughBuild = false;
	FP selfHealTime = 0;

	public override void Init(BuildData data){
		base.Init(data);
		isHero = true;
	}
    public override void Enter(TSVector p,TeamType team){
		base.Enter(p,team);
		Debug.Log("123123");
		AddChip(ObjectFactory.CreateChip(((HeroData)data).defaultChips[0]));

		if(view!=null)((HeroView)view).PlayEnterVoice();
	}

	public bool MoveTo(Coord c){
		if(!BattleMap.ins.IsTileCanWalk(c.x,c.y) || BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(c)]!=0 || BattleMap.ins.fogMap[BattleMap.ins.IndexByCoord(c)]!=0)return false;

        if(IsStunned()){
            return false;
        }

		isMoving = true;
		moveGoalPos = c;
		coord = null;
		FindPath();
		if(isMoving){
			foreach(var weapon in weapons){
				weapon.Stop();
			}

        	BattleMap.ins.tempBuildMap[BattleMap.ins.IndexByCoord(c)] = 1;

			if(view!=null)((HeroView)view).PlayMoveVoice();
		}
        return isMoving;
	}

    public override void Step(FP dt){
    	base.Step(dt);

    	selfHealTime+=dt;
    	if(selfHealTime>=1){
    		selfHealTime = 0;
	    	var maxHp = GetMaxHp();
	    	if(hp<maxHp){
	    		hp+=maxHp/100;
	    		if(hp>maxHp)hp = maxHp;
	    	}
    	}
    	
        if(IsStunned()){
            return;
        }

    	if(isMoving){
    		Move(dt);
    	}
    }

    public void FindPath(){
    	searchingPoints.Clear();
    	searchingPoints.Add(moveGoalPos);

    	pathMap.Clear();
    	for(var y=0;y<BattleMap.ins.mapData.height;y++){
    		for(var x=0;x<BattleMap.ins.mapData.width;x++){
				pathMap.Add(999);
			}
		}

		pathMap[BattleMap.ins.IndexByCoord(moveGoalPos)] = 0;
    	SearchingPathWalk(0);

    	if(pathMap[BattleMap.ins.IndexByCoord(new Coord(position.x,position.z))]==999){
    		isMoving = false;
    	}
    	else{
    		isMoving = true;
    	}
    }

    void SearchingPathWalk(int step){
    	List<Coord> newPoints = new List<Coord>();
    	step++;
    	foreach(var p in searchingPoints){
    		var i = BattleMap.ins.IndexByCoord(p);
    		if(p.x>0){
    			var iEast = BattleMap.ins.IndexByCoord(p.x-1,p.y);
    			if(BattleMap.ins.staticTerrainSP[iEast]==TerrainType.Ground){
    				if(pathMap[iEast] > step){
	    				newPoints.Add(new Coord(p.x - 1,p.y));
	                    pathMap[iEast] = step;
    				}
    			}
    		}
    		if(p.y>0){
    			var iSouth = BattleMap.ins.IndexByCoord(p.x,p.y-1);
    			if(BattleMap.ins.staticTerrainSP[iSouth]==TerrainType.Ground){
    				if(pathMap[iSouth] > step){
	    				newPoints.Add(new Coord(p.x,p.y - 1));
	                    pathMap[iSouth] = step;
    				}
    			}
    		}

    		if(p.x<BattleMap.ins.mapData.width-1){
    			var iWest = BattleMap.ins.IndexByCoord(p.x+1,p.y);
    			if(BattleMap.ins.staticTerrainSP[iWest]==TerrainType.Ground){
    				if(pathMap[iWest] > step){
	    				newPoints.Add(new Coord(p.x + 1,p.y));
	                    pathMap[iWest] = step;
    				}
    			}
    		}
    		if(p.y<BattleMap.ins.mapData.height-1){
    			var iNorth = BattleMap.ins.IndexByCoord(p.x,p.y+1);
    			if(BattleMap.ins.staticTerrainSP[iNorth]==TerrainType.Ground){
    				if(pathMap[iNorth] > step){
	    				newPoints.Add(new Coord(p.x,p.y + 1));
	                    pathMap[iNorth] = step;
    				}
    			}
    		}
    	}

    	if(newPoints.Count == 0)return;

    	searchingPoints.Clear();
    	foreach(var coord in newPoints){
    		searchingPoints.Add(coord);
    	}
    	SearchingPathWalk(step);
    }

    public void Move(FP dt){
		var p = position;
		p.y = 0;
		var map = BattleMap.ins;
		if(coord == null)coord = new Coord(p.x,p.z);
		if(nextStep == null || TSVector.Distance(new TSVector(nextStep.x,0,nextStep.y),p) < dt*speed*2) {
			if(nextStep!=null){
        		if(!throughBuild)BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(coord)] = 0;
        		throughBuild = false;
				coord = nextStep;
				if(BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(nextStep)]==1)throughBuild = true;
        		BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(nextStep)] = 1;
        		
        		position = new TSVector(nextStep.x,0,nextStep.y);
			}
			if(Utils.IsCoordInMap(coord)){
				var nextList = new List<Coord>();
				if(findNextStepDiagonal(coord.x,coord.y,-1,-1)){
					nextList.Add(new Coord(-1,-1));
				}
				if(findNextStepDiagonal(coord.x,coord.y,1,-1)){
					nextList.Add(new Coord(1,-1));
				}
				if(findNextStepDiagonal(coord.x,coord.y,-1,1)){
					nextList.Add(new Coord(-1,1));
				}
				if(findNextStepDiagonal(coord.x,coord.y,1,1)){
					nextList.Add(new Coord(1,1));
				}

				if(nextList.Count==0){
					if(findNextStep(coord.x,coord.y,-1,0)){
						nextList.Add(new Coord(-1,0));
					}
					if(findNextStep(coord.x,coord.y,1,0)){
						nextList.Add(new Coord(1,0));
					}
					if(findNextStep(coord.x,coord.y,0,-1)){
						nextList.Add(new Coord(0,-1));
					}
					if(findNextStep(coord.x,coord.y,0,1)){
						nextList.Add(new Coord(0,1));
					}
				}

				if(nextList.Count>0){
					nextStep = new Coord(coord.x+nextList[0].x,coord.y+nextList[0].y);
				}
				else{
					nextStep = null;
				}
			}
		}
		
		if(nextStep!=null){
			var targetPos = new TSVector(nextStep.x,0,nextStep.y);
			var dir = (targetPos - position).normalized;
	        yRotation = Utils.VectorToAngle(dir);
	    	position = position+dir*dt*GetSpeed();
		}
		else{
			isMoving = false;
        	BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(position.x,position.z))] = 1;
        	BattleMap.ins.tempBuildMap[BattleMap.ins.IndexByCoord(moveGoalPos)] = 0;
		}
    }

	bool findNextStep(int x,int y,int dx,int dy){
		var map = BattleMap.ins;
		if(Utils.IsCoordInMap(x+dx,y+dy)){
			var s = pathMap[map.IndexByCoord(x,y)];
			var s2 = pathMap[map.IndexByCoord(x+dx,y+dy)];
			if(s2>=0 && s2 == s-1){
				return true;
			}
		}
		return false;
	}

	bool findNextStepDiagonal(int x,int y,int dx,int dy){
		var map = BattleMap.ins;
		if(Utils.IsCoordInMap(x+dx,y+dy)){
			var s = pathMap[map.IndexByCoord(x,y)];
			var s2 = pathMap[map.IndexByCoord(x+dx,y)];
			var s3 = pathMap[map.IndexByCoord(x,y+dy)];
			var s4 = pathMap[map.IndexByCoord(x+dx,y+dy)];

			if(((s2>=0 && s2 == s-1) || (s3>=0 && s3 == s-1)) && s4>=0 && s4 == s-2 ){
				return true;
			}
		}
		return false;
	}

    public override void FromData(MapBuildData dat){
    	base.FromData(dat);

		if(level >= Configs.commonConfig.buildEvoLevel[1]){
			AddChip(ObjectFactory.CreateChip(((HeroData)data).defaultChips[1]));
		}
		else if(level >= Configs.commonConfig.buildEvoLevel[2]){
			AddChip(ObjectFactory.CreateChip(((HeroData)data).defaultChips[2]));
		}

		int n=0;
        foreach(var chip in chips){
            if(chip.HasExPara())chip.SetExtraPara(dat.extraParas[n++]);
        }
    }

    public virtual FP GetSpeed(){
    	var increase = GetMoveSpeedIncreasePercent();
    	return speed*(100+increase)/100;
    }

    public override bool IsWeaponDisable(){
        return isMoving;
    }

	public override void OnKill(BaseUnit target){
		base.OnKill(target);
		
		GetExp(Configs.commonConfig.monsterExp);
	}

	public void GetExp(int e){
		if(level<Configs.commonConfig.heroMaxLevel){
			exp+=e;

			if(exp>=Configs.commonConfig.upgradeExp[level]*data.cost/100){
				exp-=Configs.commonConfig.upgradeExp[level]*data.cost/100;
				LevelUp();

				//if(BattleManager.ins.gameMode == 0){
					if(level == Configs.commonConfig.buildEvoLevel[1]){
						AddChip(ObjectFactory.CreateChip(((HeroData)data).defaultChips[1]));
					}
					else if(level == Configs.commonConfig.buildEvoLevel[2]){
						AddChip(ObjectFactory.CreateChip(((HeroData)data).defaultChips[2]));
					}
				//}
			}
		}
		else{
			exp = 0;
		}
	}
	
    public override bool IsSelectable(){
        return base.IsSelectable() && !isMoving;
    }

    public override void OnNightEnd(){
		GetExp(Configs.commonConfig.waveExp);
    }

    public override void Die(BaseUnit damageSource){
    	base.Die(damageSource);

		if(view!=null)((HeroView)view).PlayDieVoice();
    }
}
