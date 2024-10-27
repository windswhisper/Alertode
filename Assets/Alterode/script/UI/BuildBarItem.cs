using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBarItem : MonoBehaviour
{
	public string buildId;
	bool isSelected = false;
	bool isHero = false;
	public bool isInCd = false;

	public Image icon;
	public Text txtCost;
	public GameObject selectFrame;
	public GameObject flash;
	public Image cdMask;
	float buildCd = 0;

	public void Init(string buildId){
		this.buildId = buildId;

		icon.sprite = Resources.Load<Sprite> ("image/build/"+buildId);
		RefreshCost();
	}

	public void InitHero(string heroId){
		this.buildId = heroId;
		isHero = true;
		
		icon.sprite = Resources.Load<Sprite> ("image/hero/"+heroId);
		RefreshCost();
	}

	public void RefreshCost(){
		if(isHero){
			int cost = Configs.GetHero(buildId).cost;
			if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;
			if(cost<0)cost = 0;
			txtCost.text = "$"+Convert.ToString(cost);
		}
		else{
			int cost = Configs.GetBuild(buildId).cost;
			if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;

	        var costMultipleFactor = 0;
	        foreach(var techChip in BattleManager.ins.techChipList){
	            if(techChip.buildID == buildId){
	                costMultipleFactor += techChip.chipStatic.OnGetSelfCostMutipleFactorGlobal();
	            }
	            costMultipleFactor += techChip.chipStatic.OnGetCostMutipleFactorGlobal();
	        }

	        cost = cost*(100+costMultipleFactor)/100;

			if(cost<0)cost = 0;
			txtCost.text = "$"+Convert.ToString(cost);
		}
	}

	public void OnHoverIn(){
		BuildBar.ins.ShowInfo(transform.position,buildId,isHero);
	}
	public void OnHoverOut(){
		BuildBar.ins.HideInfo();
	}

	public void OnClick(){
		BuildBar.ins.SelectItem(buildId,isHero);
	}

	public void Selected(){
		isSelected = true;
		selectFrame.SetActive(true);
	}
	public void CancelSelected(){
		isSelected = false;
		selectFrame.SetActive(false);
	}

	public void EnterCd(){
		buildCd = 10;
		isInCd = true;
		cdMask.gameObject.SetActive(true);
		flash.gameObject.SetActive(false);
		RefreshCost();
	}

	void Update(){
		if(buildCd>0){
			buildCd -= Time.deltaTime * BattleManager.ins.GetGameSpeed();
			cdMask.fillAmount = buildCd/10;
			if(buildCd<=0){
				cdMask.gameObject.SetActive(false);
				isInCd = false;
			}
		}
	}
}
