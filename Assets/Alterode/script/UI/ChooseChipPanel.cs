using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChipPanel : MonoBehaviour
{
	public RectTransform container;
	public GameObject itemChip;

	public BuildDetailPanel buildDetailPanel;

    public void Show()
    {
    	gameObject.SetActive(true);

    	for(var i=0;i<container.childCount;i++){
    		Destroy(container.GetChild(i).gameObject);
    	}
    	
        foreach(var chip in BattleManager.ins.engine.chipList){
        	var item = Instantiate(itemChip);
        	item.GetComponent<ItemChip>().Init(chip);
        	item.GetComponent<ItemChooseChip>().Init(this);
        	item.transform.SetParent(container,false);
        }

		container.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, (BattleManager.ins.engine.chipList.Count-1) / 3 *180 + 180);
    }

    public void ChooseChip(BaseChip chip){
    	buildDetailPanel.AddChip(chip);
    	Hide();
    }

    public void Hide(){
    	gameObject.SetActive(false);
    }
}
