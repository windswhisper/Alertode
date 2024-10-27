using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class ProgressData
{
	public string id;
	public int progress;
	public bool claimReward;
	public ProgressData(string id, int progress){
		this.id = id;
		this.progress = progress;
		claimReward = false;
	}
}

[Serializable]
public class TeamBuildData
{
	public string buildId;
	public bool isUsing;
	public int chipSlotCount;
	public int chipSlot0;
	public int chipSlot1;
	public int chipSlot2;
	public TeamBuildData(string buildId){
		this.buildId = buildId;
	}
	public int GetChipSlot(int index){
		if(index == 0){
			return chipSlot0;
		}
		else if(index == 1){
			return chipSlot1;
		}
		
		return chipSlot2;
	}
	public void SetChipSlot(int index){
		if(index == 0){
			chipSlot0 = index;
		}
		else if(index == 1){
			chipSlot1 = index;
		}
		else {
			chipSlot2 = index;
		}
	}

}

[Serializable]
public class UserDataConfig
{
	//保存json用
	public List<ProgressData> stageProgressDatas = new List<ProgressData>();
	public List<ProgressData> achieveProgressDatas = new List<ProgressData>();
	//实际使用
	public Dictionary<string,int> stageProgress = new Dictionary<string,int>();
	public string userId;
	public string joinTime;
	public List<TeamBuildData> buildsCollect;
	public List<string> herosCollect;
	public string heroUsing;
	public int selectedChapter;
	public int selectedStage;
	public int selectedMode;
	public int guideProgress;
	public int highScoreSurvive;
	public int highScoreFieldRunner;
	public int gem;
	public int silicon;
	public int metal;
	public long lastEnterTime;
	public int adGemCount;
	public int adMetalCount;
	public string openId;

	public void Init(){
		stageProgress.Clear();

		for(var i=0;i<Configs.stageConfig.chapters.Count;i++){
			var chapter = Configs.stageConfig.chapters[i];
			for(var j=0;j<chapter.stages.Count;j++){
				stageProgress.Add(chapter.id+"-"+chapter.stages[j].id,0);
			}
		}

		foreach(var achieve in Configs.achievementConfig.datas){
			achieveProgressDatas.Add(new ProgressData(achieve.name,0));
		}

		buildsCollect = new List<TeamBuildData>();
		buildsCollect.Add(new TeamBuildData("Miner"));
		buildsCollect.Add(new TeamBuildData("Cannon"));
		buildsCollect.Add(new TeamBuildData("Barr"));
		buildsCollect.Add(new TeamBuildData("Misl"));
		buildsCollect.Add(new TeamBuildData("Gatt"));

		foreach(var build in buildsCollect){
			build.isUsing = true;
		}

		herosCollect = new List<string>();
		herosCollect.Add("Gunner");
		heroUsing = "Gunner";

		selectedChapter = 0;
		selectedStage = 0;
		selectedMode = 0;
		guideProgress = 0;
		gem = 200;
		silicon = 50;
		metal = 50;
	}
}


public class UserData
{
	public static UserDataConfig data;

	public static string SAVE_PATH = "./Saves/playerData.sav";
	public static string SAVE_FOLDER = "./Saves";

	public static string SAVE_PATH_MOBILE = Application.persistentDataPath+"/Saves/playerData.sav";
	public static string SAVE_FOLDER_MOBILE = Application.persistentDataPath+"/Saves";

	public static void Load(){
		string json = "";

		#if UNITY_EDITOR || UNITY_STANDALONE

			if(File.Exists(SAVE_PATH)){
				json = File.ReadAllText(SAVE_PATH);
				json = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(json));
			}
			else{
				if(!Directory.Exists(SAVE_FOLDER)){
					Directory.CreateDirectory(SAVE_FOLDER);
				}
			}

		#elif UNITY_ANDROID || UNITY_IPHONE

			if(File.Exists( SAVE_PATH_MOBILE)){
				json = File.ReadAllText(SAVE_PATH_MOBILE);
				json = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(json));
			}
			else{
				if(!Directory.Exists(SAVE_FOLDER_MOBILE)){
					Directory.CreateDirectory(SAVE_FOLDER_MOBILE);
				}
			}

		#endif

		if(json == ""){
			data = new UserDataConfig();
			data.Init();
		}
		else{
			data = JsonUtility.FromJson<UserDataConfig> (json);

			data.stageProgress.Clear();
			foreach(var progress in data.stageProgressDatas){
				data.stageProgress.Add(progress.id,progress.progress);
			}

			CheckNew();

		}

		var day = System.DateTime.Now.Day;
		if(day!=data.lastEnterTime){
			ResetDaily();
			data.lastEnterTime = day;
		}

		GlobalData.Init();
	}

	public static void LoadFromString(string str){
		if(str==""){
			ToastBar.ShowMsg("数据为空");
			return;
		}

		try{
			var json = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(str));

			var newData = JsonUtility.FromJson<UserDataConfig> (json);

			if(GlobalData.tapUserProfile!=null && newData.openId!=GlobalData.tapUserProfile.openid){
				ToastBar.ShowMsg("需登录同一个Tap账户");
				return;
			}

			data = newData;

			data.stageProgress.Clear();
			foreach(var progress in data.stageProgressDatas){
				data.stageProgress.Add(progress.id,progress.progress);
			}

			CheckNew();

			Save();

			ToastBar.ShowMsg("已导入");
		}
		catch(Exception e){
		 	Debug.Log(e);
			ToastBar.ShowMsg("数据格式错误");
		}
	}

	static void CheckNew(){
		for(var i=0;i<Configs.stageConfig.chapters.Count;i++){
			var chapter = Configs.stageConfig.chapters[i];
			for(var j=0;j<chapter.stages.Count;j++){
				if(!data.stageProgress.ContainsKey(chapter.id+"-"+chapter.stages[j].id))
					data.stageProgress.Add(chapter.id+"-"+chapter.stages[j].id,0);
			}
		}

		foreach(var achieve in Configs.achievementConfig.datas){
			var isNew = true;
			foreach(var progress in data.achieveProgressDatas){
				if(progress.id==achieve.name)isNew = false;
			}
			if(isNew)
				data.achieveProgressDatas.Add(new ProgressData(achieve.name,0));
		}

		if(data.joinTime==null || data.joinTime=="")data.joinTime = (System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds + "";
	}

	static void ResetDaily(){
		data.adGemCount = 0;
		data.adMetalCount = 0;
	}

	public static void Save(){
		data.stageProgressDatas.Clear();

		foreach(var pair in data.stageProgress){
			data.stageProgressDatas.Add(new ProgressData(pair.Key,pair.Value));
		}

		#if UNITY_EDITOR || UNITY_STANDALONE

			File.WriteAllText(SAVE_PATH,System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson (data))));

		#elif UNITY_ANDROID || UNITY_IPHONE
			
			File.WriteAllText(SAVE_PATH_MOBILE,System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson (data))));

		#endif
	}

	public static string Export(){

		data.stageProgressDatas.Clear();

		foreach(var pair in data.stageProgress){
			data.stageProgressDatas.Add(new ProgressData(pair.Key,pair.Value));
		}

		return System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson (data)));
	}

	public static string GetUserId(){
		if(data.userId ==null || data.userId == ""){
			data.userId = Utils.CreateUserId();
			Save();
		}

		return data.userId;
	}
}
