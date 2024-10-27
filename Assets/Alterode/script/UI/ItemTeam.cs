using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTeam : MonoBehaviour
{
	TeamPanel teamPanel;
	public TeamBuildData data;
	public BuildData config;

	public Image icon;
	public GameObject tagUsing;
	public GameObject tagSelect;
	public bool isOnTeam;
	public bool isLocked;
	public Text txtCost;
	public Text txtChipLevel;
	public GameObject hintNew;

	public void Init(TeamPanel teamPanel,TeamBuildData data,bool isOnTeam){
		this.teamPanel = teamPanel;
		this.data = data;
		this.isOnTeam = isOnTeam;
		isLocked = false;

		icon.sprite = Resources.Load<Sprite> ("image/build/"+data.buildId);
		txtCost.text = "$"+Configs.GetBuild(data.buildId).cost;

		if(GlobalData.newBuildHint.Contains(data.buildId)){
			hintNew.SetActive(true);
		}

		Refresh();
	}

	public void Init(TeamPanel teamPanel,BuildData config){
		this.teamPanel = teamPanel;
		this.config = config;
		isLocked = true;

		icon.sprite = Resources.Load<Sprite> ("image/build/"+config.name);
		txtCost.text = "$"+config.cost;
		icon.color = new Color(0.8f,0.8f,0.8f,0.6f);
		GetComponent<Image>().color = new Color(0.7f,0.7f,0.7f);

		Refresh();
	}

	public void Refresh(){
		if(!isLocked){
			tagUsing.SetActive(data.isUsing && !isOnTeam);
			txtChipLevel.text = ""+data.chipSlotCount;
			txtChipLevel.transform.parent.gameObject.SetActive(data.chipSlotCount != 0);
		}
		tagSelect.SetActive(teamPanel.itemSelected == this);
	}

	public void Click(){
		teamPanel.SelectItem(this);

		GlobalData.newBuildHint.Remove(data.buildId);
		hintNew.SetActive(false);

	}
}
