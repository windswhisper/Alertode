using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;
using Honeti;

public class TechPanel : MonoBehaviour
{
	public GameObject buildOption;
	public GameObject chipOption;

	public List<TechBuildItem> techBuildItems;
	public List<TechChipItem> techChipItems;

	public GameObject techLevelItem;

	public Animation anim;
	public Text txtRerollCost;

	int level;
	int rerollCount;
	
	List<string> existedBuilds = new List<string>();
	List<TechChipData> existedChips = new List<TechChipData>();

	public void Show(int level){
		this.level = level;
		BattleManager.ins.Pause();
		gameObject.SetActive(true);

		int techType = GetTechTypeByLevel(level);

		rerollCount = 0;

		if(techType == 0){
			ShowBuildOption();
		}
		else if(techType == 1){
			ShowChipOption();
		}

	}

	public void Show2(){
		BattleManager.ins.Pause();
		gameObject.SetActive(true);
		anim.Play("tech_panel_show2");
	}

	public int GetTechTypeByLevel(int level){
		if(level>=Configs.commonConfig.techPathTypes.Count){
			return 1;
		}
		else{
			return Configs.commonConfig.techPathTypes[level];
		}
	}
	public void ShowBuildOption(){
		buildOption.SetActive(true);
		chipOption.SetActive(false);

		List<string> newBuilds = new List<string>();
		List<string> allBuilds = new List<string>();
		existedBuilds.Clear();

		var aaTower = "";
		List<string> aaTowers = new List<string>();
		if(level!=0){
			if(!BuildBar.ins.buildIDList.Contains("Misl")){
				aaTowers.Add("Misl");
			}
			else if(!BuildBar.ins.buildIDList.Contains("Laser")){
				aaTowers.Add("Laser");
			}
			else if(!BuildBar.ins.buildIDList.Contains("Plasma")){
				aaTowers.Add("Plasma");
			}
			else if(!BuildBar.ins.buildIDList.Contains("Lunar")){
				aaTowers.Add("Lunar");
			}
			aaTower = aaTowers[TSRandom.Range(0,aaTowers.Count)];
		}

		//foreach(var build in UserData.data.buildsCollect){
		for(var i=1;i<Configs.buildConfig.datas.Count;i++ ){
			var build = Configs.buildConfig.datas[i].name;
			var data = Configs.buildConfig.datas[i];
			if(data.uncollectable)continue;
			if(!BuildBar.ins.buildIDList.Contains(build)){
				int cost = Configs.GetBuild(build).cost;
				if(level==0){
					if(cost>=100)continue;
				}
				else{
					if(aaTower == build)continue;
					if(cost<100)continue;
				}
				bool isBan = false;
				foreach(var tag in BattleManager.ins.battleModifier.nagetiveBuildTag){
					if(data.tags.Contains(tag)){
						isBan = true;
						continue;
					}
				}
				if(isBan)continue;
				allBuilds.Add(build);
			}
		}

		for(var i=0;i<3;i++){
			techBuildItems[i].gameObject.SetActive(false);
		}

		if(allBuilds.Count == 0)return;

		//TODO 使用与战斗逻辑分离的随机种子
		allBuilds = Utils.RandomList<string>(allBuilds);

		for(var i=0;i<3&&i<allBuilds.Count;i++){
			newBuilds.Add(allBuilds[i]);
			existedBuilds.Add(allBuilds[i]);
		}

		if(level>0){
			newBuilds[0] = aaTower;
			existedBuilds[0] = aaTower;
		}

		for(var i=0;i<3&&i<newBuilds.Count;i++){
			techBuildItems[i].Init(newBuilds[i]);
			techBuildItems[i].btnReroll.SetActive(true);
		}
	}

	public void ShowChipOption(){
		chipOption.SetActive(true);
		buildOption.SetActive(false);
		
		List<string> myBuilds = new List<string>();

		foreach(var build in BuildBar.ins.buildIDList){
			int count = 0;
			foreach(var tech in BattleManager.ins.techChipList){
				if(build == tech.buildID){
					count++;
					if(Configs.GetChip(tech.chipID).canUpgrade){
						count--;
					}
				}
			}
			if(count<3){
				myBuilds.Add(build);
			}
		}

		myBuilds = Utils.RandomList<string>(myBuilds);

		txtRerollCost.text = "(     x"+rerollCount*20+")";

		if(myBuilds.Count==0){
			Close();
			return;
		}


		for(var i=0;i<3&&i<myBuilds.Count;i++){
			techChipItems[i].Init(myBuilds[i],Utils.RandomChip(myBuilds[i],existedChips,BattleManager.ins.techChipList));

			techChipItems[i].gameObject.SetActive(true);

		}

		existedChips.Clear();

		if(myBuilds.Count<3){
			for(var i=myBuilds.Count;i<3;i++){
				techChipItems[i].gameObject.SetActive(false);
			}
		}
		for(var i=0;i<3&&i<myBuilds.Count;i++){
       		existedChips.Add(new TechChipData(techChipItems[i].buildID,techChipItems[i].chipID));
		}
	}

	public void ChooseChipOption(string buildID,string chipID){
		BattleManager.ins.AddNewTech(buildID,chipID);
	}

	public void RerollBuild(int index){
		List<string> newBuilds = new List<string>();
		List<string> allBuilds = new List<string>();

		//foreach(var build in UserData.data.buildsCollect){
		for(var i=1;i<Configs.buildConfig.datas.Count;i++ ){
			var data = Configs.buildConfig.datas[i];
			if(data.uncollectable)continue;
			var build = Configs.buildConfig.datas[i].name;

			bool isRepeat = false;
        	for(var j=0;j<techBuildItems.Count;j++){
        		if(build == techBuildItems[j].buildID){
        			isRepeat = true;
        		}
        	}
        	for(var j=0;j<existedBuilds.Count;j++){
        		if(build == existedBuilds[j]){
        			isRepeat = true;
        		}
        	}

        	if(isRepeat)continue;

        	bool isBan = false;
			foreach(var tag in BattleManager.ins.battleModifier.nagetiveBuildTag){
				if(data.tags.Contains(tag)){
					isBan=true;
					continue;
				}
			}
        	if(isBan)continue;

			if(!BuildBar.ins.buildIDList.Contains(build)){
				int cost = Configs.GetBuild(build).cost;
				if(level==0){
					if(cost>=100)continue;
				}
				else{
					if(cost<100)continue;
				}
				allBuilds.Add(build);
			}
		}

		techBuildItems[index].Init(allBuilds[TSRandom.Range(0,allBuilds.Count)]);
        existedBuilds.Add(techBuildItems[index].buildID);
	}

	public void RerollChip(int index){
		List<string> myBuilds = new List<string>();

		foreach(var build in BuildBar.ins.buildIDList){
			int count = 0;
			foreach(var tech in BattleManager.ins.techChipList){
				if(build == tech.buildID){
					count++;
					if(Configs.GetChip(tech.chipID).canUpgrade){
						count--;
					}
				}
			}
			if(count<3){
				myBuilds.Add(build);
			}
		}

		var buildId = myBuilds[TSRandom.Range(0,myBuilds.Count)];
		var chipId = Utils.RandomChip(buildId,existedChips,BattleManager.ins.techChipList);

		if(chipId!="")techChipItems[index].Init(buildId,chipId);
	}          

	public void RerollAllChip(){
		if(BattleManager.ins.engine.coin>=20*rerollCount){
			BattleManager.ins.engine.coin-=20*rerollCount;
			rerollCount++;
			ShowChipOption();
		}
		else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoCoin"));
		}
	}

	public void ChooseBuilds(){
		BuildBar.ins.AddBuildItem(techBuildItems[0].buildID);
		BuildBar.ins.AddBuildItem(techBuildItems[1].buildID);
		BuildBar.ins.AddBuildItem(techBuildItems[2].buildID);
		Close();
	}

	public void Close(){
		gameObject.SetActive(false);
		BattleManager.ins.Resume();
	}

	public void HidePanel(){
		anim.Play("tech_panel_hide");
	}

	public void ShowPanel(){
		anim.Play("tech_panel_show");
	}

	public void Hide2(){
		anim.Play("tech_panel_hide2");
	}
}
