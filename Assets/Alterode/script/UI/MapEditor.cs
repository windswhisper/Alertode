using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
	public static MapEditor ins;

	public RectTransform itemContainer;
	public GameObject deleteFrame;
	public GameObject itemPrefab;
	public InputField inputWidth; 
	public InputField inputHeight; 
	public InputField inputMapData; 
	public GameObject gridLinePrefab;
	public Transform gridLayer;
	public Transform cameraHandler;
	public Transform indicator;
	public Transform tileLayer;
	public Image btnTab;
	public List<Sprite> tabButtonSp;

	MapData mapData;
	List<GameObject> mapTiles;
	List<GameObject> mapObjects;

	List<ItemMap> itemList = new List<ItemMap>();
	int tabIndex = 0;
	int tileId = -1;

	Vector3 dragDownPos;
	bool isDragingMap = false;
	bool isDragingPaint = false;

	void Awake(){
		ins = this;
	}

	void Start(){
		RefreshList();
		NewMap();
	}

	void Update(){
		var scale = cameraHandler.localScale.x;

		if(Input.GetMouseButtonUp(0)){
			isDragingMap = false;
			isDragingPaint = false;
		}
		if(isDragingMap){
			var p = Input.mousePosition;
			var d = p - dragDownPos;

			d = d * 1000 / Screen.width;
			cameraHandler.localPosition += new Vector3((d.x-d.y)/80,0,(d.y+d.x)/80)*scale;
			dragDownPos = p;
		}

		if(Input.GetMouseButtonUp(1))UnselectAll();

		if(Input.GetAxis("Mouse ScrollWheel")!=0){
			scale -= Input.GetAxis("Mouse ScrollWheel")/2;
			if(scale<0.4f)scale = 0.4f;
			else if(scale>1.5f)scale = 1.5f;
			cameraHandler.localScale = new Vector3(scale,scale,scale);
		}
	}

	public void ChangeTab(){
		tabIndex = (tabIndex+1)%3;

		btnTab.sprite = tabButtonSp[tabIndex];

		RefreshList();
	}

	public void NewMap(){
		var w =  Convert.ToInt32(inputWidth.text);
		var h =  Convert.ToInt32(inputHeight.text);
		if(w>99)w=99;
		if(h>99)h=99;
		mapData = new MapData(w,h);
		InitMap();
	}

	public void LoadMap(){
		var json = inputMapData.text;

		mapData = JsonUtility.FromJson<MapData>(json);

		InitMap();
	}

	public void InitMap(){
		var w = mapData.width;
		var h = mapData.height;

		for(var i=0;i<gridLayer.childCount;i++)
		{
			Destroy(gridLayer.GetChild(i).gameObject);
		}	

		for(var i=0;i<tileLayer.childCount;i++)
		{
			Destroy(tileLayer.GetChild(i).gameObject);
		}	

		this.mapTiles = new List<GameObject>();
		this.mapObjects = new List<GameObject>();

		for(var i=0;i<w*h;i++){
			mapTiles.Add(null);
			mapObjects.Add(null);
		}

		// for(var i=0;i<=w;i++){
		// 	var line = Instantiate(gridLinePrefab);
		// 	line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(i,0,0));
		// 	line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(i,0,h));
		// 	line.transform.SetParent(gridLayer,false);
		// }

		// for(var i=0;i<=w;i++){
		// 	var line = Instantiate(gridLinePrefab);
		// 	line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0,0,i));
		// 	line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(w,0,i));
		// 	line.transform.SetParent(gridLayer,false);
		// }

		var frame1 = Instantiate(gridLinePrefab);
		frame1.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0,0,0));
		frame1.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0,0,h));
		frame1.transform.SetParent(gridLayer,false);

		var frame2 = Instantiate(gridLinePrefab);
		frame2.GetComponent<LineRenderer>().SetPosition(0, new Vector3(w,0,0));
		frame2.GetComponent<LineRenderer>().SetPosition(1, new Vector3(w,0,h));
		frame2.transform.SetParent(gridLayer,false);

		var frame3 = Instantiate(gridLinePrefab);
		frame3.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0,0,0));
		frame3.GetComponent<LineRenderer>().SetPosition(1, new Vector3(w,0,0));
		frame3.transform.SetParent(gridLayer,false);

		var frame4 = Instantiate(gridLinePrefab);
		frame4.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0,0,h));
		frame4.GetComponent<LineRenderer>().SetPosition(1, new Vector3(w,0,h));
		frame4.transform.SetParent(gridLayer,false);

		cameraHandler.localPosition = new Vector3(w/2,0,h/2);

		for(var i=0;i<w;i++){
			for(var j=0;j<h;j++){
				if(mapData.map[j*w+i]!=0){
					putTile(i,j,mapData.map[j*w+i],true);
				}
			}
		}

		foreach(var obj in mapData.objs){
			putObject(obj.x,obj.y,obj.id,true);
		}
	}

	public void RefreshList(){
		for(var i=0;i<itemContainer.childCount;i++)
		{
			Destroy(itemContainer.GetChild(i).gameObject);
		}	
		itemList.Clear();

		if(tabIndex == 0){
			for(var i=0;i<Configs.mapConfig.tileDatas.Count;i++)
			{
				var item = Instantiate(itemPrefab);
				item.transform.SetParent(itemContainer,false);
				itemList.Add(item.GetComponent<ItemMap>());
				itemList[i].Init(tabIndex,i+1);
			}
			itemContainer.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Configs.mapConfig.tileDatas.Count * 192 + 180);
		}
		else if(tabIndex == 1){
			for(var i=0;i<Configs.mapConfig.mapObjectDatas.Count;i++)
			{
				var item = Instantiate(itemPrefab);
				item.transform.SetParent(itemContainer,false);
				itemList.Add(item.GetComponent<ItemMap>());
				itemList[i].Init(tabIndex,i+1);
			}
			itemContainer.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Configs.mapConfig.mapObjectDatas.Count * 192 + 180);
		}
	}

	public void SelectItem(int id){
		if(tileId == id){
			UnselectAll();
			return;
		}

		tileId = id;

		foreach(var item in itemList){
			if(item.id==id){
				item.Selected();
			}
			else{
				item.CancelSelected();
			}
		}

		deleteFrame.SetActive(false);
	}

	public void SelectDelete(){
		if(tileId == 0){
			UnselectAll();
			return;
		}

		tileId = 0;

		foreach(var item in itemList){
			item.CancelSelected();
		}
		deleteFrame.SetActive(true);
	}

	public void UnselectAll(){
		tileId = -1;

		foreach(var item in itemList){
			item.CancelSelected();
		}
		deleteFrame.SetActive(false);
	}

	public void OnHoverMap(){
		if(!isDragingMap){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit[] hits = Physics .RaycastAll (ray,int.MaxValue,1<<LayerMask .NameToLayer ("RaycastReceiver"));
		    if(hits.Length>0){
			    var locate = hits[0].point;
				var coord = new Coord(locate.x,locate.z);
				if(Utils.IsCoordInMap(coord,mapData.width,mapData.height)){
					indicator.gameObject.SetActive(true);
					indicator.localPosition = new Vector3(coord.x,0,coord.y);

					if(isDragingPaint){
						if(tileId != -1)
						{
							if(Utils.IsCoordInMap(coord,mapData.width,mapData.height)){
								put(coord.x,coord.y);
							}
						}
					}
				}
				else{
					indicator.gameObject.SetActive(false);
				}
			}
		}
		else{
			indicator.gameObject.SetActive(false);
		}
	}

	public void OnLeaveMap(){
		indicator.gameObject.SetActive(false);
	}

	public void OnClickDownMap(){
		if(tileId == -1)
		{
			dragDownPos = Input.mousePosition;
			isDragingMap = true;
		}	
		else{
			isDragingPaint = true;
		}
		
	}
	public void OnClickUpMap(){

		if(tileId != -1)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit[] hits = Physics .RaycastAll (ray,int.MaxValue,1<<LayerMask .NameToLayer ("RaycastReceiver"));
		    if(hits.Length>0){
			    var locate = hits[0].point;
				var coord = new Coord(locate.x,locate.z);

				if(Utils.IsCoordInMap(coord,mapData.width,mapData.height)){
					put(coord.x,coord.y);
				}
			}
		}
	}

	public void put(int x,int y){
		if(tabIndex == 0){
			putTile(x,y);
		}
		else if(tabIndex == 1){
			putObject(x,y);
		}
	}

	public void putTile(int x,int y){
		putTile(x,y,tileId,false);
	}

	public void putTile(int x,int y,int id,bool force)
	{
		var index = y*mapData.width+x;
		if(mapData.map[index]!=id || force){
			Destroy(mapTiles[index]);
			mapData.map[index]=id;
			if(id != 0){
				var tile = Instantiate(Resources.Load<GameObject>(string.Format("prefab/map/mapTile{0:D3}",id)));
				tile.transform.SetParent(tileLayer,false);
				tile.transform.localPosition = new Vector3(x,-0.5f,y);
				mapTiles[index] = tile;
			}
			else{
				mapTiles[index] = null;
			}
		}
	}
	public void putObject(int x,int y){
		putObject(x,y,tileId,false);
	}

	public void putObject(int x,int y,int id,bool force){
		var index = y*mapData.width+x;
		bool isExist = false;
		foreach(var obj in mapData.objs){
			if(obj.x == x && obj.y == y){
				if(obj.id == id){
					isExist = true;
				}
				else if(!force){
					mapData.objs.Remove(obj);
				}
				break;
			} 
		}
		if(!isExist || force){
			Destroy(mapObjects[index]);
			if(!force && id != 0)mapData.objs.Add(new MapObjectID(x,y,id));
			if(id != 0){
				var tile = Instantiate(Resources.Load<GameObject>(string.Format("prefab/map/mapObj{0:D3}",id)));
				tile.transform.SetParent(tileLayer,false);
				tile.transform.localPosition = new Vector3(x,0,y);
				mapObjects[index] = tile;
			}
			else{
				mapObjects[index] = null;
			}
		}

	}

	public void SaveMap(){
		var json = JsonUtility.ToJson(mapData);
		Debug.Log(json);
	}
	public void SaveMapBlock(){
		MapBlockData data = new MapBlockData();
		data.tiles = mapData.map;
		data.objs = mapData.objs;
		var json = JsonUtility.ToJson(data);
		Debug.Log(json);
	}
}
