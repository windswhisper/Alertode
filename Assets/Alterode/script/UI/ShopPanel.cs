using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ShopPanel : MonoBehaviour
{
	public GameObject newBuildPanel;

	public GameObject soldOutHero;
	public GameObject soldOutBuild;
	public GameObject buyHeroPanel;
	public GameObject buyTowerPanel;

	public Text txtGem;
	public Text txtSilicon;
	public Text txtMetal;

	public GameObject soldOutGem;
	public GameObject soldOutMetal;
	public GameObject rewardPanelPrefab;
	public GameObject btnAdGem;
	public GameObject btnAdMetal;

	void Start(){
		RefreshItem();

		#if UNITY_ANDROID || UNITY_IPHONE
			btnAdGem.SetActive(true);
			btnAdMetal.SetActive(true);
		#endif
	}

	public void RefreshItem(){
		var uncollectableCount = 0;
		foreach(var data in Configs.buildConfig.datas){
			if(data.uncollectable)uncollectableCount++;
		}
		if(Configs.buildConfig.datas.Count - uncollectableCount == UserData.data.buildsCollect.Count){
			soldOutBuild.SetActive(true);
		}
		if(Configs.heroConfig.datas.Count  == UserData.data.herosCollect.Count){
			soldOutHero.SetActive(true);
		}

		if(UserData.data.adGemCount>=3){
			soldOutGem.SetActive(true);
		}
		if(UserData.data.adMetalCount>=3){
			soldOutMetal.SetActive(true);
		}

		txtGem.text = UserData.data.gem+"";
		txtSilicon.text = UserData.data.silicon+"";
		txtMetal.text = UserData.data.metal+"";
	}

	public void ShowBuyTowerPanel(){
		if(UserData.data.metal<200){
			ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoMetal"));
			return;
		}

		buyTowerPanel.SetActive(true);
	}

	public void ShowBuyHeroPanel(){
		if(UserData.data.gem<2000){
			ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoGem"));
			return;
		}

		buyHeroPanel.SetActive(true);
	}

	public void BuyTower(){
		if(UserData.data.metal<200){
			ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoMetal"));
			return;
		}
		UserData.data.metal-=200;

		var list = new List<string>();

		foreach(var data in Configs.buildConfig.datas){
			if(!data.uncollectable)list.Add(data.name);
		}
		foreach(var build in UserData.data.buildsCollect){
			list.Remove(build.buildId);
		}



		var unlockBuild = list[UnityEngine.Random.Range(0,list.Count)];

        UserData.data.buildsCollect.Add(new TeamBuildData(unlockBuild));

        var panel = Instantiate(newBuildPanel);
        panel.GetComponent<NewBuildPanel>().Show(unlockBuild);
        panel.transform.SetParent(transform.parent,false);

        UserData.Save();

        RefreshItem();
	}

	public void BuyHero(){
		if(UserData.data.gem<2000){
			ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoGem"));
			return;
		}
		UserData.data.gem-=2000;

		var list = new List<string>();

		foreach(var data in Configs.heroConfig.datas){
			if(!data.uncollectable)list.Add(data.name);
		}
		foreach(var hero in UserData.data.herosCollect){
			list.Remove(hero);
		}


		var unlockHero = list[UnityEngine.Random.Range(0,list.Count)];

        UserData.data.herosCollect.Add(unlockHero);

        var panel = Instantiate(newBuildPanel);
        panel.GetComponent<NewBuildPanel>().ShowHero(unlockHero);
        panel.transform.SetParent(transform.parent,false);

        UserData.Save();

        RefreshItem();

	}

	public void AdDiamond(){
        MobileAdManager.ins.ShowRewardedAd(()=>{
        	UserData.data.gem+=200;
        	UserData.data.adGemCount++;

    		List<GiftContentData> rewards = new List<GiftContentData>();
    		rewards.Add(new GiftContentData("gem",200));

	    	var rewardPanel = Instantiate(rewardPanelPrefab);
	    	rewardPanel.GetComponent<RewardPanel>().Init(rewards);
	    	rewardPanel.transform.SetParent(transform.parent,false);

    		UserData.Save();

			RefreshItem();
        });
	}

	public void AdMetal(){
        MobileAdManager.ins.ShowRewardedAd(()=>{
        	UserData.data.metal+=50;
        	UserData.data.adMetalCount++;

    		List<GiftContentData> rewards = new List<GiftContentData>();
    		rewards.Add(new GiftContentData("metal",50));

	    	var rewardPanel = Instantiate(rewardPanelPrefab);
	    	rewardPanel.GetComponent<RewardPanel>().Init(rewards);
	    	rewardPanel.transform.SetParent(transform.parent,false);

    		UserData.Save();

			RefreshItem();
        });
	}
}
