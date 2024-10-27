using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHero : MonoBehaviour
{
	public HeroPanel heroPanel;
	public Image icon;
	public GameObject tagSelected;
	public GameObject tagUsing;
	public GameObject hintNew;


	string heroId;

	public void Init(HeroPanel heroPanel,string heroId,bool unlock){
		this.heroPanel = heroPanel;
		icon.sprite = Resources.Load<Sprite>("image/hero/"+heroId);
		this.heroId = heroId;

		if(GlobalData.newHeroHint.Contains(heroId)){
			hintNew.SetActive(true);
		}

		if(unlock){
			icon.color = new Color(0.8f,0.8f,0.8f,0.6f);
			GetComponent<Image>().color = new Color(0.7f,0.7f,0.7f);
		}
	}

	public void Refresh(){
		tagSelected.SetActive(heroPanel.selectedHeroId == heroId);
		tagUsing.SetActive(UserData.data.heroUsing == heroId);
	}

	public void Click(){
		if(heroPanel.selectedHeroId != heroId)heroPanel.SelectHero(heroId);

		GlobalData.newHeroHint.Remove(heroId);
		hintNew.SetActive(false);
	}
}
