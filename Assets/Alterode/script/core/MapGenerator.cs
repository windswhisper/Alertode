using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;


/*随机地图块数据*/
[Serializable]
public class MapBlockData{
	public int id;
	public List<int> tiles;
	public List<MapObjectID> objs;

	public MapBlockData(){
	}
	public MapBlockData(MapBlockData blockData){
		this.id = blockData.id;
		this.tiles = new List<int>(blockData.tiles);
		this.objs = new List<MapObjectID>();
		foreach(var obj in blockData.objs){
			this.objs.Add(new MapObjectID(obj));
		}
	}

}

/*随机地图块配置*/
[Serializable]
public class MapBlockConfigs{
	public int mapBlockWidth;
	public int mapBlockNum;
	public int baseBlockCount;
	public List<int> monsterPointCount;
	public List<int> monsterHpTotal;
	public List<MapBlockData> datas;
	public List<MapBlockIndex> indexs;
	public string style;
}

/*随机地图块索引*/
[Serializable]
public class MapBlockIndex{
	public int id;
	public string sideType;
	public int rotate;

	public MapBlockIndex(int id,string sideType,int rotate){
		this.id = id;
		this.sideType = sideType;
		this.rotate = rotate;
	}
}


public class MapGenerator
{
	static int MAP_BLOCK_WIDTH = 5;
	static int MAP_BLOCK_NUM = 5;

	static MapBlockConfigs mbConfigs;
	static List<MapBlockIndex> blockIndexRotate;

	static MapData mapData;
	static List<string> blockTypeMap;
	static List<MapBlockIndex> mapIndexList;

	static List<MonsterData> listMonsterType;

	public static MapData GenMap(string configName){
		mbConfigs = JsonUtility.FromJson<MapBlockConfigs> (Resources.Load<TextAsset>("randMap/"+configName).text);
		MAP_BLOCK_WIDTH = mbConfigs.mapBlockWidth;
		MAP_BLOCK_NUM = mbConfigs.mapBlockNum;
		blockIndexRotate = new List<MapBlockIndex>();
		for(var r=0;r<4;r++){
			for(var i=0;i<mbConfigs.indexs.Count;i++){
				if(mbConfigs.indexs[i].id<100)continue;
				blockIndexRotate.Add(new MapBlockIndex(mbConfigs.indexs[i].id,RotateSide(mbConfigs.indexs[i].sideType,r),r));
			}
		}

        mapData = new MapData(MAP_BLOCK_WIDTH*MAP_BLOCK_NUM, MAP_BLOCK_WIDTH*MAP_BLOCK_NUM);
        mapData.wave = 50;
        mapData.waveSecondOffset = 10;
        mapData.exStartCoin = 100;
        mapData.style = mbConfigs.style;

		blockTypeMap = new List<string>();
		mapIndexList = new List<MapBlockIndex>();

		for(var i=0;i<MAP_BLOCK_NUM;i++){
			for(var j=0;j<MAP_BLOCK_NUM;j++){
				blockTypeMap.Add(null);
				mapIndexList.Add(null);
			}
		}

		var baseIndex = TSRandom.Range(0,mbConfigs.baseBlockCount);
		mapIndexList[MAP_BLOCK_NUM*MAP_BLOCK_NUM/2] = mbConfigs.indexs[baseIndex];
		blockTypeMap[MAP_BLOCK_NUM*MAP_BLOCK_NUM/2] = mbConfigs.indexs[baseIndex].sideType;

		for(var i=0;i<MAP_BLOCK_NUM;i++){
			for(var j=0;j<MAP_BLOCK_NUM;j++){
				if(blockTypeMap[i+j*MAP_BLOCK_NUM]==null)FindBlock(i,j);
			}
		}

		mapData.width = MAP_BLOCK_NUM*MAP_BLOCK_WIDTH;
		mapData.height = MAP_BLOCK_NUM*MAP_BLOCK_WIDTH;

		for(var i=0;i<MAP_BLOCK_NUM;i++){
			for(var j=0;j<MAP_BLOCK_NUM;j++){
				PutBlock(i,j);
			}
		}

		for(var i=0;i<mapData.width;i++){
			mapData.map[i] = 1;
			mapData.map[i*mapData.width] = 1;
			mapData.map[i+mapData.width*(mapData.width-1)] = 1;
			mapData.map[mapData.width-1+i*mapData.width] = 1;
		}

        listMonsterType = new List<MonsterData>();
        var tempMonsterSmallList = new List<MonsterData>();
        var tempMonsterBigList = new List<MonsterData>();
		foreach(var m in Configs.monsterConfig.datas){
			if(m.strength<20000){
				if(m.strength>1000)
					tempMonsterBigList.Add(m);
				else
					tempMonsterSmallList.Add(m);
			}
		}
		Utils.RandomList<MonsterData>(tempMonsterSmallList);
		Utils.RandomList<MonsterData>(tempMonsterBigList);
        for(var i=0;i<4;i++){
        	listMonsterType.Add(tempMonsterBigList[i]);
        }
        for(var i=0;i<6;i++){
        	listMonsterType.Add(tempMonsterSmallList[i]);
        }
		foreach(var m in Configs.monsterConfig.datas){
			if(m.strength>=20000){
        		listMonsterType.Add(m);
			}
		}

        for(var i=1;i<=50;i++){
        	var count = mbConfigs.monsterPointCount[(i-1)%10];
        	if(i<=40)count +=  i/10-1;
        	else count+=3;
        	for(var n=0;n<count;n++){
        		var hpTotal = mbConfigs.monsterHpTotal[(i-1)%10];
        		if(i<5)hpTotal/=2;
        		else if(i<=10)hpTotal=hpTotal*2/3;
        		var mg = GetRandMonster(hpTotal,i<=3);
        		mg.wave = i;
        		if(i%10==0){
        			mg.repeat*=3;
        		}
        		mapData.monGens.Add(mg);
        	}
        	if(i%10==0){
        		var mg = GetRandBoss(0,false);
        		mg.wave = i;
        		mapData.monGens.Add(mg);
        	}
        }

		return mapData;
	}

	public static MonsterGeneratorData GetRandMonster(int hpTotal, bool banFly){
		var data = new MonsterGeneratorData();

		var monsters = new List<MonsterData>();
		foreach(var m in listMonsterType){
			if(m.strength<20000 && m.strength<=hpTotal){
				if(banFly&&m.moveType==1)continue;
				if(banFly&&m.strength>1000)continue;
				monsters.Add(m);
			}
		}
		var monster = monsters[TSRandom.Range(0,monsters.Count)];
		if(monster.moveType==1)hpTotal/=2;
		data.monsters.Add(monster.name);
		data.repeat =  hpTotal/monster.strength;
		data.deltaTime = 8000/data.repeat;
		data.randomEdge = 1;
		return data;
	}

	public static MonsterGeneratorData GetRandBoss(int hpTotal, bool banFly){
		var data = new MonsterGeneratorData();

		var monsters = new List<MonsterData>();
		foreach(var m in Configs.monsterConfig.datas){
			if(m.strength>20000 ){
				if(banFly&&m.moveType==1)continue;
				monsters.Add(m);
			}
		}
		var monster = monsters[TSRandom.Range(0,monsters.Count)];
		if(monster.moveType==1)hpTotal/=2;
		data.monsters.Add(monster.name);
		data.repeat =  1;
		data.deltaTime = 100;
		data.randomEdge = 1;
		data.isBoss = true;
		return data;
	}


	public static void FindBlock(int x,int y){
		List<char> matchSides = new List<char>();
		for(var i=0;i<MAP_BLOCK_WIDTH;i++){
			matchSides.Add('-');
		}

		if(y<MAP_BLOCK_NUM-1 && blockTypeMap[x+(y+1)*MAP_BLOCK_NUM]!=null){
			matchSides[0] = blockTypeMap[x+(y+1)*MAP_BLOCK_NUM][2];
		}
		if(x<MAP_BLOCK_NUM-1 && blockTypeMap[x+1+y*MAP_BLOCK_NUM]!=null){
			matchSides[1] = blockTypeMap[x+1+y*MAP_BLOCK_NUM][3];
		}
		if(y>0 && blockTypeMap[x+(y-1)*MAP_BLOCK_NUM]!=null){
			matchSides[2] = blockTypeMap[x+(y-1)*MAP_BLOCK_NUM][0];
		}
		if(x>0 && blockTypeMap[x-1+y*MAP_BLOCK_NUM]!=null){
			matchSides[3] = blockTypeMap[x-1+y*MAP_BLOCK_NUM][1];
		}

		var list = new List<MapBlockIndex>();

		foreach(var data in blockIndexRotate){
			bool notMatch = false;
			for(var i=0;i<MAP_BLOCK_WIDTH;i++){
				if(matchSides[i]!='-' && data.sideType[i]!='-' && matchSides[i]!=data.sideType[i]){
					notMatch = true;
				}
			}
			if(!notMatch)list.Add(data);
		}

		if(list.Count==0)list.Add(blockIndexRotate[TSRandom.Range(0,blockIndexRotate.Count)]);

		var block = list[TSRandom.Range(0,list.Count)];
		mapIndexList[x+y*MAP_BLOCK_NUM] = block;
		blockTypeMap[x+y*MAP_BLOCK_NUM] = block.sideType;
		
	}

	public static void PutBlock(int blockX,int blockY){
		var index = mapIndexList[blockX+blockY*MAP_BLOCK_NUM];
		var blockData = GetBlockData(index.id);

		blockData =  RotateBlock(blockData,index.rotate);

		for(var i=0;i<MAP_BLOCK_WIDTH;i++){
			for(var j=0;j<MAP_BLOCK_WIDTH;j++){
				mapData.map[i+blockX*MAP_BLOCK_WIDTH+(j+blockY*MAP_BLOCK_WIDTH)*mapData.width] = blockData.tiles[i+j*MAP_BLOCK_WIDTH];
			}	
		}

		foreach(var obj in blockData.objs){
			obj.x+=blockX*MAP_BLOCK_WIDTH;
			obj.y+=blockY*MAP_BLOCK_WIDTH;

			if((blockX*11+blockY*7)%10>4 && obj.id==2)continue;
			mapData.objs.Add(new MapObjectID(obj));
		}
	}

	public static MapBlockData GetBlockData(int id){
		foreach(var block in mbConfigs.datas){
			if(block.id == id)return block;
		}
		return null;
	}

	public static string RotateSide(string side,int rotate){
		string sideRoted = "";

		for(var i=0;i<4;i++){
			sideRoted += side[(i-rotate+4)%4];
		}

		return sideRoted;
	}

	public static MapBlockData RotateBlock(MapBlockData blockData,int rotate){
		MapBlockData data = new MapBlockData(blockData);
		var tilesNew = new List<int>();
		for(var m=0;m<MAP_BLOCK_WIDTH;m++)
			for(var n=0;n<MAP_BLOCK_WIDTH;n++){
				tilesNew.Add(0);
			}

		for(var i=0;i<rotate;i++){
			foreach(var obj in data.objs){
				int x = obj.x;
				obj.x = obj.y;
				obj.y = MAP_BLOCK_WIDTH-x-1;
			}
			for(var m=0;m<MAP_BLOCK_WIDTH;m++)
				for(var n=0;n<MAP_BLOCK_WIDTH;n++){
					tilesNew[n+(MAP_BLOCK_WIDTH-m-1)*MAP_BLOCK_WIDTH] = data.tiles[m+n*MAP_BLOCK_WIDTH];
				}
			for(var m=0;m<MAP_BLOCK_WIDTH;m++)
				for(var n=0;n<MAP_BLOCK_WIDTH;n++){
					int tileId = tilesNew[m+n*MAP_BLOCK_WIDTH];
					if(tileId==4)tilesNew[m+n*MAP_BLOCK_WIDTH]=5;
					else if(tileId==5)tilesNew[m+n*MAP_BLOCK_WIDTH]=4;
					else if(tileId>=6&&tileId<=8)tilesNew[m+n*MAP_BLOCK_WIDTH]=tileId+1;
					else if(tileId==9)tilesNew[m+n*MAP_BLOCK_WIDTH]=6;
					data.tiles[m+n*MAP_BLOCK_WIDTH] = tilesNew[m+n*MAP_BLOCK_WIDTH];
				}
		}

		foreach(var obj in data.objs){
			obj.rotation = rotate*90;
		}

		return data;
	}
}
