using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TechChipItem : MonoBehaviour
{
	public TechPanel techPanel;
	public string buildID;
	public string chipID;

	public Text txtName;
	public Text txtDesc;
	public Image iconBuild;
	public Image iconChip;
	public Animation anim;
	public GameObject btnReroll;
	public Text txtCost;
	public AudioSource soundSelect;
	public Image btnBuy;
	int cost;

	public void Init(string buildID,string chipID){
		this.buildID = buildID;
		this.chipID = chipID;

		var buildData = Configs.GetBuild(buildID);
		var chipData = Configs.GetChip(chipID);
		txtName.text = I18N.instance.getValue(chipData.uiName);
		if(chipData.prerequisite!="")txtName.text+=" +"+chipData.level;

		string[] paras = new string[chipData.paras.Count];
		int i=0;
		foreach(var para in chipData.paras){
			paras[i] = Convert.ToString(para);
			i++;
		}

		txtDesc.text = string.Format(I18N.instance.getValue("@UI.TechChipDesc"),I18N.instance.getValue(buildData.uiName),string.Format(new DescFormatter(),I18N.instance.getValue(chipData.desc),paras));

		iconBuild.sprite = Resources.Load<Sprite>("image/build/"+buildID);
		iconChip.sprite = Resources.Load<Sprite>("image/chip/"+chipData.image);

		int totalLevel = GetBuildTotalTechLevel(buildID);
		if(totalLevel>=Configs.commonConfig.chipTechPrice.Count){
			cost = Configs.commonConfig.chipTechPrice[Configs.commonConfig.chipTechPrice.Count-1];
		}
		else{
			cost = Configs.commonConfig.chipTechPrice[totalLevel];
		}

		txtCost.text = "$"+cost;

		anim.Play();
	}

	void Update(){		
		if(BattleManager.ins.engine.coin>=cost){
			btnBuy.color = (Color.white);
		}
		else{
			btnBuy.color = (new Color(0.7f,0.7f,0.7f,0.7f));
		}
	}

	public int GetBuildTotalTechLevel(string buildID){
		int level = 0;
		foreach(var techChip in BattleManager.ins.techChipList){
			if(techChip.buildID == buildID){
				var chipData = Configs.GetChip(techChip.chipID);
				level += chipData.level+1;
				Debug.Log(buildID+"/"+"chipID"+":"+level);
			}
		}
		return level;
	}

	public void Selected(){
		if(BattleManager.ins.engine.coin >= cost){
			BattleManager.ins.engine.coin -= cost;
			techPanel.ChooseChipOption(buildID,chipID);
			anim.Play("item_tech_selected");
			soundSelect.Play();
		}
		else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoCoin"));
		}
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
