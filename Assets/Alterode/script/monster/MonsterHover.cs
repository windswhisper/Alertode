using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterHover : BaseMonster
{
	Coord coord;
	Coord nextStep = null;
	protected TSVector targetPos;
	protected bool haveTarget = false;

	/*移动*/
	public override void Move(FP dt){
		Walk(dt);
	}

	void Walk(FP dt){
		var p = position;
		p.y = 0;
		var map = BattleMap.ins;
		if(coord == null)coord = new Coord(p.x,p.z);
		if (nextStep == null || (haveTarget && TSVector.Distance(targetPos, p) < dt * speed * 2))
		{
			haveTarget = false;
			if (nextStep!=null){
				coord = nextStep;
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

				if(nextList.Count>0){
					var densityMap = BattleMap.ins.densityMapWalk;
					var density = densityMap[map.IndexByCoord(nextList[0].x+coord.x,nextList[0].y+coord.y)];
					nextStep = new Coord(coord.x+nextList[0].x,coord.y+nextList[0].y);

					foreach(var dir in nextList){
						if(densityMap[map.IndexByCoord(dir.x+coord.x,dir.y+coord.y)] < density){
							density = densityMap[map.IndexByCoord(dir.x+coord.x,dir.y+coord.y)];
							nextStep = new Coord(coord.x+dir.x,coord.y+dir.y);
						}
					}

	    			BattleMap.ins.densityMapWalk[map.IndexByCoord(nextStep.x,nextStep.y)]++;
				}
				else{
					nextStep = null;
				}
			}
		}

		if (!haveTarget && nextStep != null)
		{
			haveTarget = true;
			var deviation = Configs.commonConfig.monsterMoveDeviation;
			targetPos = new TSVector(nextStep.x, 0, nextStep.y) + new TSVector(TSRandom.Range(-deviation, deviation) / 100, 0, TSRandom.Range(-deviation, deviation) / 100);
		}

		if (haveTarget)
		{
			var dir = (targetPos - position).normalized;
			yRotation = Utils.VectorToAngle(dir);
			position = position + dir * dt * GetSpeed();
		}
	}

	bool findNextStep(int x,int y,int dx,int dy){
		var map = BattleMap.ins;
		if(Utils.IsCoordInMap(x+dx,y+dy)){
			var s = map.pathMapHover[map.IndexByCoord(x,y)];
			var s2 = map.pathMapHover[map.IndexByCoord(x+dx,y+dy)];
			if(s2>=0 && s2 == s-1 && map.buildMap[map.IndexByCoord(x+dx,y+dy)] != 1 && map.crowdedMapGround[map.IndexByCoord(x + dx, y + dy)] < Configs.commonConfig.crowdedMaxInCoord)
			{
				return true;
			}
		}
		return false;
	}

	bool findNextStepDiagonal(int x,int y,int dx,int dy){
		var map = BattleMap.ins;
		if(Utils.IsCoordInMap(x+dx,y+dy)){
			var s = map.pathMapHover[map.IndexByCoord(x,y)];
			var s2 = map.pathMapHover[map.IndexByCoord(x+dx,y)];
			var s3 = map.pathMapHover[map.IndexByCoord(x,y+dy)];
			var s4 = map.pathMapHover[map.IndexByCoord(x+dx,y+dy)];

			if(((s2>=0 && s2 == s-1) || (s3>=0 && s3 == s-1)) && s4>=0 && s4 == s-2 && map.buildMap[map.IndexByCoord(x+dx,y)] != 1 && map.buildMap[map.IndexByCoord(x,y+dy)] != 1 && map.buildMap[map.IndexByCoord(x+dx,y+dy)] != 1 && map.crowdedMapGround[map.IndexByCoord(x + dx, y + dy)] < Configs.commonConfig.crowdedMaxInCoord)
			{
				return true;
			}
		}
		return false;
	}
}
