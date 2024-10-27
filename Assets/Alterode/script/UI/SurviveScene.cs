using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class SurviveScene : MonoBehaviour
{
    public GameObject hintNewHero;
    public GameObject hintNewTower;
    public Text txtHighScoreSurvive;
    public Text txtHighScoreFieldRunner;
    public LoadGamePanel loadGamePanel;

    void Start()
    {
        if(GlobalData.newHeroHint.Count!=0){
            hintNewHero.SetActive(true);
        }
        if(GlobalData.newBuildHint.Count!=0){
            hintNewTower.SetActive(true);
        }

        txtHighScoreSurvive.text = string.Format(I18N.instance.getValue("@UI.HighScoreWave"),UserData.data.highScoreSurvive);
        txtHighScoreFieldRunner.text = string.Format(I18N.instance.getValue("@UI.HighScoreWave"),UserData.data.highScoreFieldRunner);

        if(SaveGame.HasSaveGame(1)){
            loadGamePanel.Show(1);
        }

        VolumeManager.ins.PlayBgm("The Medieval Sailor - SkyhammerSound");
    }

    public void EnterSurviveMode(){
        GlobalData.selectedSurviveMode = 3;

        GlobalData.battleModifier = new BattleModifier();
        GlobalData.battleModifier.reviveMaxCount = 1;
        GlobalData.battleModifier.nagetiveBuildTag.Add("NoInfinite");
        GlobalData.battleModifier.nagetiveBuildTag.Add("Naval");

        UserData.Save();

        LoadingMask.ins.LoadSceneAsync("BattleScene");
    }
    public void EnterFieldRunnerMode(){
        GlobalData.selectedSurviveMode = 2;

        GlobalData.battleModifier = new BattleModifier();
        GlobalData.battleModifier.isPBMode = true;
        GlobalData.battleModifier.isTDMode = true;
        GlobalData.battleModifier.reviveMaxCount = 1;
        GlobalData.battleModifier.nagetiveBuildTag.Add("NoInfinite");
        GlobalData.battleModifier.nagetiveBuildTag.Add("Walker");
        GlobalData.battleModifier.nagetiveBuildTag.Add("Naval");

        UserData.Save();

        LoadingMask.ins.LoadSceneAsync("BattleScene");
    }

    public void Back(){
        LoadingMask.ins.LoadSceneAsync("MainScene");
    }
}
