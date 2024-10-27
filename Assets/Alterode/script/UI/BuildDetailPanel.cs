using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;
using TrueSync;

public class BuildDetailPanel : MonoBehaviour
{
	public Text txtLevel;
	public Text txtName;
	public Image icon;
	public Text txtHp;
	public Text txtDamage;
	public Text txtRange;
	public Text txtMoneyRate;
	public GameObject infoMoneyRate;
	public GameObject infoAtk;
	public Text txtCost;
	public Text txtCostAll;
	public ItemChipBuild[] itemChips;
	public GameObject btnUpgrade;
	public GameObject btnUpgradeAll;
	public GameObject upgradeMax;
	public Text txtTargetPrefer;
	public GameObject btnSell;
	public GameObject targetPrefer;
	public GameObject btnMove;
	public GameObject expInfo;
	public GameObject occupyInfo;
	public Transform utmSkillRoot;
	public GameObject utmSkillBtnPrefab;
	public Image expBar;
	public Image occupyBar;
	public Text txtExp;
	public Text txtOccupyProgress;

	public ChooseChipPanel chooseChipPanel;
	public ChipDetailPanel chipDetailPanel;
	public HeroMovePanel movePanel;
	public CastSkillPanel castSkillPanel;

	BaseBuild build;
	string[] targetPreferStrings = {"@UI.TargetPreferClose","@UI.TargetPreferStrong","@UI.TargetPreferWeak","@UI.TargetPreferFar"};
	string[] rotatePreferStrings = {"@UI.RotatePreferDL","@UI.RotatePreferUL","@UI.RotatePreferUR","@UI.RotatePreferDR"};

	List<UltimateSkillButton> utmSKillBtns = new List<UltimateSkillButton>();
	int oldLevel;

	void Update(){
		txtHp.text = build.hp + "/" + build.GetMaxHp();
		if(build==null || build.isDead)Hide();

		if(build.level!=oldLevel){
			UpdateInfo();
		}
		else{
			UpdateNumberInfo();
		}

		if(build.isHero && Input.GetKeyUp(KeyCode.M)){
			ChooseMovePos();
		}
		if(Input.GetKeyUp(KeyCode.Delete) || Input.GetKeyUp(KeyCode.Backspace) ){
			Sell();
		}
		if(Input.GetKeyUp(KeyCode.Q) && utmSKillBtns.Count>0){
			utmSKillBtns[0].Click();
		}
		if(Input.GetKeyUp(KeyCode.W) && utmSKillBtns.Count>1){
			utmSKillBtns[1].Click();
		}
		if(Input.GetKeyUp(KeyCode.E) && utmSKillBtns.Count>2){
			utmSKillBtns[2].Click();
		}
		if(Input.GetKeyUp(KeyCode.R) && utmSKillBtns.Count>3){
			utmSKillBtns[3].Click();
		}

		if(!build.isHero && build.level < Configs.commonConfig.buildMaxLevel && build.data.name!="Obelisk"){
			if(Input.GetKey(KeyCode.LeftShift)){
				btnUpgrade.SetActive(false);
				btnUpgradeAll.SetActive(true);
			}
			else{
				btnUpgrade.SetActive(true);
				btnUpgradeAll.SetActive(false);
			}
		}

	}

	public void UpdateChipList(){
		var index = 0;
		foreach(var chip in build.chips){
			itemChips[index].Init(chip);
			index++;
		}

		if(BattleManager.ins.gameMode == -1){
			for(var i=index;i<3;i++){
				itemChips[i].Init(build.level >= Configs.commonConfig.buildEvoLevel[i]);
			}
		}
		else{
			for(var i=index;i<3;i++){
				itemChips[i].Init(i<build.chips.Count);
			}
		}
	}

	public void Upgrade(){
		BattleManager.ins.UpgradeBuild(build);
	}

	public void UpgradeAll(){
		BattleManager.ins.UpgradeBuilds(build);
	}

	public void ShowAddChipPanel(){
		chooseChipPanel.Show();
	}

	public void AddChip(BaseChip chip){
		if(!Utils.IsChipMatchBuild(build.data,chip.data)){
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.ChipNotMatch"));
			return;
		}

		BattleManager.ins.engine.EquipChip(build,chip);

		UpdateInfo();
	}

	public void Sell(){
		if(build.isHero)return;
		build.Sell();
		Hide();
	}

	public void ChooseMovePos(){
		Hide();
		movePanel.Show((BaseHero)build,this);
	}

	public void ChooseSkillPos(BaseUltimateSkill skill){
		Hide();
		castSkillPanel.Show(skill,this);
	}

	public void Hide(){
		gameObject.SetActive(false);
		chooseChipPanel.Hide();
		chipDetailPanel.Hide();
	}

	public void Show(BaseBuild build){
		this.build = build;

		gameObject.SetActive(true);

		UpdateInfo();
	}

	public void PreTargetPrefer(){

		switch(build.data.settingType){
			case 0:
				build.targetPrefer = (TargetPrefer)(((int)build.targetPrefer+targetPreferStrings.Length-1)%targetPreferStrings.Length);
				break;
			case 1:
				build.turretRotation = (build.turretRotation+TSMath.Pi/2)%(TSMath.Pi*2);
				break;
		}

		if(Input.GetKey(KeyCode.LeftShift)){
			foreach(var u in BattleManager.ins.engine.unitList){
				if(u.type == UnitType.Building && u.team == TeamType.Self && ((BaseBuild)u).name == build.name){
					switch(build.data.settingType){
						case 0:
							((BaseBuild)u).targetPrefer = build.targetPrefer;
							break;
						case 1:
							((BaseBuild)u).turretRotation = build.turretRotation;
							break;
					}
				}
			}
		}

		UpdateInfo();
	}

	public void NextTargetPrefer(){
		switch(build.data.settingType){
			case 0:
				build.targetPrefer = (TargetPrefer)(((int)build.targetPrefer+targetPreferStrings.Length+1)%targetPreferStrings.Length);
				break;
			case 1:
				build.turretRotation = (build.turretRotation-TSMath.Pi/2)%(TSMath.Pi*2);
				break;
		}

		if(Input.GetKey(KeyCode.LeftShift)){
			foreach(var u in BattleManager.ins.engine.unitList){
				if(u.type == UnitType.Building && u.team == TeamType.Self && ((BaseBuild)u).name == build.name){
					switch(build.data.settingType){
						case 0:
							((BaseBuild)u).targetPrefer = build.targetPrefer;
							break;
						case 1:
							((BaseBuild)u).turretRotation = build.turretRotation;
							break;
					}
				}
			}
		}

		UpdateInfo();
	}

	public void UpdateInfo(){
		txtName.text = I18N.instance.getValue(build.data.uiName);
		txtLevel.text = build.level+"";
		if(build.isHero){
			icon.sprite = Resources.Load<Sprite> ("image/hero/"+build.data.name);
			btnSell.SetActive(false);
			btnUpgrade.SetActive(false);
			upgradeMax.SetActive(false);
			occupyInfo.SetActive(false);

			btnMove.SetActive(true);
    		if(BattleManager.ins.battleModifier.isPBMode || BattleManager.ins.battleModifier.isTDMode)btnMove.SetActive(false);
			expInfo.SetActive(true);

			if(build.level<Configs.commonConfig.heroMaxLevel){
				txtExp.text = ((BaseHero)build).exp+"/"+Configs.commonConfig.upgradeExp[build.level]*build.data.cost/100;
				expBar.fillAmount = ((BaseHero)build).exp*1f/(Configs.commonConfig.upgradeExp[build.level]*build.data.cost/100);
			}
			else{
				txtExp.text = I18N.instance.getValue("@UI.UpgradeMax");
				expBar.fillAmount = 1;
			}
		}
		else{
			icon.sprite = Resources.Load<Sprite> ("image/build/"+build.data.name);

			if(build.data.name == "Base" || build.data.name == "Obelisk"){
				btnSell.SetActive(false);
			}
			else{
				btnSell.SetActive(true);
			}

			btnMove.SetActive(false);
			expInfo.SetActive(false);

			if(build.data.name == "Obelisk"){
				btnUpgrade.SetActive(false);
				btnUpgradeAll.SetActive(false);
				upgradeMax.SetActive(false);

				occupyInfo.SetActive(true);
				occupyBar.fillAmount = ((BuildObelisk)build).progress*1f/100;
				txtOccupyProgress.text = ((BuildObelisk)build).progress+"%";
			}
			else{
				occupyInfo.SetActive(false);
				if(build.level < Configs.commonConfig.buildMaxLevel){
					int cost = build.data.cost*Configs.commonConfig.upgradeCostRate[build.level]/100;
		        	if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;

			        var costMultipleFactor = 0;
			        foreach(var techChip in BattleManager.ins.techChipList){
			            if(techChip.buildID == build.data.name){
			                costMultipleFactor += techChip.chipStatic.OnGetSelfCostMutipleFactorGlobal();
			            }
			            costMultipleFactor += techChip.chipStatic.OnGetCostMutipleFactorGlobal();
			        }

			        cost = cost*(100+costMultipleFactor)/100;

			        if(cost<0)cost = 0;

			        int count = 0;
			        foreach(var unit in BattleManager.ins.engine.unitList){
			            if(unit.team == TeamType.Self && unit.type == UnitType.Building && ((BaseBuild)unit).data.name == build.data.name && ((BaseBuild)unit).level == build.level){
			                count++;
			            }
			        }
			        
					txtCost.text = Convert.ToString(cost);
					txtCostAll.text = Convert.ToString(cost*count);
					btnUpgrade.SetActive(true);
					upgradeMax.SetActive(false);
				}
				else{
					btnUpgrade.SetActive(false);
					btnUpgradeAll.SetActive(false);
					upgradeMax.SetActive(true);
				}
			}
		}
		txtHp.text = build.hp + "/" + build.GetMaxHp();
		if(build.weapons.Count!=0){
			txtDamage.text = string.Format("{0}×{1:F1}",build.weapons[0].GetDamage(),(float)build.weapons[0].GetFireSpeed());
			var range = (float)build.weapons[0].GetRange();
			txtRange.text =  string.Format("{0:F1}",range);

            if(range<0.6f)
            {
                BattleManager.ins.selectorRange.localScale = new Vector3(0,0,0);
            }
            else{
            	BattleManager.ins.selectorRange.localScale = new Vector3(range,range,range);
            }
		}
		else{
			txtDamage.text = "-";
			txtRange.text = "-";
            BattleManager.ins.selectorRange.localScale = new Vector3(0,0,0);
		}


		if(build.data.settingType==0 && build.data.weapons.Count == 0){
			targetPrefer.SetActive(false);
		}
		else if(build.data.settingType == -1){
			targetPrefer.SetActive(false);
		}
		else{
			targetPrefer.SetActive(true);

			switch(build.data.settingType){
				case 0:
					txtTargetPrefer.text = I18N.instance.getValue(targetPreferStrings[(int)build.targetPrefer]);
					break;
				case 1:
					txtTargetPrefer.text = I18N.instance.getValue(rotatePreferStrings[(int)(TSMath.Round(build.turretRotation/TSMath.Pi*2+4))%4]);
					break;
			}

		}

		UpdateChipList();
		UpdateUltimateSkills();

		oldLevel = build.level;
	}

	public void UpdateUltimateSkills(){
		for(var i=0;i<utmSkillRoot.childCount;i++){
			Destroy(utmSkillRoot.GetChild(i).gameObject);
		}
		utmSKillBtns.Clear();

		foreach(var utmSkill in build.utmSkills){
			var btn = Instantiate(utmSkillBtnPrefab);
			btn.GetComponent<UltimateSkillButton>().Init(this,utmSkill);
			btn.transform.SetParent(utmSkillRoot,false);
			utmSKillBtns.Add(btn.GetComponent<UltimateSkillButton>());
		}
	}

	public void UpdateNumberInfo(){
		txtLevel.text = build.level+"";
		txtHp.text = build.hp + "/" + build.GetMaxHp();

		if(build.weapons.Count!=0){
			txtDamage.text = string.Format("{0}×{1:F1}",build.weapons[0].GetDamage(),(float)build.weapons[0].GetFireSpeed());
			var range = (float)build.weapons[0].GetRange();
			txtRange.text =  string.Format("{0:F1}",range);
            if(range<0.6f)
            {
                BattleManager.ins.selectorRange.localScale = new Vector3(0,0,0);
            }
            else{
            	BattleManager.ins.selectorRange.localScale = new Vector3(range,range,range);
            }

			infoAtk.SetActive(true);
			infoMoneyRate.SetActive(false);
		}
		else{
			txtDamage.text = "-";
			txtRange.text = "-";
            BattleManager.ins.selectorRange.localScale = new Vector3(0,0,0);

			if(build.data.name == "Miner"){
				infoAtk.SetActive(false);
				infoMoneyRate.SetActive(true);
				txtMoneyRate.text = "$"+((BuildMiner)build).GetProductAmount()+"/"+build.data.paras[0]+"s";
			}
			else{
				infoAtk.SetActive(true);
				infoMoneyRate.SetActive(false);
			}

		}
		
		if(build.isHero){
			txtExp.text = ((BaseHero)build).exp+"/"+Configs.commonConfig.upgradeExp[build.level]*build.data.cost/100;
			expBar.fillAmount = ((BaseHero)build).exp*1f/(Configs.commonConfig.upgradeExp[build.level]*build.data.cost/100);
		}

		if( build.data.name=="Obelisk"){
			txtOccupyProgress.text = ((BuildObelisk)build).progress+"%";
			occupyBar.fillAmount = ((BuildObelisk)build).progress*1f/100;
		}
	}

	public void ShowOccupyHint(){
		ToastBar.ShowMsg(I18N.instance.getValue("@UI.MoveHeroNearBy"));
	}
}
