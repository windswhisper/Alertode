using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class WaveProgress : MonoBehaviour
{
	public Text txtWave;
	public RectTransform progressBar;
	public float barWidth = 1000;
    public BaseMonster boss;

    public Image imgBar;
    public Sprite spBarDay;
    public Sprite spBarNight;
    public Sprite spBarBoss;

    void Update()
    {
        if(BattleManager.ins.engine.isBossBattle){
                progressBar.localPosition = new Vector3(-barWidth+(float)boss.hp*barWidth/boss.GetMaxHp(),0,0);
        }
        else{
        	if(BattleManager.ins.engine.isDay){
        		progressBar.localPosition = new Vector3(-barWidth+(float)(BattleManager.ins.engine.time)*barWidth/Configs.commonConfig.dayTime,0,0);
        	}
        	else{
        		progressBar.localPosition = new Vector3(-(float)(BattleManager.ins.engine.time)*barWidth/BattleManager.ins.engine.nightTime,0,0);
        	}
        }
    }

    public void SwitchNight(){
        imgBar.sprite = spBarNight;
    }
    public void SwitchDay(){
        txtWave.text = string.Format(I18N.instance.getValue("@UI.Wave"),BattleManager.ins.engine.wave,BattleMap.ins.mapData.wave) ;
        if(BattleManager.ins.gameMode>=2)
        {
            txtWave.text = string.Format(I18N.instance.getValue("@UI.Wave2"),BattleManager.ins.engine.wave) ;
        }
        imgBar.sprite = spBarDay;
    }

    public void EnterBossBattle(BaseMonster monster){
        imgBar.sprite = spBarBoss;
        txtWave.text = "BOSS - " + I18N.instance.getValue(monster.uiName);
        boss = monster;
    }

}
