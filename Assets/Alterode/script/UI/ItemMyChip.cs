using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMyChip : MonoBehaviour
{
	public ItemChip itemChip;
	MyChipsPanel panel;

	public void Init(MyChipsPanel panel){
		this.panel = panel;
	}

	public void Click(){
		panel.ShowChipDetail(itemChip.chip);
	}
}
