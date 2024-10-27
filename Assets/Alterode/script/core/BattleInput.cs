using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInput : MonoBehaviour
{
	public Transform cameraHandler;
	public Transform indicator;

	Vector3 dragDownPos;
	float doubleTouchDownDis;
	bool isDragingMap = false;
	bool isDragingFar = false;
	bool isChoosePos = false;
	bool isScaling = false; 
	Action<Coord> choosePosCallback;
	Action<Coord> choosePosHoverCallback;
	Action choosePosCancelCallback;

	void Update(){
		var scale = cameraHandler.localScale.x;

		if(Input.touchCount == 2){
			if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved){
				Touch touch1 = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);
				var curDis = Vector2.Distance(touch1.position, touch2.position);
				curDis = curDis * 1000 / Screen.width;
				if(!isScaling){
					isScaling = true;
					doubleTouchDownDis = curDis;
				}
				else{
					scale -= (curDis-doubleTouchDownDis)/400;
					if(scale<0.4f)scale = 0.4f;
					else if(scale>1.5f)scale = 1.5f;
					cameraHandler.localScale = new Vector3(scale,scale,scale);

					doubleTouchDownDis = curDis;
				}

				isDragingMap = false;
			}
		}
		else{

			if(Input.GetMouseButtonUp(0)){
				if(isDragingMap && !isDragingFar){
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				    RaycastHit[] hits = Physics .RaycastAll (ray,int.MaxValue,1<<LayerMask .NameToLayer ("RaycastReceiver"));
				    if(hits.Length>0){
					    var locate = hits[0].point;
						var coord = new Coord(locate.x,locate.z);
						if(Utils.IsCoordInMap(coord)){
							if(isChoosePos){
								isChoosePos = false;
								choosePosCallback(coord);
							}
							else{
								BattleManager.ins.SelectMapObject(coord.x,coord.y);
							}
						}
					}
				}
				isDragingMap = false;
			}
			if(isDragingMap){
				var p = Input.mousePosition;
				var d = p - dragDownPos;
				d = d * 1000 / Screen.width;

				var cameraPos = cameraHandler.localPosition;
				cameraPos+= new Vector3((d.x-d.y)/80,0,(d.y+d.x)/80)*scale;
				if(cameraPos.x<0)cameraPos.x = 0;
				if(cameraPos.x>BattleMap.ins.mapData.width)cameraPos.x = BattleMap.ins.mapData.width;
				if(cameraPos.z<0)cameraPos.z = 0;
				if(cameraPos.z>BattleMap.ins.mapData.height)cameraPos.z = BattleMap.ins.mapData.height;

				cameraHandler.localPosition = cameraPos;

				dragDownPos = p;
				if(d.x>2 || d.y>2 || d.x<-2 || d.y<-2){
					isDragingFar = true;
				}
			}	

			isScaling = false;
		}

		if(Input.GetMouseButtonUp(1)){
			BuildBar.ins.UnselectAll();
			BattleManager.ins.UnselectMapObject();
			if(isChoosePos){
				choosePosCancelCallback();
				isChoosePos = false;
			}
		}

		if(Input.GetAxis("Mouse ScrollWheel")!=0){
			scale -= Input.GetAxis("Mouse ScrollWheel")/2;
			if(scale<0.4f)scale = 0.4f;
			else if(scale>1.5f)scale = 1.5f;
			cameraHandler.localScale = new Vector3(scale,scale,scale);
		}

		if(Input.GetKeyUp(KeyCode.H)){
			SelectHero();
		}
	}

	public void OnHoverMap(){
		if(!isDragingMap){
			if(BuildBar.ins.selectId != "" || isChoosePos){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			    RaycastHit[] hits = Physics .RaycastAll (ray,int.MaxValue,1<<LayerMask .NameToLayer ("RaycastReceiver"));
			    if(hits.Length>0){
				    var locate = hits[0].point;
					var coord = new Coord(locate.x,locate.z);
					if(isChoosePos){
						choosePosHoverCallback(coord);
					}
					else{
						if(Utils.IsCoordInMap(coord)){
							indicator.gameObject.SetActive(true);
							if(BattleMap.ins.GetStaticTerrainType(coord.x,coord.y) == TerrainType.Water){
								indicator.localPosition = new Vector3(coord.x,-0.5f,coord.y);
							}
							else {
								indicator.localPosition = new Vector3(coord.x,0,coord.y);
							}
						}
						else{
							indicator.gameObject.SetActive(false);
						}
					}
				}
			}
			else{
				indicator.gameObject.SetActive(false);
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
		if(BuildBar.ins.selectId == "")
		{
			dragDownPos = Input.mousePosition;
			isDragingMap = true;
			isDragingFar = false;
		}	
		
	}
	public void OnClickUpMap(){
		if(BuildBar.ins.selectId != "")
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit[] hits = Physics .RaycastAll (ray,int.MaxValue,1<<LayerMask .NameToLayer ("RaycastReceiver"));
		    if(hits.Length>0){
			    var locate = hits[0].point;
				var coord = new Coord(locate.x,locate.z);

				if(Utils.IsCoordInMap(coord)){
					var build = BattleManager.ins.PutBuild(BuildBar.ins.selectId,coord.x,coord.y,BuildBar.ins.selectHero,false);
					if(build!=null){
						BuildBar.ins.UnselectAll();
						indicator.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	public void ChoosePos(Action<Coord> callback,Action<Coord> hoverCallback,Action cancelCallback){
		isChoosePos = true;
		choosePosCallback = callback;
		choosePosHoverCallback = hoverCallback;
		choosePosCancelCallback = cancelCallback;

		BuildBar.ins.UnselectAll();
		BattleManager.ins.UnselectMapObject();
	}

	public void CancelChoosePos(){
		isChoosePos = false;
	}

	public void SelectHero(){
        BattleManager.ins.SelectHero();
	}
}
