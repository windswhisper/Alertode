using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterGenerator
{
	public MonsterGeneratorData data;

	public FP t;
	public FP deltaTime;
	public FP delayTime;
	public FP repeatDelta;
	public int index;
	public int repeatCount;

	public bool isDone;

	public MonsterGeneratorView view;

	public void Init(MonsterGeneratorData data){
		this.data = data;
		this.index = 0;
		this.repeatCount = 0;
		isDone = false;
		deltaTime = new FP(data.deltaTime)/100;
		delayTime = new FP(data.delayTime)/100;
		repeatDelta = new FP(data.repeatDelta)/100;
	}

	public void Step(FP dt){
		t-=dt;
		if(t < -delayTime){
			t+=deltaTime;
			GeneratorMonster();
		}
	}

	public void GeneratorMonster(){
		if(index >= data.monsters.Count){
			repeatCount++;
			if(repeatCount >= data.repeat){
				Remove();
				return;
			}
			index = 0;
		}

		if(data.monsters[index]!=""){
			var mData = Configs.GetMonster(data.monsters[index]);
			var coord = FindGenPos(mData,data.x,data.y);
			BattleManager.ins.PutMonster(data.monsters[index],coord.x,coord.y,data.isBoss);
		}

		if(index == data.monsters.Count -1){
			t+=repeatDelta;
		}

		if(index == data.monsters.Count -1 && repeatCount == data.repeat-1){
			Remove();
			return;
		}

		index++;

	}

	public Coord FindGenPos(MonsterData mData,int x,int y){
		var map = BattleMap.ins;
		
		var posList = new List<Coord>();

		for(var i=x-1;i<=x+1;i++){
			for(var j=y-1;j<=y+1;j++){
				if(!Utils.IsCoordInMap(i,j) || map.buildMap[map.IndexByCoord(i,j)] == 1 || map.staticTerrainSP[map.IndexByCoord(i,j)]!=map.staticTerrainSP[map.IndexByCoord(x,y)])continue;
				if(mData.moveType == 0 && map.IsTileCanWalk(i,j))posList.Add(new Coord(i,j));
				if(mData.moveType == 1 && map.staticTerrainSP[map.IndexByCoord(i,j)] != TerrainType.Wall) posList.Add(new Coord(i,j));
				if(mData.moveType == 2 && map.staticTerrainSP[map.IndexByCoord(i,j)] != TerrainType.Wall)posList.Add(new Coord(i,j));
				if(mData.moveType == 3 && map.staticTerrainSP[map.IndexByCoord(i,j)] != TerrainType.Wall)posList.Add(new Coord(i,j));
				if(mData.moveType == 4 && map.IsTileCanWalk(i,j))posList.Add(new Coord(i,j));
			}
		}

		if(posList.Count == 0)posList.Add(new Coord(x,y));

		var densityMap = BattleMap.ins.densityMapWalk;
		var density = densityMap[map.IndexByCoord(posList[0].x,posList[0].y)];
		var genPos = new Coord(posList[0].x,posList[0].y);

		foreach(var dir in posList){
			if(densityMap[map.IndexByCoord(dir.x,dir.y)] < density){
				density = densityMap[map.IndexByCoord(dir.x,dir.y)];
				genPos.x = dir.x;
				genPos.y = dir.y;
			}
		}

		densityMap[map.IndexByCoord(genPos.x,genPos.y)]++;

		return genPos;
	}

	public bool hasFlyMonster(){
		foreach(var m in data.monsters){
			var mData = Configs.GetMonster(m);
			if(mData!=null&&mData.moveType == 1)return true;
		}
		return false;
	}

	public bool isBoss(){
		return data.isBoss;
	}

	public void Remove(){
		isDone = true;
	}
}
