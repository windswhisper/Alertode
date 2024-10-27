using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[Serializable]
public class MapBuildData{
	public string buildId;
	public int x;
	public int y;
	public bool isHero;
	public int level;
	public int targetPrefer;
	public List<int> extraParas;
	public string extraNamePara;
}

[Serializable]
public class SaveGameData{
	public int gameMode;
	public int difficult;
	public int chapterId;
	public int stageId;
	public int wave;
	public int coin;
	public int seed;
    public BattleModifier battleModifier;
    public List<TechChipData> techChipList;
    public List<MapBuildData> mapBuildList;
    public string heroId;
    public List<string> buildBarIdList;
}

public class SaveGame
{
	#if UNITY_EDITOR || UNITY_STANDALONE
		public static string SAVE_PATH = "./Saves/lastGame.sav";
		public static string SAVE_PATH_ENDLESS = "./Saves/lastGameEndless.sav";

	#elif UNITY_ANDROID || UNITY_IPHONE
		public static string SAVE_PATH = Application.persistentDataPath+"/Saves/lastGame.sav";
		public static string SAVE_PATH_ENDLESS = Application.persistentDataPath+"/Saves/lastGameEndless.sav";

	#endif

	public static bool HasSaveGame(int type){

        if(type == 0){
			if(File.Exists(SAVE_PATH)){
				return true;
			}
        }
        else{
			if(File.Exists(SAVE_PATH_ENDLESS)){
				return true;
			}
        }
        return false;
	}

    public static SaveGameData Load(int type)
    {
    	string json;
        if(type == 0){
			json = File.ReadAllText(SAVE_PATH);
			json = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(json));
        }
        else{
			json = File.ReadAllText(SAVE_PATH_ENDLESS);
			json = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(json));
        }

		var	data = JsonUtility.FromJson<SaveGameData> (json);

		return data;
    }

    public static void Save()
    {
    	var manager = BattleManager.ins;
        var data = new SaveGameData();
        data.gameMode = manager.gameMode;
        data.difficult = manager.difficult;
        data.chapterId = GlobalData.selectedChapter;
        data.stageId = GlobalData.selectedStage;
        data.wave = manager.engine.wave;
        data.coin = manager.engine.coin;
        data.seed = manager.engine.seed;
        data.battleModifier = manager.battleModifier;
        foreach(var techChip in manager.techChipList){
        	techChip.ExportPara();
        }
        data.techChipList = manager.techChipList;
        data.heroId = BuildBar.ins.GetHeroId();
        data.buildBarIdList = BuildBar.ins.ToIdList();
        data.mapBuildList = new List<MapBuildData>();
        foreach(var unit in manager.engine.unitList){
        	if(unit.type == UnitType.Building){
        		data.mapBuildList.Add(((BaseBuild)unit).ToData());
        	}
        }

        if(data.gameMode<2)
			File.WriteAllText(SAVE_PATH,System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson (data))));
		else
			File.WriteAllText(SAVE_PATH_ENDLESS,System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson (data))));

    }

    public static void Delete(){
    	var manager = BattleManager.ins;

        if(manager.gameMode<2)
			File.Delete(SAVE_PATH);
		else
			File.Delete(SAVE_PATH_ENDLESS);
    }

}
