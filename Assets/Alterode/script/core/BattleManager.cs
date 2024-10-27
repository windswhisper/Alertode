using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using UnityEngine.UI;
using Honeti;

[Serializable]
public class TechChipData{
    public string buildID;
    public string chipID;
    public BaseChip chipStatic;
    public int exPara;

    public TechChipData(string buildID,string chipID){
        this.buildID = buildID;
        this.chipID = chipID;
        this.chipStatic = ObjectFactory.CreateChip(chipID);
    }

    public void InitChipStatic(){
        this.chipStatic = ObjectFactory.CreateChip(chipID);
        if(chipStatic.HasExPara()){
            chipStatic.SetExtraPara(exPara);
        }
    }

    public void ExportPara(){
        if(chipStatic.HasExPara()){
            exPara = chipStatic.GetExtraPara();
        }
    }
}

public class BattleManager : MonoBehaviour
{
    //静态实例
    public static BattleManager ins;

    public WaveProgress waveProgress;
    public BuildDetailPanel buildDetailPanel;
    public MonsterGenDetailPanel monsterGenDetailPanel;
    public GuidePanel guidePanel;
    public Transform selector;
    public Transform selectorRange;
    public GameObject chipDropping;
    public Text txtCoin;
    public BattleInput battleInput;
    public TechPanel techPanel;
    public WinPanel winPanel;
    public FailPanel failPanel;
    public SurviveFailPanel surviveFailPanel;
    public SettingPanel settingPanel;
    public GameObject wavePassTips;
    public Transform warningLayer;
    public GameObject hpBarPrefab;
    public Transform hpBarLayer;
    public Transform hitLabelLayer;
    public AudioSource soundBuild;
    public AudioSource soundHero;
    public UltimateSkillShow ultimateSkillShow;
    public GameObject fakeEffect;
    public GameObject btnTech;

    [HideInInspector]
    public BattleEngine engine;
    [HideInInspector]
    public BattleModifier battleModifier;

    FP frameDelta;
    int gameSpeed;
    public bool isPause = false;
    public bool isPause2 = false;
    public bool isGameOver = false;
    [HideInInspector]
    public bool isRevived = false;

    public int gameMode = 0;
    public int difficult = 0;
    [HideInInspector]
    public int techLevel = 0;
    [HideInInspector]
    public List<TechChipData> techChipList = new List<TechChipData>();


    public bool isLoadGame = false;
    int originSpeed = 1;

    int skipFrameCount = 0;

    public int reviveCount = 0;

    [HideInInspector]
    public bool noBuildLose = true;

    void Awake(){
        ins = this;
        gameSpeed = 1;
        frameDelta = new FP(1)/Configs.commonConfig.frameRate;
        ObjectPool.Init();

        gameMode = GlobalData.selectedMode;
        if(GlobalData.selectedSurviveMode!=0)gameMode=GlobalData.selectedSurviveMode;

        engine = new BattleEngine();

        if(GlobalData.isLoadGame){
            isLoadGame = true;
            SaveGameData savDat = SaveGame.Load(GlobalData.saveGameType);
            LoadGame(savDat);
        }
        else{
            difficult = GlobalData.selectedDifficult;
            battleModifier = GlobalData.battleModifier;
            if(GlobalData.selectedChapter == 0 && gameMode<2){
                gameMode = 0;
                difficult = 0;
            }
            if(battleModifier.isPBMode){
                battleModifier.isTDMode = true;
                battleModifier.noDayRest = true;
                battleModifier.nagetiveBuildTag.Add("Barrier");
                difficult = 0;
            }
            engine.Init();
        }
    }

    void LoadGame(SaveGameData savDat){
        gameMode = savDat.gameMode;
        GlobalData.selectedDifficult = difficult = savDat.difficult;
        if(gameMode<2){
            GlobalData.selectedMode = savDat.gameMode;
            GlobalData.selectedDifficult = savDat.difficult;
            GlobalData.selectedChapter = savDat.chapterId;
            GlobalData.selectedStage = savDat.stageId;
        }
        else{
            GlobalData.selectedSurviveMode = savDat.gameMode;
        }

        BuildBar.ins.SetHeroId(savDat.heroId);
        BuildBar.ins.FromIdList(savDat.buildBarIdList);
        engine.seed = savDat.seed;
        engine.Init();
        engine.coin = savDat.coin;
        engine.wave = savDat.wave;
        GlobalData.battleModifier = battleModifier = savDat.battleModifier;

        techChipList = savDat.techChipList;
        foreach(var techChip in techChipList){
            techChip.InitChipStatic();
        }

        foreach(var mapBuild in savDat.mapBuildList){
            var build = PutBuild(mapBuild.buildId,mapBuild.x,mapBuild.y,mapBuild.isHero,true);
            build.FromData(mapBuild);
        }
    }

    void InitTech(){
        if(gameMode == 0){
            foreach(var build in UserData.data.buildsCollect){
                if(!build.isUsing)continue;
                var buildData = Configs.GetBuild(build.buildId);
                for(var i=0;i<build.chipSlotCount;i++){
                    if(build.GetChipSlot(i)!=0){
                        AddNewTech(build.buildId,buildData.defaultChips[i*2+build.GetChipSlot(i)-1]);
                    }
                }
            }
        }
    }

    /*初始化*/
    void Start()
    {
        if(!GlobalData.isLoadGame){
            InitTech();
        }
        else{
            GlobalData.isLoadGame = false;
        }

        engine.Start();

        var cameraCenter = new Vector3(0,0,0);
        var mgCenter = new Vector3(0,0,0);
        foreach(var p in BattleMap.ins.basePoints){
            cameraCenter+=new Vector3(p.x,0,p.y);
        }
        cameraCenter/=BattleMap.ins.basePoints.Count;
        battleInput.cameraHandler.position = cameraCenter;

        if(UserData.data.guideProgress == 0 && gameMode == 0){
            Invoke("ShowGuide",3);
        }
        if(UserData.data.guideProgress == 4 && gameMode == 0){
            Invoke("ShowGuide",3);
        }
    }

    void Update(){
        txtCoin.text = ""+engine.coin;
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(!settingPanel.gameObject.activeInHierarchy){
                settingPanel.Show();
                Pause();
            }
            else{
                settingPanel.Hide();
                Resume();
            }
        }
    }

    public void ShowGuide(){
        if(UserData.data.guideProgress == 0){
            BuildBar.ins.FlashItem("Miner");
        }
        if(UserData.data.guideProgress == 1){
            BuildBar.ins.FlashItem("Cannon");
        }
        if(UserData.data.guideProgress == 3){
            BuildBar.ins.FlashItem("Misl");
        }
        if(UserData.data.guideProgress == 4){
            BuildBar.ins.FlashItem("Gunner");
        }
        guidePanel.Show();
    }

    public void Pause(){
        isPause = true;
    }

    public void Resume(){
        isPause = false;
    }

    public void SetGameSpeed(int speed){
        gameSpeed = speed;
    }

    public int GetGameSpeed(){
        if(isPause || isPause2)return 0;
        return gameSpeed;
    }

    /*定时更新*/
    void FixedUpdate()
    {
        if(isPause || isGameOver)return;

        // if(PlaySetting.FXLevel<3){
        //     skipFrameCount++;
        //     if(skipFrameCount<4-PlaySetting.FXLevel){
        //         return;
        //     }
        //     else{
        //         skipFrameCount = 0;
        //     }
        // }

        // if((4-PlaySetting.FXLevel)*GetGameSpeed()>3){
            // for(var i=0;i<GetGameSpeed();i++){
                // if(isPause || isGameOver)return;
                engine.Step(frameDelta*GetGameSpeed());
            // }
        // }
        // else{
        //     engine.Step(frameDelta*(4-PlaySetting.FXLevel)*GetGameSpeed());
        // }
    }

    public void SwitchNight(){
        waveProgress.SwitchNight();
        if(gameMode == 0){
            if(UserData.data.guideProgress == 6 && engine.wave == 4){
                Invoke("ShowGuide",30/GetGameSpeed());
            }
        }
    }
    public void WavePass(){
        wavePassTips.SetActive(true);
    }
    public void SwitchDay(){
        if(engine.wave>1)SaveGame.Save();

        waveProgress.SwitchDay();
        if(gameMode >= 1 && !isRevived){
            techPanel.Show(engine.wave-1);
            if(engine.wave>=3)btnTech.SetActive(true);    
        }
        else{
            if(UserData.data.guideProgress == 2){
                Invoke("ShowGuide",3);
            }
            if(UserData.data.guideProgress == 3 && engine.wave == 3){
                Invoke("ShowGuide",4);
            }
            if(UserData.data.guideProgress == 5 && engine.wave == 2){
                Invoke("ShowGuide",3);
            }
        }
        isRevived = false;
    }

    public void AddNewTech(string buildID,string chipID){
        var chipData = Configs.GetChip(chipID);
        if(chipData.prerequisite!=""){
            for(var i=0;i<techChipList.Count;i++){
                if(techChipList[i].buildID==buildID && techChipList[i].chipID == chipData.prerequisite){
                    techChipList.Remove(techChipList[i]);
                    break;
                }
            }
        }
        techChipList.Add(new TechChipData(buildID,chipID));

        foreach(var unit in engine.unitList){
            if(unit.type == UnitType.Building){
                var build = (BaseBuild)unit;
                if(build.name == buildID){
                    if(chipData.prerequisite!=""){
                        build.RemoveChip(chipData.prerequisite);
                    }
                    build.AddChip(ObjectFactory.CreateChip(chipID));
                }
            }
        }

        BuildBar.ins.RefreshCost();
    }

    /*创建建筑*/
    public BaseBuild CreateBuild(string name){
        var build = ObjectFactory.CreateBuild(name);
        var view = ObjectFactory.CreateBuildView(build);
        engine.AddUnit(build);

        if(UserData.data.guideProgress == 1 && name == "Miner"){
            Invoke("ShowGuide",3);
        }

        return build;
    }

    /*创建英雄*/
    public BaseBuild CreateHero(string name){
        var hero = ObjectFactory.CreateHero(name);
        var view = ObjectFactory.CreateHeroView(hero);
        engine.AddUnit(hero);
        return hero;
    }

    /*创建怪物*/
    public BaseMonster CreateMonster(string name,bool isBoss){
        var monster = ObjectFactory.CreateMonster(name);
        monster.isBoss = isBoss;
        var view = ObjectFactory.CreateMonsterView(monster);
        engine.AddUnit(monster);

        if(difficult>=1){
            monster.hp = monster.maxHp = monster.maxHp*(100+Configs.commonConfig.difficultParas[1])/100;
        }
        if(difficult>=1){
            foreach(var weapon in monster.weapons){
                weapon.damage = weapon.damage*(100+Configs.commonConfig.difficultParas[1])/100;
            }
        }
        // if(difficult>=4){
        //     monster.speed = monster.speed*(100+Configs.commonConfig.difficultParas[4])/100;
        //     foreach(var weapon in monster.weapons){
        //         weapon.fireSpeed = weapon.fireSpeed*(100+Configs.commonConfig.difficultParas[4])/100;
        //     }
        // }

        if(gameMode==1){
            int nerfRate = 0;
            if(engine.wave<15){
                nerfRate = -(10-engine.wave)*4;
            }
            else{
                nerfRate = 20;
            }
            monster.hp = monster.maxHp = monster.maxHp*(100+nerfRate)/100;
            foreach(var weapon in monster.weapons){
                weapon.damage = weapon.damage*(100+nerfRate)/100;
            }
        }
        else if(gameMode==2){
            var n = (engine.wave-1)/10;
            int buffRate = n*(n+1)*160;
            int atkBuffRate = n*70;

            if(engine.wave>40){
                var m=(engine.wave-1)/10-3;
                for(var i=0;i<m;i++){
                    buffRate*=2;
                    atkBuffRate*=2;
                }
            }
            
            monster.hp = monster.maxHp = monster.maxHp*(100+buffRate)/100;
            if(monster.moveType != MoveType.Fly && engine.wave>3)monster.hp = monster.maxHp = monster.maxHp*2;
            if(monster.moveType == MoveType.Fly )monster.hp = monster.maxHp = monster.maxHp/2;
            if(monster.isBoss)monster.hp = monster.maxHp = monster.maxHp/2;
            foreach(var weapon in monster.weapons){
                weapon.damage = weapon.damage/2*(100+atkBuffRate)/100;
            }
        }
        else if(gameMode==3){
            var n = (engine.wave-1)/10;
            int buffRate = n*(n+1)*160;
            int atkBuffRate = n*70;
            
            if(engine.wave>40){
                var m=(engine.wave-1)/10-3;
                for(var i=0;i<m;i++){
                    buffRate*=2;
                    atkBuffRate*=2;
                }
            }
            
            monster.hp = monster.maxHp = monster.maxHp*(100+buffRate)/100;
            if(monster.isBoss)monster.hp = monster.maxHp = monster.maxHp/2;
            foreach(var weapon in monster.weapons){
                weapon.damage = weapon.damage*(100+atkBuffRate)/100;
            }
        }

        monster.hp = monster.maxHp = monster.maxHp*battleModifier.enemyHpRate/100;
        foreach(var weapon in monster.weapons){
            weapon.damage = weapon.damage*battleModifier.enemyAtkRate/100;
        }

        return monster;
    }

    /*创建子弹*/
    public BaseBullet CreateBullet(string name){
        var bullet = ObjectFactory.CreateBullet(name);
        engine.AddBullet(bullet);
        if(PlaySetting.FXLevel<=0)return bullet;
        var view = ObjectFactory.CreateBulletView(bullet);
        view.gameObject.SetActive(false);
        return bullet;
    }

    /*创建Buff*/
    public BaseBuff CreateBuff(string name,string source){
        var buff = ObjectFactory.CreateBuff(name,source);
        if(PlaySetting.FXLevel<=1)return buff;
        var view = ObjectFactory.CreateBuffView(buff);
        return buff;
    }

    /*创建刷兵点*/
    public MonsterGenerator CreateMonsterGenerator(MonsterGeneratorData data){
        var monsGen = ObjectFactory.CreateMonsterGenerator(data);
        var view = ObjectFactory.CreateMonsterGeneratorView(monsGen);
        engine.AddMonsterGenerator(monsGen);

        if(difficult>=3){
            monsGen.repeatCount = monsGen.repeatCount*(100+Configs.commonConfig.difficultParas[5])/100;
            monsGen.deltaTime = monsGen.deltaTime*100/(100+Configs.commonConfig.difficultParas[5]);
            monsGen.repeatDelta = monsGen.repeatDelta*100/(100+Configs.commonConfig.difficultParas[5]);
        }

        return monsGen;
    }

    /*创建随机芯片*/
    public void CreateChip(TSVector position){
        if(gameMode != -1)return;

        var chip = ObjectFactory.CreateChip(Utils.RandomChip());
        var dropEffect = Instantiate(chipDropping);
        dropEffect.GetComponent<ChipDropping>().Init(chip);
        var p = Utils.TSVecToVec3(position);
        p.y=0;
        dropEffect.transform.position = p;
        engine.AddChip(chip);
    }

    /*创建指定芯片*/
    public void CreateChip(TSVector position,string name){
        if(gameMode != -1)return;
        
        var chip = ObjectFactory.CreateChip(name);
        var dropEffect = Instantiate(chipDropping);
        dropEffect.GetComponent<ChipDropping>().Init(chip);
        dropEffect.transform.position = Utils.TSVecToVec3(position)+new Vector3(UnityEngine.Random.value/2,0,UnityEngine.Random.value/2);
        engine.AddChip(chip);
    }

    public void CreateDropCoin(TSVector position,int coin){
        engine.AddCoin(coin);
    }

    /*摆放建筑*/
    public BaseBuild PutBuild(string buildId,int x,int y,bool isHero,bool isFree){
        BuildData data;
        if(isHero)
            data = Configs.GetHero(buildId);
        else
            data = Configs.GetBuild(buildId);
        var cost = data.cost;
        if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;

        var costMultipleFactor = 0;
        foreach(var techChip in techChipList){
            if(techChip.buildID == buildId){
                costMultipleFactor += techChip.chipStatic.OnGetSelfCostMutipleFactorGlobal();
            }
            costMultipleFactor += techChip.chipStatic.OnGetCostMutipleFactorGlobal();
        }

        cost = cost*(100+costMultipleFactor)/100;
        if(cost<0)cost=0;

        if( !isFree){
            if(cost > engine.coin){
                ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoCoin"));
                return null;
            }

            if(!BattleMap.ins.IsTileCanBuild(x,y,data.isNaval,data.isMiner)){
                ToastBar.ShowMsg(I18N.instance.getValue("@UI.CanNotBuild"));
                return null;
            }

            if(battleModifier.isPBMode && !data.isTrap && BattleMap.ins.IsBuildBlocking(x,y)){
                ToastBar.ShowMsg(I18N.instance.getValue("@UI.ShouldRemainRoad"));
                return null;
            }

            foreach(var unit in engine.unitList){
                if(TSMath.Abs(unit.position.x - x) < 0.5f && TSMath.Abs(unit.position.z - y) < 0.5f && (unit.type == UnitType.Building || (unit.type != UnitType.Building && unit.IsOnGround()))){
                    if(!data.isTrap || unit.type != UnitType.Monster){
                        ToastBar.ShowMsg(I18N.instance.getValue("@UI.AlreadyOccupied"));
                        return null;
                    }
                }
            }
        }

        if(!isFree)engine.AddCoin(-cost);
        BaseBuild build;
        if(isHero)
        {
            build = CreateHero(buildId);
            BuildBar.ins.HideHero();
            if(!isFree)soundHero.Play();
        }
        else{
            build = CreateBuild(buildId);
            if(!isFree)BuildBar.ins.EnterCd(buildId);
            if(!isFree)soundBuild.Play();
            AchievementManager.OnBuild();
        }

        engine.PutBuild(build,x,y);
        if(buildId !="Base"){
            if(battleModifier.isPBMode && !data.isTrap){
                BattleMap.ins.ChangeTileSP(x,y,TerrainType.Wall);
            }
            if(battleModifier.isTDMode)build.AddBuff(CreateBuff("Inviso",""),1);
        }
        
        if(!isFree){
            foreach(var techChip in techChipList){
                if(techChip.buildID == buildId){
                    techChip.chipStatic.OnBoughtGlobal(build);
                }
                techChip.chipStatic.OnBuyBuildGlobal(build);
            }
        }

        if(!isHero){
            foreach(var tech in techChipList){
                if(tech.buildID == buildId){
                    build.AddChip(ObjectFactory.CreateChip(tech.chipID));
                }
            }
        }

        if(!isHero&&!isFree)
        {
            BuildBar.ins.EnterCd(buildId);
        }

        return build;
    }

    public bool UpgradeBuild(BaseBuild build){
        if(build.level > Configs.commonConfig.buildMaxLevel){
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.UpgradeMax"));
            return false;
        }

        var cost = build.data.cost*Configs.commonConfig.upgradeCostRate[build.level]/100;
        if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;

        var costMultipleFactor = 0;
        foreach(var techChip in BattleManager.ins.techChipList){
            if(techChip.buildID == build.data.name){
                costMultipleFactor += techChip.chipStatic.OnGetSelfCostMutipleFactorGlobal();
            }
            costMultipleFactor += techChip.chipStatic.OnGetCostMutipleFactorGlobal();
        }

        cost = cost*(100+costMultipleFactor)/100;

        if(cost<0)cost = 0;

        if(engine.coin >= cost){
            engine.AddCoin(-cost);
            build.LevelUp();

            foreach(var techChip in techChipList){
                techChip.chipStatic.OnUpgradeBuildGlobal(build);
            }

            CheckMaxBuildAchieve();
            return true;
        }
        else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoCoin"));
        }
        return false;
    }

    public bool UpgradeBuilds(BaseBuild build){
        var cost = build.data.cost*Configs.commonConfig.upgradeCostRate[build.level]/100;
        if(BattleManager.ins.difficult >= 2)cost = cost*(100+Configs.commonConfig.difficultParas[3])/100;

        var costMultipleFactor = 0;
        foreach(var techChip in BattleManager.ins.techChipList){
            if(techChip.buildID == build.data.name){
                costMultipleFactor += techChip.chipStatic.OnGetSelfCostMutipleFactorGlobal();
            }
            costMultipleFactor += techChip.chipStatic.OnGetCostMutipleFactorGlobal();
        }

        cost = cost*(100+costMultipleFactor)/100;

        if(cost<0)cost = 0;

        int count = 0;
        foreach(var unit in engine.unitList){
            if(unit.team == TeamType.Self && unit.type == UnitType.Building && ((BaseBuild)unit).data.name == build.data.name && ((BaseBuild)unit).level == build.level){
                count++;
            }
        }
        cost*=count;

        if(engine.coin >= cost){
            int level = build.level;
            engine.AddCoin(-cost);
            foreach(var unit in engine.unitList){
                if(unit.team == TeamType.Self && unit.type == UnitType.Building && ((BaseBuild)unit).data.name == build.data.name && ((BaseBuild)unit).level == level){
                    ((BaseBuild)unit).LevelUp();

                    foreach(var techChip in techChipList){
                        techChip.chipStatic.OnUpgradeBuildGlobal((BaseBuild)unit);
                    }

                }
            }
            CheckMaxBuildAchieve();
            return true;
        }
        else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoCoin"));
        }
        return false;
    }
    public void SellBuild(BaseBuild build){
        engine.SellBuild(build);
        var c = new Coord(build.position.x,build.position.z);
        if(battleModifier.isPBMode)BattleMap.ins.ChangeTileSP(c.x,c.y,TerrainType.Ground);
        UnselectMapObject();
    }

    public void CheckMaxBuildAchieve(){
        int count = 0;
        foreach(var unit in engine.unitList){
            if(unit.team == TeamType.Self && unit.type == UnitType.Building && !((BaseBuild)unit).isHero && ((BaseBuild)unit).level == Configs.commonConfig.buildMaxLevel){
                count++;
            }
        }

        AchievementManager.OnOwnMaxLevelBuilding(count);
    }

    /*生成刷怪点*/
    public void PutMonsterGenerator(MonsterGeneratorData data){
        var monsGen = CreateMonsterGenerator(data);
        monsGen.view.transform.SetParent(BattleMap.ins.transform,false);
        if(BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByCoord(data.x,data.y)] == TerrainType.Water ){
            monsGen.view.transform.localPosition = new Vector3(data.x,-0.4f,data.y);
        }
        else{
            monsGen.view.transform.localPosition = new Vector3(data.x,0,data.y);
        }
    }

    /*生成怪物*/
    public BaseMonster PutMonster(string monsterId,int x,int y,bool isBoss){
        var monster = CreateMonster(monsterId,isBoss);
        monster.view.transform.SetParent(BattleMap.ins.transform,false);
        if(BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByCoord(x,y)] == TerrainType.Water && monster.moveType!=MoveType.Hover){
            monster.Enter(new TSVector(x,-1,y), TeamType.Enemy);
            monster.view.transform.localPosition = new Vector3(x,-1,y);
        }
        else{
            monster.Enter(new TSVector(x,0,y), TeamType.Enemy);
            monster.view.transform.localPosition = new Vector3(x,0,y);
        }
        if(isBoss){
            EnterBossBattle(monster);
        }
        return monster;
    }

    public void EnterBossBattle(BaseMonster monster){
        waveProgress.EnterBossBattle(monster);
        engine.EnterBossBattle(monster);

        VolumeManager.ins.PlayBgm("Epic Heroic Battle - Master Logan");
    }

    /*生成友军随从*/
    public BaseMonster PutAlly(string allyId,int x,int y){
        var ally = CreateMonster(allyId,false);
        ally.view.transform.SetParent(BattleMap.ins.transform,false);
        if(BattleMap.ins.staticTerrainSP[BattleMap.ins.IndexByCoord(x,y)] == TerrainType.Water && ally.moveType!=MoveType.Hover){
            ally.Enter(new TSVector(x,-1,y), TeamType.Self);
            ally.view.transform.localPosition = new Vector3(x,-1,y);
        }
        else{
            ally.Enter(new TSVector(x,0,y), TeamType.Self);
            ally.view.transform.localPosition = new Vector3(x,0,y);
        }
        return ally;
    }

    public void SelectMapObject(int x,int y){
        foreach(var unit in engine.unitList){
            if(unit.type == UnitType.Building && !unit.isDead){

                if(!((BaseBuild)unit).IsSelectable()){
                    continue;
                }

                var c = new Coord(unit.position.x,unit.position.z);
                if(c.x == x && c.y==y){
                    buildDetailPanel.Show((BaseBuild)unit);
                    selector.gameObject.SetActive(true);
                    selector.position = new Vector3(c.x,unit.view.transform.position.y,c.y);
                    return;
                }
            }
        }

        foreach(var monsterGen in engine.monsterGeneratorList){
            if(monsterGen.data.x == x && monsterGen.data.y == y){
                monsterGenDetailPanel.Show(monsterGen);
                selector.gameObject.SetActive(true);
                selector.position = new Vector3(x,monsterGen.view.transform.position.y,y);
                return;
            }
        }


        BattleManager.ins.UnselectMapObject();
    }

    public void UnselectMapObject(){
        selector.gameObject.SetActive(false);
        buildDetailPanel.Hide();
        monsterGenDetailPanel.Hide();
    }

    public void SelectHero(){
        BattleManager.ins.UnselectMapObject();

        foreach(var unit in engine.unitList){
            if(unit.type == UnitType.Building && !unit.isDead){
                if(((BaseBuild)unit).isHero && !((BaseHero)unit).isMoving){
                    buildDetailPanel.Show((BaseBuild)unit);
                    selector.gameObject.SetActive(true);
                    selector.position = new Vector3(unit.view.transform.position.x,0,unit.view.transform.position.z);
                    break;
                }
            }
        }
    }

    public GameObject PlayEffect(string effectName,Vector3 position){
        if(PlaySetting.FXLevel<=1)return fakeEffect;
        var effect = ObjectFactory.CreateEffect(effectName);
        effect.transform.SetParent(BattleMap.ins.transform,false);
        effect.transform.position = position;
        return effect;
    }


    public void PlayCoinEffect(int amount,Vector3 position){
        var effect = GameObject.Instantiate(Resources.Load<GameObject>("prefab/effect/EffectMoney"));
        effect.GetComponent<EffectMoney>().Init(amount);
        effect.transform.SetParent(BattleMap.ins.transform,false);
        effect.transform.position = position;
    }

    public void PlayGemEffect(int amount,Vector3 position){
        var effect = GameObject.Instantiate(Resources.Load<GameObject>("prefab/effect/EffectGem"));
        effect.GetComponent<EffectMoney>().Init(amount);
        effect.transform.SetParent(BattleMap.ins.transform,false);
        effect.transform.position = position;
    }

    public void PlayHitLabel(int value,int type,Vector3 position){
        if(PlaySetting.FXLevel<=2)return;
        var effect = ObjectFactory.CreateEffect("HitLabel");
        effect.GetComponent<HitLabel>().Init(value,type);
        effect.transform.SetParent(hitLabelLayer,false);
        effect.transform.position = position+new Vector3(UnityEngine.Random.value-0.5f,UnityEngine.Random.value-0.5f,UnityEngine.Random.value-0.5f);
    }

    public void Win(){
        if(isGameOver)return;
        isGameOver = true;

        SaveGame.Delete();

        StartCoroutine(WinDelay());
    }

    IEnumerator WinDelay(){
        yield return new WaitForSeconds(2);

        CameraEffect.ins.LaunchFirework();

        winPanel.Show();

        if(noBuildLose && difficult==3)AchievementManager.OnBattleWinWithNoLose();
        if(GlobalData.selectedChapter == 0){
            difficult = 3;
            GlobalData.selectedDifficult = 3;
        }
        AchievementManager.OnBattleWin();
        UserData.Save();
    }

    public void Fail(){
        if(isGameOver)return;
        isGameOver = true;

        StartCoroutine(FailDelay());
    }
    
    IEnumerator FailDelay(){
        yield return new WaitForSeconds(2);

        SaveGame.Delete();

        AchievementManager.OnBattleFail();

        if(gameMode>=2){
            surviveFailPanel.Show();
        }
        else{
            failPanel.Show();
        }
    }


    public void Revive(){
        isGameOver = false;
        isRevived = true;
        reviveCount++;
        engine.isBossBattle = false;
        engine.Revive();
        AchievementManager.OnBattleRevive();
    }

}
