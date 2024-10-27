using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using LPWAsset;


//（0，1）为正北方向，（1，0）为正东方向

public enum TerrainType{
	Water,
	Ground,
	LandingSN,
	LandingEW,
	Wall
}

//地图数据类
[Serializable]
public class MapData{
	//地板数据
	public List<int> map;
	//物体数据
	public List<MapObjectID> objs;
	//地图总波次
	public int wave;
	//地图宽度
	public int width;
    //地图长度
    public int height;
    //额外初始金币
    public int exStartCoin;
    //每波时长修正
    public int waveSecondOffset;
    //每波金币修正
    public int waveCoinOffset;
    //地图风格
    public string style;
    //战争迷雾
    public bool fog;
	//刷怪数据
	public List<MonsterGeneratorData> monGens;
	//标签，用于筛选可用防御塔
	public List<string> tags;
	//反面标签，用于筛选可用防御塔
	public List<string> negativeTags;

	public MapData(int width,int height){
		this.width = width;
		this.height = height;
		this.map = new List<int>();
		this.objs = new List<MapObjectID>();
        this.monGens = new List<MonsterGeneratorData>();
        this.tags = new List<string>();
        this.negativeTags = new List<string>();

		for(var i=0;i<width*height;i++){
			this.map.Add(0);
		}
	}
}
//地图物体数据类
[Serializable]
public class MapObjectID{
	public int x;
	public int y;
	public int id;
    public int para;
    public string namePara;
    public int rotation;
    [NonSerialized]
    public GameObject gameObject;

    public MapObjectID(int x,int y,int id){
        this.x = x;
        this.y = y;
        this.id = id;
    }
    public MapObjectID(MapObjectID obj){
        this.x = obj.x;
        this.y = obj.y;
        this.id = obj.id;
        this.para = obj.para;
        this.namePara = obj.namePara;
        this.rotation = obj.rotation;
    }
}
//地图刷怪数据类
[Serializable]
public class MonsterGeneratorData{
	public int x;
	public int y;
	public int wave;
	public List<string> monsters;
	public int delayTime = 0;
	public int deltaTime = 300;
	public int repeat = 1;
    public int repeatDelta = 0;
	public bool isBoss = false;
    public int randomEdge = 0;

	public MonsterGeneratorData(){
		delayTime = 0;
		deltaTime = 300;
		repeat = 1;
        monsters = new List<string>();
	}
}


public class BattleMap : MonoBehaviour
{
	public static BattleMap ins;

	public MapData mapData;

	public Transform tileLayer;
    public List<GameObject> tileList;
    public List<GameObject> objList;
    public LowPolyWaterScript lpWater;
    public Transform fogLayer;
    public List<GameObject> fogList;
    public GameObject fogPrefab;
    public SpriteRenderer[] fogsEdge;
    public Material lavaWaterMat;

    //建筑占用表
    public List<int> buildMap = new List<int>();
    //临时建筑占用表
    public List<int> tempBuildMap = new List<int>();
    //战争迷雾表
    public List<int> fogMap = new List<int>();
	//用于建筑的静态地形表
	public List<TerrainType> staticTerrain = new List<TerrainType>();
	//用于寻路的静态地形表
	public List<TerrainType> staticTerrainSP = new List<TerrainType>();
	//实时路径流向地图
	public List<int> pathMapWalk = new List<int>();
	public List<int> pathMapSwim = new List<int>();
	public List<int> pathMapHover = new List<int>();
    bool spMapChanged = false;

	List<Coord> searchingPoints = new List<Coord>();
	public List<Coord> basePoints = new List<Coord>();
	public List<int> densityMapWalk = new List<int>();
    public List<BaseMapScript> scripts = new List<BaseMapScript>();
    public List<int> crowdedMapGround = new List<int>();
    public List<int> crowdedMapFly = new List<int>();

    static string[] bgmGreen = {"Pirate Folk Adventures - Master_Logan"};
    static string[] bgmWar = {"Epic Pirate Battle - Master_Logan"};
    static string[] bgmLake = {"Russian Medieval - Maestro Misha2012"};
    static string[] bgmSand = {"Mysteries of Ancient Egypt - Revturkey"};
    static string[] bgmSnow = {"Northern Medieval Song - Master_Logan"};
    static string[] bgmLava = {"Renaissance Song - Master_Logan"};
    static string[] bgmTemple = {"Scottish Celtic Ballad_Master Logan"};

	void Awake(){
		ins = this;
	}

    public void LoadMap(string fileName){
        var mapFile = Resources.Load<TextAsset>("map/"+fileName);

        var data = JsonUtility.FromJson<MapData>(mapFile.text);

        InitMap(data);
        Step(0);
    }

    public void LoadRandMap(string fileName){
        var data = MapGenerator.GenMap(fileName);

        InitMap(data);

        Step(0);
    }

    public void InitMap(MapData mapData){
    	this.mapData = mapData;

        BattleManager.ins.engine.coin += mapData.exStartCoin;

        lpWater.sizeX = mapData.width+3;
        lpWater.sizeZ = mapData.height+3;
        if(mapData.style == "Lava"){
            lpWater.material = lavaWaterMat;
        }
        lpWater.Generate();
        lpWater.transform.localPosition = new Vector3(mapData.width/2f-0.5f,-0.5f,mapData.height/2f-0.5f);

        fogsEdge[0].size = new Vector2(50.2f,mapData.height+50);
        fogsEdge[2].size = new Vector2(50.2f,mapData.height+50);
        fogsEdge[1].size = new Vector2(mapData.width+50,50.2f);
        fogsEdge[3].size = new Vector2(mapData.width+50,50.2f);
        fogsEdge[0].transform.localPosition = new Vector3(-25.5f,0,mapData.height/2f);
        fogsEdge[2].transform.localPosition = new Vector3(24.5f+mapData.width,0,mapData.height/2f);
        fogsEdge[1].transform.localPosition = new Vector3(mapData.width/2f,0,-25.5f);
        fogsEdge[3].transform.localPosition = new Vector3(mapData.width/2f,0,24.5f+mapData.height);

        if(mapData.fog || BattleManager.ins.gameMode >= 2){
            fogsEdge[0].gameObject.layer = LayerMask.NameToLayer("FogOfWar");
            fogsEdge[1].gameObject.layer = LayerMask.NameToLayer("FogOfWar");
            fogsEdge[2].gameObject.layer = LayerMask.NameToLayer("FogOfWar");
            fogsEdge[3].gameObject.layer = LayerMask.NameToLayer("FogOfWar");
        }

        PlayBgm();

        tileList.Clear();
        objList.Clear();
    	staticTerrain.Clear();
    	staticTerrainSP.Clear();
    	pathMapWalk.Clear();
    	pathMapSwim.Clear();
    	pathMapHover.Clear();
    	densityMapWalk.Clear();
        scripts.Clear();
    	for(var y=0;y<mapData.height;y++){
    		for(var x=0;x<mapData.width;x++){
				var index = IndexByCoord(x,y);
				if(mapData.map[index] != 0){
					var tile = Instantiate(Resources.Load<GameObject>(string.Format("prefab/map/mapTile{0:D3}",mapData.map[index])));
					tile.transform.SetParent(tileLayer,false);
					tile.transform.localPosition = new Vector3(x,-0.5f,y);
					tileList.Add(tile);
					var data = Configs.mapConfig.tileDatas[mapData.map[index]-1];
					if(data.landing){
						if(data.landingType == 0){
							staticTerrain.Add(TerrainType.LandingSN);
							staticTerrainSP.Add(TerrainType.LandingSN);
						}
						else{
							staticTerrain.Add(TerrainType.LandingEW);
							staticTerrainSP.Add(TerrainType.LandingEW);
						}
					}
					else{
						if(data.buildable)
							staticTerrain.Add(TerrainType.Ground);
						else
							staticTerrain.Add(TerrainType.Wall);
							
						if(data.walkable)
							staticTerrainSP.Add(TerrainType.Ground);
						else
							staticTerrainSP.Add(TerrainType.Wall);
					}
				}
				else{
					tileList.Add(null);
					staticTerrain.Add(TerrainType.Water);
					staticTerrainSP.Add(TerrainType.Water);
				}

                if(mapData.fog || BattleManager.ins.gameMode >= 2){
                    var fog = Instantiate(fogPrefab);
                    fog.transform.SetParent(fogLayer,false);
                    fog.transform.localPosition = new Vector3(x,0,y);
                    fogList.Add(fog);
                }

				pathMapWalk.Add(999);
				pathMapSwim.Add(999);
				pathMapHover.Add(999);
				densityMapWalk.Add(0);
                fogMap.Add(0);
                buildMap.Add(0);
                tempBuildMap.Add(0);
                crowdedMapGround.Add(0);
                crowdedMapFly.Add(0);

            }
		}
    	
    	for(var i=0;i<mapData.objs.Count;i++){
    		var obj = mapData.objs[i];

    		var data = Configs.mapConfig.mapObjectDatas[obj.id-1];
    		if(data.isBuild){
    			mapData.objs.Remove(obj);
    			if(data.paraStr == "Base")basePoints.Add(new Coord(obj.x,obj.y));
                i--;
                if(BattleManager.ins.isLoadGame)continue;
                var build = BattleManager.ins.PutBuild(data.paraStr,obj.x,obj.y,false,true);
                if(build.data.name == "Obelisk"){
                    ((BuildObelisk)build).SetTech(obj.namePara);
                }
    			continue;
    		}
            else if(data.isScript){
                var script = ObjectFactory.CreateMapScript(data.paraStr);
                script.Init(obj.x,obj.y,obj.para,obj.namePara);
                scripts.Add(script);
            }
    		else if(!data.isInviso){
                PutMapObj(obj);
    		}

    		if(!data.walkable)staticTerrainSP[IndexByCoord(obj.x,obj.y)] = TerrainType.Wall;
    		if(!data.buildable)staticTerrain[IndexByCoord(obj.x,obj.y)] = TerrainType.Wall;
    	}

    	SearchPathWalk();
    	SearchPathSwim();
    	SearchPathHover();
    }

    public void AddMapObj(int x,int y,int objId){
        var obj = new MapObjectID(x,y,objId);
        mapData.objs.Add(obj);
        obj.gameObject = PutMapObj(obj);
    }

    GameObject PutMapObj(MapObjectID obj){
        var tile = Instantiate(Resources.Load<GameObject>(string.Format("prefab/map/mapObj{0:D3}",obj.id)));
        tile.transform.SetParent(tileLayer,false);
        tile.transform.localPosition = new Vector3(obj.x,0,obj.y);
        tile.transform.localRotation = Quaternion.Euler(0,obj.rotation,0);
        objList.Add(tile);
        return tile;
    }

    public void RemoveMapObj(int x,int y,int objId){
        for(var i=0;i<mapData.objs.Count;i++){
            var obj = mapData.objs[i];
            if(obj.x == x && obj.y == y && obj.id == objId){
                mapData.objs.Remove(obj);
                objList.Remove(obj.gameObject);
                Destroy(obj.gameObject);
                break;
            }
        }
    }

    public void PlayBgm(){

        switch(mapData.style){
            case "Green":
                VolumeManager.ins.PlayBgm(bgmGreen[UnityEngine.Random.Range(0,bgmGreen.Length)]);
                break;
            case "War":
                VolumeManager.ins.PlayBgm(bgmWar[UnityEngine.Random.Range(0,bgmWar.Length)]);
                break;
            case "Lake":
                VolumeManager.ins.PlayBgm(bgmLake[UnityEngine.Random.Range(0,bgmLake.Length)]);
                break;
            case "Sand":
                VolumeManager.ins.PlayBgm(bgmSand[UnityEngine.Random.Range(0,bgmSand.Length)]);
                break;
            case "Snow":
                VolumeManager.ins.PlayBgm(bgmSnow[UnityEngine.Random.Range(0,bgmSnow.Length)]);
                break;
            case "Lava":
                VolumeManager.ins.PlayBgm(bgmLava[UnityEngine.Random.Range(0,bgmLava.Length)]);
                break;
            case "Temple":
                VolumeManager.ins.PlayBgm(bgmTemple[UnityEngine.Random.Range(0,bgmTemple.Length)]);
                break;
        }
    }

    public void SearchPathWalk(){
    	searchingPoints.Clear();
    	for(var i=0;i<mapData.width*mapData.height;i++){
    		if(staticTerrainSP[i] == TerrainType.Ground)
    		{
    			pathMapWalk[i] = 999;
    		}
    		else{
    			pathMapWalk[i] = -2;
    		}
    	}

    	foreach(var basePos in basePoints){
    		pathMapWalk[IndexByCoord(basePos)] = 0;
		    searchingPoints.Add(new Coord(basePos.x,basePos.y));
    	}

    	SearchingPathWalk(0);
    }

    public void SearchPathSwim(){
        searchingPoints.Clear();
        for(var i=0;i<mapData.width*mapData.height;i++){
            if(staticTerrainSP[i] != TerrainType.Wall)
            {
                pathMapSwim[i] = 999;
            }
            else{
                pathMapSwim[i] = -2;
            }
        }

        foreach(var basePos in basePoints){
            pathMapSwim[IndexByCoord(basePos)] = 0;
            searchingPoints.Add(new Coord(basePos.x,basePos.y));
        }

        SearchingPathSwim(0);
    }

    public void SearchPathHover(){
        searchingPoints.Clear();
        for(var i=0;i<mapData.width*mapData.height;i++){
            if(staticTerrainSP[i] != TerrainType.Wall)
            {
                pathMapHover[i] = 999;
            }
            else{
                pathMapHover[i] = -2;
            }
        }

        foreach(var basePos in basePoints){
            pathMapHover[IndexByCoord(basePos)] = 0;
            searchingPoints.Add(new Coord(basePos.x,basePos.y));
        }

        SearchingPathHover(0);

        // var str = "";
        // for(var i=0;i<mapData.width;i++){
        //  for(var j=0;j<mapData.height;j++){
        //      str+=pathMapHover[j*mapData.width+i]+",";
        //  }
        //  str+="\n";
        // }
        // Debug.Log(str);
    }

    void SearchingPathWalk(int step){
    	List<Coord> newPoints = new List<Coord>();
    	step++;
    	foreach(var p in searchingPoints){
    		var i = IndexByCoord(p);
    		if(p.x>0){
    			var iEast = IndexByCoord(p.x-1,p.y);
    			if(staticTerrainSP[iEast]==TerrainType.Ground){
    				if(pathMapWalk[iEast] > step){
	    				newPoints.Add(new Coord(p.x - 1,p.y));
	                    pathMapWalk[iEast] = step;
    				}
    			}
    		}
    		if(p.y>0){
    			var iSouth = IndexByCoord(p.x,p.y-1);
    			if(staticTerrainSP[iSouth]==TerrainType.Ground){
    				if(pathMapWalk[iSouth] > step){
	    				newPoints.Add(new Coord(p.x,p.y - 1));
	                    pathMapWalk[iSouth] = step;
    				}
    			}
    		}

    		if(p.x<mapData.width-1){
    			var iWest = IndexByCoord(p.x+1,p.y);
    			if(staticTerrainSP[iWest]==TerrainType.Ground){
    				if(pathMapWalk[iWest] > step){
	    				newPoints.Add(new Coord(p.x + 1,p.y));
	                    pathMapWalk[iWest] = step;
    				}
    			}
    		}
    		if(p.y<mapData.height-1){
    			var iNorth = IndexByCoord(p.x,p.y+1);
    			if(staticTerrainSP[iNorth]==TerrainType.Ground){
    				if(pathMapWalk[iNorth] > step){
	    				newPoints.Add(new Coord(p.x,p.y + 1));
	                    pathMapWalk[iNorth] = step;
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

    void SearchingPathSwim(int step){
    	List<Coord> newPoints = new List<Coord>();
    	step++;
    	foreach(var p in searchingPoints){
    		var i = IndexByCoord(p);
    		if(p.x>0){
    			var iEast = IndexByCoord(p.x-1,p.y);
    			if(staticTerrainSP[iEast]!=TerrainType.Wall && (staticTerrainSP[iEast] == staticTerrainSP[i] || staticTerrainSP[iEast] == TerrainType.LandingEW || staticTerrainSP[i] == TerrainType.LandingEW)){
    				if(pathMapSwim[iEast] > step){
	    				newPoints.Add(new Coord(p.x - 1,p.y));
	                    pathMapSwim[iEast] = step;
    				}
    			}
    		}
    		if(p.y>0){
    			var iSouth = IndexByCoord(p.x,p.y-1);
    			if(staticTerrainSP[iSouth]!=TerrainType.Wall && (staticTerrainSP[iSouth] == staticTerrainSP[i] || staticTerrainSP[iSouth] == TerrainType.LandingSN || staticTerrainSP[i] == TerrainType.LandingSN)){
    				if(pathMapSwim[iSouth] > step){
	    				newPoints.Add(new Coord(p.x,p.y - 1));
	                    pathMapSwim[iSouth] = step;
    				}
    			}
    		}

    		if(p.x<mapData.width-1){
    			var iWest = IndexByCoord(p.x+1,p.y);
    			if(staticTerrainSP[iWest]!=TerrainType.Wall && (staticTerrainSP[iWest] == staticTerrainSP[i] || staticTerrainSP[iWest] == TerrainType.LandingEW || staticTerrainSP[i] == TerrainType.LandingEW)){
    				if(pathMapSwim[iWest] > step){
	    				newPoints.Add(new Coord(p.x + 1,p.y));
	                    pathMapSwim[iWest] = step;
    				}
    			}
    		}
    		if(p.y<mapData.height-1){
    			var iNorth = IndexByCoord(p.x,p.y+1);
    			if(staticTerrainSP[iNorth]!=TerrainType.Wall && (staticTerrainSP[iNorth] == staticTerrainSP[i] || staticTerrainSP[iNorth] == TerrainType.LandingSN || staticTerrainSP[i] == TerrainType.LandingSN)){
    				if(pathMapSwim[iNorth] > step){
	    				newPoints.Add(new Coord(p.x,p.y + 1));
	                    pathMapSwim[iNorth] = step;
    				}
    			}
    		}
    	}

    	if(newPoints.Count == 0)return;

    	searchingPoints.Clear();
    	foreach(var coord in newPoints){
    		searchingPoints.Add(coord);
    	}
    	SearchingPathSwim(step);
    }

    void SearchingPathHover(int step){
        List<Coord> newPoints = new List<Coord>();
        step++;
        foreach(var p in searchingPoints){
            var i = IndexByCoord(p);
            if(p.x>0){
                var iEast = IndexByCoord(p.x-1,p.y);
                if(staticTerrainSP[iEast]!=TerrainType.Wall){
                    if(pathMapHover[iEast] > step){
                        newPoints.Add(new Coord(p.x - 1,p.y));
                        pathMapHover[iEast] = step;
                    }
                }
            }
            if(p.y>0){
                var iSouth = IndexByCoord(p.x,p.y-1);
                if(staticTerrainSP[iSouth]!=TerrainType.Wall){
                    if(pathMapHover[iSouth] > step){
                        newPoints.Add(new Coord(p.x,p.y - 1));
                        pathMapHover[iSouth] = step;
                    }
                }
            }

            if(p.x<mapData.width-1){
                var iWest = IndexByCoord(p.x+1,p.y);
                if(staticTerrainSP[iWest]!=TerrainType.Wall){
                    if(pathMapHover[iWest] > step){
                        newPoints.Add(new Coord(p.x + 1,p.y));
                        pathMapHover[iWest] = step;
                    }
                }
            }
            if(p.y<mapData.height-1){
                var iNorth = IndexByCoord(p.x,p.y+1);
                if(staticTerrainSP[iNorth]!=TerrainType.Wall){
                    if(pathMapHover[iNorth] > step){
                        newPoints.Add(new Coord(p.x,p.y + 1));
                        pathMapHover[iNorth] = step;
                    }
                }
            }
        }

        if(newPoints.Count == 0)return;

        searchingPoints.Clear();
        foreach(var coord in newPoints){
            searchingPoints.Add(coord);
        }
        SearchingPathHover(step);
    }

    public void Step(FP dt){
        UpdateFogLayer();
        UpdateCrowdedMap();

        if(spMapChanged){
            spMapChanged = false;

            SearchPathWalk();
            SearchPathSwim();
            SearchPathHover();
        }
    }

    public void UpdateFogLayer()
    {
        if (BattleManager.ins.gameMode == 3 || BattleManager.ins.gameMode == 2)
        {
            int wave = BattleManager.ins.engine.wave;

            var radius = (wave - 1) / 10 * 2 + 7;
            if (radius > 14) radius = 14;

            for (var y = 0; y < mapData.height; y++)
            {
                for (var x = 0; x < mapData.width; x++)
                {
                    if (TSMath.Abs(x - mapData.width / 2) > radius || TSMath.Abs(y - mapData.height / 2) > radius)
                    {
                        fogMap[IndexByCoord(x, y)] = 1;
                        fogList[IndexByCoord(x, y)].SetActive(true);
                    }
                    else
                    {
                        fogMap[IndexByCoord(x, y)] = 0;
                        fogList[IndexByCoord(x, y)].SetActive(false);
                    }
                }
            }
        }
        else if (mapData.fog)
        {
            var radius = Configs.commonConfig.disfogRadius;

            for (var y = 0; y < mapData.height; y++)
            {
                for (var x = 0; x < mapData.width; x++)
                {
                    fogMap[IndexByCoord(x, y)] = 1;
                    fogList[IndexByCoord(x, y)].SetActive(true);
                }
            }
            for (var y = 0; y < mapData.height; y++)
            {
                for (var x = 0; x < mapData.width; x++)
                {
                    var index = IndexByCoord(x, y);
                    if (buildMap[index] != 0)
                    {
                        for (var i = x - radius + 1; i < x + radius; i++)
                        {
                            for (var j = y - radius + 1; j < y + radius; j++)
                            {
                                if (Mathf.Abs(i - x) + Mathf.Abs(j - y) <= radius && IsCoordInMap(i, j))
                                {
                                    fogMap[IndexByCoord(i, j)] = 0;
                                    fogList[IndexByCoord(i, j)].SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    public void UpdateCrowdedMap()
    {
        for (var y = 0; y < mapData.height; y++)
        {
            for (var x = 0; x < mapData.width; x++)
            {
                var index = IndexByCoord(x, y);
                crowdedMapGround[index] = 0;
                crowdedMapFly[index] = 0;
            }
        }

        foreach(var u in BattleManager.ins.engine.unitList)
        {
            if(u.type == UnitType.Monster)
            {
                if(((BaseMonster)u).moveType == MoveType.Fly)
                {
                    crowdedMapFly[IndexByCoord(new Coord(u.position.x, u.position.z))]++;
                }
                else
                {
                    crowdedMapGround[IndexByCoord(new Coord(u.position.x, u.position.z))]++;
                }
            }
        }
    }

    public void OnDayStart(){
        foreach(var script in scripts){
            if(script.enable){
                script.OnDayStart();
            }
        }
        for(var i=scripts.Count-1;i>=0;i--){
            if(scripts[i].isRemove){
                scripts.Remove(scripts[i]);
            }
        }
    }

    public void OnNightStart(){
        foreach(var script in scripts){
            if(script.enable){
                script.OnNightStart();
            }
        }
    }

    public void PutWaveMonster(int wave){
        var edgePosList = new List<Coord>();
        var edgePosOccupy = new List<Coord>();

        if(BattleManager.ins.gameMode >= 2  && wave>50)wave=(wave-1)%40+10;

    	foreach(var data in mapData.monGens){
    		if(data.wave == wave){
                if(data.randomEdge != 0){
                    edgePosList.Clear();
                    if(data.randomEdge==1){
                        for(var i=0;i<mapData.width;i++){   
                            edgePosList.Add(new Coord(i,0));
                            edgePosList.Add(new Coord(i,mapData.height-1));
                        }
                        
                        for(var j=1;j<mapData.height-1;j++){   
                            edgePosList.Add(new Coord(0,j));
                            edgePosList.Add(new Coord(mapData.width-1,j));
                        }
                    }
                    else if(data.randomEdge == 2){
                        for(var i=0;i<mapData.width;i++){   
                            edgePosList.Add(new Coord(i,mapData.height-1));
                        }
                    }
                    else if(data.randomEdge == 3){
                        for(var j=0;j<mapData.height;j++){   
                            edgePosList.Add(new Coord(mapData.width-1,j));
                        }
                    }
                    else if(data.randomEdge == 4){
                        for(var i=0;i<mapData.width;i++){   
                            edgePosList.Add(new Coord(i,0));
                        }
                    }
                    else if(data.randomEdge == 5){
                        for(var j=0;j<mapData.height;j++){   
                            edgePosList.Add(new Coord(0,j));
                        }
                    }

                    for(var i=0;i<edgePosOccupy.Count;i++){
                        for(var j=edgePosList.Count-1;j>=0;j--){
                            if(edgePosOccupy[i].EqualTo(edgePosList[j])){
                                edgePosList.Remove(edgePosList[j]);
                            }
                        }
                    }
                    if(edgePosList.Count == 0)continue;
                    var p = edgePosList[TSRandom.Range(0,edgePosList.Count)];
                    edgePosOccupy.Add(p);
                    data.x = p.x;
                    data.y = p.y;
                }
    			BattleManager.ins.PutMonsterGenerator(data);
    		}
    	}
    }

    public void ChangeTileBuild(int x,int y,TerrainType type){
        staticTerrain[IndexByCoord(x,y)] = type;
    }
    public void ChangeTileSP(int x,int y,TerrainType type){
        staticTerrainSP[IndexByCoord(x,y)] = type;
        spMapChanged = true;

    }
    public void SetFloatTileStatus(int x,int y,bool floating){
        if(floating){
            tileList[IndexByCoord(x,y)].GetComponent<Animation>().Play("tile_float_up");
        }
        else{
            tileList[IndexByCoord(x,y)].GetComponent<Animation>().Play("tile_float_down");
        }
    }

    public void HideFog(){
        mapData.fog = false;

            for (var y = 0; y < mapData.height; y++)
            {
                for (var x = 0; x < mapData.width; x++)
                {
                    fogMap[IndexByCoord(x, y)] = 0;
                    fogList[IndexByCoord(x, y)].SetActive(false);
                }
            }
    }

    public int IndexByPosition(TSVector p)
    {
        return IndexByCoord((int)TSMath.Round(p.x),(int)TSMath.Round(p.z));
    }

    public int IndexByCoord(Coord c){
    	return IndexByCoord(c.x,c.y);
    }

    public int IndexByCoord(int x,int y){
    	return y*mapData.width+x;
    }

    public bool IsCoordInMap(int x,int y){
        if(x<0||y<0||x>mapData.width-1||y>mapData.height-1)return false;

        return true;
    }

    public bool IsTileCanWalk(int x,int y){
    	if(!IsCoordInMap(x,y))return false;

		var index = IndexByCoord(x,y);

		return staticTerrainSP[index]==TerrainType.Ground;
    }

    public bool IsTileCanBuild(int x,int y,bool isNaval,bool isMiner){
    	bool hasOre = false;
		var index = y*mapData.width+x;
        if(tempBuildMap[index]!=0)return false;
        if(fogMap[index]!=0)return false;
    	foreach(var obj in mapData.objs){
    		if(obj.x == x && obj.y == y ){
    			if(!Configs.mapConfig.mapObjectDatas[obj.id-1].buildable)
    				return false;
				if(isMiner && Configs.mapConfig.mapObjectDatas[obj.id-1].isOre)
					hasOre = true;
				
    		}
    	}

    	if(isMiner && !hasOre)return false;

		if(isNaval){
			if(staticTerrain[index] == TerrainType.Water){
				return true;
			}
		}
		else{
			if(staticTerrain[index] == TerrainType.Ground){
				return true;
			}
		}

		return false;
    }

    public void ExpulsionUnitInCoord(int x,int y){

    }

    public bool IsBuildBlocking(int x,int y){
        var t = staticTerrainSP[IndexByCoord(x,y)];
        BattleMap.ins.ChangeTileSP(x,y,TerrainType.Wall);
        SearchPathWalk();
        if(pathMapWalk[0]==999 || pathMapWalk[mapData.width-1]==999){
            BattleMap.ins.ChangeTileSP(x,y,t);
            SearchPathWalk();
            return true;
        }
        BattleMap.ins.ChangeTileSP(x,y,t);
        SearchPathWalk();

        return false;
    }

    public TerrainType GetStaticTerrainType(int x,int y){
        return staticTerrain[IndexByCoord(x,y)];
    }
}
