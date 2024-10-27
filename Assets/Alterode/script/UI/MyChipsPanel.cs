using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyChipsPanel : MonoBehaviour
{
	public RectTransform container;
	public GameObject itemChip;

	public ChipDetailPanel chipDetailPanel;

    public void Show()
    {
    	gameObject.SetActive(true);

    	for(var i=0;i<container.childCount;i++){
    		Destroy(container.GetChild(i).gameObject);
    	}
    	
        foreach(var chip in BattleManager.ins.engine.chipList){
        	var item = Instantiate(itemChip);
        	item.GetComponent<ItemChip>().Init(chip);
        	item.GetComponent<ItemMyChip>().Init(this);
        	item.transform.SetParent(container,false);
        }

		container.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, (BattleManager.ins.engine.chipList.Count-1) / 3 *180 + 180);
    }

    public void ShowChipDetail(BaseChip chip){
		chipDetailPanel.Show(chip);
    }

    public void Hide(){
    	gameObject.SetActive(false);
    	chipDetailPanel.gameObject.SetActive(false);
    }
}
