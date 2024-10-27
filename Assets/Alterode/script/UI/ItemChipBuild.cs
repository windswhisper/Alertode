using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChipBuild : MonoBehaviour
{
	public ItemChip itemChip;
	public GameObject itemAdd;
	public GameObject itemLock;
	public BuildDetailPanel panel;
	public ChipDetailPanel chipDetailPanel;

	public void Init(BaseChip chip){
		itemChip.Init(chip);
		itemChip.gameObject.SetActive(true);
		itemAdd.SetActive(false);
		itemLock.SetActive(false);
	}

	public void Init(bool isUnlock){
		if(isUnlock){
			itemAdd.SetActive(true);
			itemLock.SetActive(false);
			itemChip.gameObject.SetActive(false);
		}
		else{
			itemAdd.SetActive(false);
			itemLock.SetActive(true);
			itemChip.gameObject.SetActive(false);
		}
	}

	public void ShowChipDetail(){
		chipDetailPanel.Show(itemChip.chip);
	}

}
