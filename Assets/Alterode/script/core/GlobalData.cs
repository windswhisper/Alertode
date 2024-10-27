using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData
{
	public static int selectedChapter = 0;
	public static int selectedStage = 0;
	public static int selectedMode = 0;
	public static int selectedDifficult = 0;
	public static BattleModifier battleModifier = null;
	public static int selectedSurviveMode = 0;
	public static bool isLoadGame = false;
	public static int saveGameType = 0;

	public static List<string> newHeroHint = new List<string>();
	public static List<string> newBuildHint = new List<string>();

	public static TapTap.Login.Profile tapUserProfile;

	public static void Init(){
		selectedChapter = UserData.data.selectedChapter;
		selectedStage = UserData.data.selectedStage;
		selectedMode = UserData.data.selectedMode;
	}
}
