using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class HeroPanel : MonoBehaviour
{
	public Transform listRoot;
	public GameObject heroItem;
	public Image card;
	public Text txtName;
	public Text txtDesc;
	public Text txtHp;
	public Text txtDamage;
	public Text txtRange;
	public List<ItemChip> chips;
	public List<TipsChipInfo> chipInfos;
	public List<ItemHeroSkill> skills;
	public List<TipsSkillInfo> skillInfos;
	public GameObject btnUse;
    public GameObject tagUsed;
    public GameObject tagLock;
    public Animation animSwitchHero;

	List<ItemHero> myHeros = new List<ItemHero>();
	public string selectedHeroId;

	void Start(){
    	foreach(var heroId in UserData.data.herosCollect){
    		var item = Instantiate(heroItem);
    		item.GetComponent<ItemHero>().Init(this,heroId,false);
    		item.transform.SetParent(listRoot,false);
    		myHeros.Add(item.GetComponent<ItemHero>());
    	}

        foreach(var data in Configs.heroConfig.datas){
            if(!UserData.data.herosCollect.Contains(data.name)){
                var item = Instantiate(heroItem);
                item.GetComponent<ItemHero>().Init(this,data.name,true);
                item.transform.SetParent(listRoot,false);
                myHeros.Add(item.GetComponent<ItemHero>());
            }
        }

    	SelectHero(UserData.data.heroUsing);
	}


    public void SelectHero(string heroId){
    	selectedHeroId = heroId;

    	var data = Configs.GetHero(heroId);

    	card.sprite = Resources.Load<Sprite>("image/hero/"+heroId+"_Card");
    	txtName.text = I18N.instance.getValue(data.uiName+"_Title")+" "+I18N.instance.getValue(data.uiName);
    	txtDesc.text = I18N.instance.getValue(data.uiName+"_Desc");
    	txtHp.text = data.strength+"";

    	if(data.weapons.Count!=0){
			var weaponData = Configs.GetWeapon(data.weapons[0]);
			txtDamage.text = string.Format("{0}×{1:F1}",weaponData.damage,weaponData.fireSpeed/100f);
			txtRange.text =  string.Format("{0:F1}",weaponData.range/100f);
    	}
    	else{
    		txtDamage.text = "-";
    		txtRange.text = "-";
    	}

    	for(var i=0;i<data.defaultChips.Count;i++){
    		chips[i].Init(data.defaultChips[i]);
    		chipInfos[i].Init(data.defaultChips[i]);
    	}

    	for(var i=0;i<data.ultimateSkills.Count;i++){
    		skills[i].Init(data.ultimateSkills[i]);
    		skillInfos[i].Init(data.ultimateSkills[i]);
    	}

        if(UserData.data.herosCollect.Contains(heroId)){
            btnUse.SetActive(heroId != UserData.data.heroUsing);
            tagUsed.SetActive(heroId == UserData.data.heroUsing);
            tagLock.SetActive(false);
        }
        else{
            btnUse.SetActive(false);
            tagUsed.SetActive(false);
            tagLock.SetActive(true);
        }

        animSwitchHero.Play();

    	RefreshList();
    }

    public void UseHero(){
    	UserData.data.heroUsing = selectedHeroId;

    	btnUse.SetActive(false);
    	tagUsed.SetActive(true);
    	RefreshList();
    }

    public void RefreshList(){
    	foreach(var item in myHeros){
    		item.Refresh();
    	}
    }
}
