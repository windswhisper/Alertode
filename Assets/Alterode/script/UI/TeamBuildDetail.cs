using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TeamBuildDetail : MonoBehaviour
{
	public Image icon;
	public Text txtName;
	public Text txtHp;
	public Text txtDamage;
	public Text txtRange;
	public Text txtDesc;
	public Text txtMoneyRate;
	public GameObject infoMoneyRate;
	public GameObject infoAtk;
	public ItemChipTeam[] itemChips;
	public GameObject btnUse;
	public GameObject btnRemove;
	public GameObject tagLocked;
	public TeamPanel teamPanel;

	public ItemTeam itemSelected;

	BuildData buildData;

	public void SelectItem(ItemTeam itemSelected){
		this.itemSelected = itemSelected;

		if(itemSelected.isLocked){
			buildData = itemSelected.config;
			icon.sprite = Resources.Load<Sprite> ("image/build/"+buildData.name);
		}
		else{
			buildData = Configs.GetBuild(itemSelected.data.buildId);
			icon.sprite = Resources.Load<Sprite> ("image/build/"+itemSelected.data.buildId);
		}

		if(buildData.name == "Miner"){
			infoAtk.SetActive(false);
			infoMoneyRate.SetActive(true);
			txtMoneyRate.text = "$"+buildData.paras[1]+"/"+buildData.paras[0]+"s";
		}
		else{
			infoAtk.SetActive(true);
			infoMoneyRate.SetActive(false);
		}


		txtName.text = I18N.instance.getValue(buildData.uiName);
		txtDesc.text = I18N.instance.getValue(buildData.uiName+"_Desc");

		txtHp.text = buildData.strength+"";

		if(buildData.weapons.Count!=0){
			var weaponData = Configs.GetWeapon(buildData.weapons[0]);
			txtDamage.text = string.Format("{0}×{1:F1}",weaponData.damage,weaponData.fireSpeed/100f);
			txtRange.text =  string.Format("{0:F1}",weaponData.range/100f);
		}
		else{
			txtDamage.text = "-";
			txtRange.text = "-";
		}

		if(itemSelected.isLocked){
			btnUse.SetActive(false);
			btnRemove.SetActive(false);
			tagLocked.SetActive(true);
		}
		else{
			tagLocked.SetActive(false);
			if(itemSelected.data.isUsing){
				btnUse.SetActive(false);
				btnRemove.SetActive(true);
			}
			else{
				btnUse.SetActive(true);
				btnRemove.SetActive(false);
			}
		}

		RefreshChip();
	}

	public void RefreshChip(){
		if(!itemSelected.isLocked){
			for(var i=0;i<3;i++){
				if(itemSelected.data.chipSlotCount < i){
					itemChips[i].Init(false,i);
				}
				else if(itemSelected.data.chipSlotCount == i){
					itemChips[i].Init(true,i);
				}
				else{
					itemChips[i].Init(buildData,i);
				}
			}
		}
		else{
			for(var i=0;i<3;i++){
				itemChips[i].Init(false,i);
			}
		}
	}

	public void UseBuild(){
		if(teamPanel.UseBuild(itemSelected)){
			btnUse.SetActive(false);
			btnRemove.SetActive(true);
		}
	}

	public void RemoveBuild(){
		teamPanel.RemoveBuild(itemSelected);

		btnUse.SetActive(true);
		btnRemove.SetActive(false);
	}

}
