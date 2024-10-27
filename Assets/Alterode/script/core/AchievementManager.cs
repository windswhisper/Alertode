using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager
{	
	public static void OnBattleWin(){
		if(GlobalData.selectedChapter == 0&&GlobalData.selectedStage == 2){
			SetProgress(0,1);
		}
		if(GlobalData.selectedDifficult == 3){
			SetProgress(1,1);

			bool allWinHard3 = true;
			foreach(var pair in UserData.data.stageProgress){
				if(pair.Value<=3)allWinHard3 = false;
			}
			if(allWinHard3)SetProgress(8,1);
		}
		if(GlobalData.selectedMode == 1){
			SetProgress(2,1);
		}


		bool allWin = true;
		foreach(var pair in UserData.data.stageProgress){
			if(pair.Value==0)allWin = false;
		}
		if(allWin)SetProgress(7,1);

		if(GlobalData.selectedMode == 0){
			var count=0;
			foreach(var build in UserData.data.buildsCollect){
				if(build.isUsing == true)count++;
			}

			if(count<=1){
				SetProgress(13,1);
			}
		}
	}

	public static void OnBattleWinWithNoLose(){
		SetProgress(6,1);
	}

	public static void OnBattleFail(){
		AddProgress(9,1);
	}

	public static void OnBattleRevive(){
		AddProgress(10,1);
	}

	public static void OnKillMonster(){
		AddProgress(3,1);
	}

	public static void OnHeroKillMonster(){
		AddProgress(4,1);
	}

	public static void OnBuild(){
		AddProgress(15,1);
	}

	public static void OnSetupChip(){
		AddProgress(17,1);
	}

	public static void OnOwnMaxLevelBuilding(int count){
		SetProgress(16,count);
	}

	public static void OnGetMoney(int money){
		SetProgress(5,money);
	}

	public static void OnGetBuild(){
		SetProgress(11,UserData.data.buildsCollect.Count);
	}

	public static void OnGetHero(){
		SetProgress(12,UserData.data.herosCollect.Count);
	}

	public static void GetAchievement(string name){
		bool isAllAchieved = true;
		foreach(var achieve in UserData.data.achieveProgressDatas){
			var data = Configs.GetAchieve(achieve.id);
			if(data.type!=14){
				if(achieve.progress < data.goal){
					isAllAchieved = false;
				}
			}
		}
		if(isAllAchieved)SetProgress(14,1);

		foreach(var achieve  in UserData.data.achieveProgressDatas){
			if(achieve.id == name){
				achieve.claimReward = true;
			}
		}
	}

	public static void AddProgress(int type, int progress){
		foreach(var achieve  in UserData.data.achieveProgressDatas){
			var data = Configs.GetAchieve(achieve.id);
			if(data.type == type){
				achieve.progress += progress;
			}
		}
	}

	public static void SetProgress(int type, int progress){
		foreach(var achieve  in UserData.data.achieveProgressDatas){
			var data = Configs.GetAchieve(achieve.id);
			if(data.type == type && progress > achieve.progress){
				achieve.progress = progress;
			}
		}
	}
}
