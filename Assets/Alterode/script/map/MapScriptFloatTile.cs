using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScriptFloatTile : BaseMapScript
{
	List<int> statusQueue;

	public int curStatus = 1;

	override public void OnInit(){
		statusQueue = new List<int>();
		for(var i=0;i<namePara.Length;i++){
			statusQueue.Add(namePara[i]=='0'?0:1);
		}
	}

	override public void OnDayStart(){
		int index = (BattleManager.ins.engine.wave-1)%statusQueue.Count;

		int status = statusQueue[index];

		if(curStatus==status)return;

		curStatus = status;

		if(status == 0){
			BattleMap.ins.ChangeTileSP(x,y,TerrainType.Water);
			BattleMap.ins.SetFloatTileStatus(x,y,false);
			BattleMap.ins.ExpulsionUnitInCoord(x,y);
		}
		else{
			BattleMap.ins.ChangeTileSP(x,y,TerrainType.Ground);
			BattleMap.ins.SetFloatTileStatus(x,y,true);
		}
	}
}
