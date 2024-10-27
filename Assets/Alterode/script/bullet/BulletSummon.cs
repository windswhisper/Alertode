using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSummon : BulletInviso
{
	public string monsterName;

	public override void Miss(){
		if(isMissed)return;
		var coord = new Coord(position.x,position.z);
		BattleManager.ins.PutMonster(monsterName,coord.x,coord.y,false);
		
    	if(view!=null)view.Explodes();
		base.Miss();
    }
}
