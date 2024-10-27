using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScriptShowMap : BaseMapScript
{
    public int wave = 1;

    override public void OnInit(){
        wave = intPara;
    }

    override public void OnDayStart(){
        if(BattleManager.ins.engine.wave>=wave){
            BattleMap.ins.HideFog();
        }
    }
}
