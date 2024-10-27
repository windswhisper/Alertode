using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
	public GameObject itemGem;
	public GameObject itemMetal;
	public GameObject itemSilicon;
	public Text txtGem;
	public Text txtMetal;
	public Text txtSilicon;

	public void Init(List<GiftContentData> rewards){
    	foreach(var reward in rewards){
    		if(reward.name == "gem"){
    			itemGem.SetActive(true);
    			txtGem.text = "x"+reward.number;
    		}
    		else if(reward.name == "metal"){
    			itemMetal.SetActive(true);
    			txtMetal.text = "x"+reward.number;
    		}
    		else if(reward.name == "silicon"){
    			itemSilicon.SetActive(true);
    			txtSilicon.text = "x"+reward.number;
    		}
    	}
	}

	public void Close(){
		Destroy(gameObject);
	}
}
