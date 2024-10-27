using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ItemChipTeam : MonoBehaviour
{
	public ChipData chipData;
	public ItemChip itemChip;
	public GameObject itemAdd;
	public GameObject itemLock;
	public Text txtCost;

	int index;
	public TeamPanel teamPanel;
	public ChangeTeamChipPanel changeTeamChipPanel;

	public void Init(BuildData buildData,int index){
		this.index = index;

		chipData = Configs.GetChip(buildData.defaultChips[index*2+(teamPanel.itemSelected.data.GetChipSlot(index)-1)]);

		itemChip.Init(chipData);
		itemChip.gameObject.SetActive(true);
		itemAdd.SetActive(false);
		itemLock.SetActive(false);
	}

	public void Init(bool isUnlock,int index){
		this.index = index;

		if(isUnlock){
			itemAdd.SetActive(true);
			itemLock.SetActive(false);
			itemChip.gameObject.SetActive(false);

			txtCost.text = Configs.commonConfig.chipSlotPrice[index]+"";
		}
		else{
			itemAdd.SetActive(false);
			itemLock.SetActive(true);
			itemChip.gameObject.SetActive(false);
		}
	}

	public void ChangeChip(){
		changeTeamChipPanel.Show(index);
	}
}
