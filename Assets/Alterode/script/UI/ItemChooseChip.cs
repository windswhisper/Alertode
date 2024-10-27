using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChooseChip : MonoBehaviour
{
	public ItemChip itemChip;
	ChooseChipPanel chooseChipPanel;

	public void Init(ChooseChipPanel chooseChipPanel){
		this.chooseChipPanel = chooseChipPanel;
	}

	public void Click(){
		chooseChipPanel.ChooseChip(itemChip.chip);
	}
}
