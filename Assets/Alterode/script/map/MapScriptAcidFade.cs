using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScriptAcidFade : BaseMapScript
{
	public int life = 3;

	override public void OnDayStart(){
		life--;
		if(life<0){
			BattleMap.ins.RemoveMapObj(x,y,37);
			
			isRemove = true;
		}
	}
}
