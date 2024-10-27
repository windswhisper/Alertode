using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateSkillShow : MonoBehaviour
{
	public Image imgHero;

    public void Play(string heroId)
    {
    	gameObject.SetActive(true);
    	imgHero.sprite = Resources.Load<Sprite>("image/hero/"+heroId+"_Card");
        BattleManager.ins.isPause2 = true;
    }

    public void End(){
        BattleManager.ins.isPause2 = false;
    	gameObject.SetActive(false);
    }

}
