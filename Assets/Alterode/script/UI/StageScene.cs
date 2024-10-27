using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Honeti;

public class StageScene : MonoBehaviour
{
	public RectTransform container;
    public GameObject itemChapterPrefab;
    public GameObject itemStagePrefab;
	public Image stagePictrue;
    public Text txtMapName;
    public Text txtWave;
    public GameObject mapTagFog;
    public GameObject mapTagBoss;
    public GameObject mapTagPBMode;
    public GameObject itemMonsterInfoPrefab;
    public Transform monsterInfoContainer;
    public Text txtMode;
    public Text txtDifficult;
    public Text txtModeDesc;
    public Text txtDifficultDesc;
    public Button btnPreDifficult;
    public Button btnNextDifficult;
    public GameObject hintNewHero;
    public GameObject hintNewTower;
    public GameObject modeSelect;
    public GameObject diffSelect;
    public LoadGamePanel loadGamePanel;

    List<ItemChapter> itemChapters = new List<ItemChapter>();
    List<ItemStage> items = new List<ItemStage>();
    StageData selected;
    int mode = 0;
    int difficult = 0;
    int selectedChapter = 0;

    public static string[] modeName = {"@UI.ModeClassical","@UI.ModeGacha","@UI.ModeChaos"};
    public static string[] modeDesc = {"@UI.ClassicalDesc","@UI.GachaDesc","@UI.ChaosDesc"};
    public static string[] difficultDesc = {"@UI.DifficultDesc00","@UI.DifficultDesc01","@UI.DifficultDesc02","@UI.DifficultDesc03","@UI.DifficultDesc04","@UI.DifficultDesc05"};

    void Start()
    {
        foreach(var chapter in Configs.stageConfig.chapters){
            var itemChapter = Instantiate(itemChapterPrefab);
            itemChapters.Add(itemChapter.GetComponent<ItemChapter>());
            itemChapter.transform.SetParent(container,false);
            foreach(var stageData in chapter.stages){
                var item = Instantiate(itemStagePrefab);
                item.GetComponent<ItemStage>().Init(this,chapter.id,stageData);
                items.Add(item.GetComponent<ItemStage>());
                item.transform.SetParent(container,false);
                item.gameObject.SetActive(false);
            }
            itemChapter.GetComponent<ItemChapter>().Init(this,chapter.id,chapter.uiName);
        }

        mode = GlobalData.selectedMode;
        txtMode.text = I18N.instance.getValue(modeName[mode]);
        txtModeDesc.text = I18N.instance.getValue(modeDesc[mode]);

        difficult = GlobalData.selectedDifficult;

        foreach(var item in itemChapters){
            if(item.chapterId == GlobalData.selectedChapter){
                item.Selected();
                break;
            }
        }

        foreach(var item in items){
            if(item.data.id == GlobalData.selectedStage && item.chapterId == GlobalData.selectedChapter){
                item.Selected();
                break;
            }
        }

        btnPreDifficult.interactable = difficult!=0;
        if(difficult==3 || difficult>=UserData.data.stageProgress[selectedChapter+"-"+selected.id])btnNextDifficult.interactable = false;

        txtDifficult.text = string.Format(I18N.instance.getValue("@UI.Difficult"),difficult,3);
        txtDifficultDesc.text = I18N.instance.getValue(difficultDesc[difficult]);

        if(GlobalData.newHeroHint.Count!=0){
            hintNewHero.SetActive(true);
        }
        if(GlobalData.newBuildHint.Count!=0){
            hintNewTower.SetActive(true);
        }

        if(SaveGame.HasSaveGame(0)){
            loadGamePanel.Show(0);
        }

        VolumeManager.ins.PlayBgm("The Medieval Sailor - SkyhammerSound");
    }

    public void SelectChapter(int chapterId,bool isShow){
        var count = 0;

        foreach(var item in items){
            if(item.chapterId == chapterId){
                item.gameObject.SetActive(isShow);
            }
            if(item.gameObject.activeInHierarchy){
                count++;
            }
        }

        container.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, count * 95 + itemChapters.Count*115 + 180);
    }

    public void SelectStage(int chapterId,StageData data){
    	foreach(var item in items){
    		if(item.data != data){
    			item.Unselected();
    		}
    	}

        if(chapterId==0){
            modeSelect.SetActive(false);
            diffSelect.SetActive(false);
        }
        else{
            modeSelect.SetActive(true);
            diffSelect.SetActive(true);
        }

        var mapFile = Resources.Load<TextAsset>("map/"+data.mapName);
        var mapData = JsonUtility.FromJson<MapData>(mapFile.text);

        txtMapName.text = I18N.instance.getValue(data.uiName);
        txtWave.text = string.Format(I18N.instance.getValue("@UI.TotalWave"),mapData.wave);
        mapTagFog.SetActive(mapData.fog);
        bool hasBoss = false;
        foreach(var monsters in mapData.monGens){
            if(monsters.isBoss)hasBoss = true;
        }
        mapTagBoss.SetActive(hasBoss);
        mapTagPBMode.SetActive(data.isPBMode);

        var monterNames = new Dictionary<string,int>();

        foreach(var monGens in mapData.monGens){
            foreach(var monster in monGens.monsters){
                if(monster=="")continue;
                if(!monterNames.ContainsKey(monster)){
                    monterNames.Add(monster,monGens.repeat);
                }
                else{
                    monterNames[monster]+=monGens.repeat;
                }
            }
        }

        for(var i=0;i<monsterInfoContainer.childCount;i++){
            Destroy(monsterInfoContainer.GetChild(i).gameObject);
        }

        var count = 0;
        foreach(var monster in monterNames){
            if(count>=8)break;
            count++;
            var item = Instantiate(itemMonsterInfoPrefab);
            item.GetComponent<ItemMonsterGen>().Init(monster.Key,monster.Value);
            item.transform.SetParent(monsterInfoContainer,false);

        }

    	stagePictrue.sprite = Resources.Load<Sprite> ("image/stage/"+data.mapName);
        selected = data;
        selectedChapter = chapterId;

        difficult = UserData.data.stageProgress[chapterId+"-"+data.id];
        if(difficult>3)difficult = 3;

        btnPreDifficult.interactable = difficult!=0;
        if(difficult==3 || difficult>=UserData.data.stageProgress[selectedChapter+"-"+selected.id])btnNextDifficult.interactable = false;

        txtDifficult.text = string.Format(I18N.instance.getValue("@UI.Difficult"),difficult,3);
        txtDifficultDesc.text = I18N.instance.getValue(difficultDesc[difficult]);
    }

    public void StartStage(){
        var mapFile = Resources.Load<TextAsset>("map/"+selected.mapName);
        var mapData = JsonUtility.FromJson<MapData>(mapFile.text);

        GlobalData.selectedChapter = selectedChapter;
        GlobalData.selectedStage = selected.id;
        GlobalData.selectedMode = mode;
        GlobalData.selectedDifficult = difficult;
        GlobalData.selectedSurviveMode = 0;

        GlobalData.battleModifier = new BattleModifier();
        GlobalData.battleModifier.nagetiveBuildTag=mapData.negativeTags;

        if(selected.isPBMode)GlobalData.battleModifier.isPBMode = true;
        if(selected.enemyHpRate!=0)GlobalData.battleModifier.enemyHpRate = 100+selected.enemyHpRate;
        if(selected.enemyAtkRate!=0)GlobalData.battleModifier.enemyAtkRate = 100+selected.enemyAtkRate;

        UserData.data.selectedChapter = selectedChapter;
        UserData.data.selectedStage = selected.id;
        UserData.data.selectedMode = mode;

        UserData.Save();

        LoadingMask.ins.LoadSceneAsync("BattleScene");
    }

    public void Back(){
        LoadingMask.ins.LoadSceneAsync("MainScene");
    }

    public void preMode(){
        this.mode = (mode+1)%2;
        txtMode.text = I18N.instance.getValue(modeName[mode]);
        txtModeDesc.text = I18N.instance.getValue(modeDesc[mode]);
    }

    public void nextMode(){
        this.mode = (mode+1)%2;
        txtMode.text = I18N.instance.getValue(modeName[mode]);
        txtModeDesc.text = I18N.instance.getValue(modeDesc[mode]);
    }

    public void preDifficult(){
        this.difficult = difficult-1;
        txtDifficult.text = string.Format(I18N.instance.getValue("@UI.Difficult"),difficult,3);
        if(difficult==0)btnPreDifficult.interactable = false;
        btnNextDifficult.interactable = true;
        txtDifficultDesc.text = I18N.instance.getValue(difficultDesc[difficult]);
    }

    public void nextDifficult(){
        this.difficult = difficult+1;
        txtDifficult.text = string.Format(I18N.instance.getValue("@UI.Difficult"),difficult,3);
        if(difficult==3 || difficult>=UserData.data.stageProgress[selectedChapter+"-"+selected.id])btnNextDifficult.interactable = false;
        btnPreDifficult.interactable = true;
        txtDifficultDesc.text = I18N.instance.getValue(difficultDesc[difficult]);
    }
}
