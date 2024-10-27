using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BattleEngine 
{
    //单位列表
    public List<BaseUnit> unitList = new List<BaseUnit>();
    //子弹列表
    public List<BaseBullet> bulletList = new List<BaseBullet>();
    //刷怪点列表
    public List<MonsterGenerator> monsterGeneratorList = new List<MonsterGenerator>();
    //芯片背包
    public List<BaseChip> chipList = new List<BaseChip>();

    public int seed;
    //金币
    public int coin;

    public int wave;
    public FP time;
    public bool isDay;
    public BaseMonster boss;
    public bool isBossBattle = false;

    public int nightTime;

    public void Init(){
        if(!GlobalData.isLoadGame)wave = 1;
        time = 0;
        isDay = true;
        if(!GlobalData.isLoadGame){
            seed = (int)(DateTime.Now.ToFileTime()%65536);
        }
        TSRandom.Init(seed);
        if(!GlobalData.isLoadGame)coin = Configs.commonConfig.startMoney;
        if(BattleManager.ins.gameMode<=1){
            BattleMap.ins.LoadMap(Configs.stageConfig.chapters[GlobalData.selectedChapter].stages[GlobalData.selectedStage].mapName); 
        }
        else if(BattleManager.ins.gameMode==2){
            BattleMap.ins.LoadRandMap("field"); 
        }else if(BattleManager.ins.gameMode==3){
            BattleMap.ins.LoadRandMap("grass"); 
        }
        nightTime =  Configs.commonConfig.nightTime+BattleMap.ins.mapData.waveSecondOffset;

        chipList.Add(ObjectFactory.CreateChip("Split"));
        chipList.Add(ObjectFactory.CreateChip("Drain"));
        chipList.Add(ObjectFactory.CreateChip("DamageTraining"));

    }

    public void Start(){
        SwitchDay();   
    }

    public void Step(FP dt){
        BattleMap.ins.Step(dt);

        time += dt;

        if(isBossBattle){
            if(boss==null || boss.isDead){
                for(var i = unitList.Count-1;i>=0;i--){
                    if(unitList[i].type != UnitType.Building && unitList[i].team == TeamType.Enemy){
                       ((BaseMonster) unitList[i]).Retreat();
                    }
                }
                if(BattleManager.ins.gameMode<=1 && wave == BattleMap.ins.mapData.wave){
                    BattleManager.ins.Win();
                    return;
                }
                else{
                    isBossBattle = false;
                    BattleMap.ins.PlayBgm();
                    NightEnd();
                    SwitchDay();
                    BattleManager.ins.WavePass();
                }
            }

            foreach(var monsterGenerator in monsterGeneratorList){
                monsterGenerator.Step(dt);
            }
        }
        else{
            if(isDay){
                if(time > Configs.commonConfig.dayTime){
                    SwitchNight();
                }
            }
            else{
                if(time > nightTime){
                    NightEnd();
                    SwitchDay();
                    BattleManager.ins.WavePass();
                }
                else{
                    foreach(var monsterGenerator in monsterGeneratorList){
                        monsterGenerator.Step(dt);
                    }
                }
            }
        }



        for(var i=0;i<unitList.Count;i++){
            var unit = unitList[i];
            if(!unit.isDead){
                unit.Step(dt);
            }
        }
        for(var i=0;i<bulletList.Count;i++){
            var bullet =  bulletList[i];
            if(!bullet.isMissed){
                bullet.Step(dt);
            }
        }

        foreach(var techChip in BattleManager.ins.techChipList){
            techChip.chipStatic.OnStepGlobal(dt);
        }

        for(var i = monsterGeneratorList.Count-1;i>=0;i--){
            if(monsterGeneratorList[i].isDone){
                RemoveMonsterGenerator(monsterGeneratorList[i]);
            }
        }
        for(var i = unitList.Count-1;i>=0;i--){
            if(unitList[i].isDead){
                if(unitList[i].type == UnitType.Building)
                    BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(unitList[i].position.x,unitList[i].position.z))] = 0; 
                else
                    RemoveUnit(unitList[i]);
            }
        }
        for(var i = bulletList.Count-1;i>=0;i--){
            if(bulletList[i].isMissed){
                RemoveBullet(bulletList[i]);
            }
        }

        if(wave == BattleMap.ins.mapData.wave && !isDay){
            if(monsterGeneratorList.Count == 0){
                bool noEnemy = true;
                foreach(var unit in unitList){
                    if(unit.team == TeamType.Enemy){
                        noEnemy = false;
                        break;
                    }
                }
                if(BattleManager.ins.gameMode<=1){
                    if(noEnemy)BattleManager.ins.Win();
                }
            }
        }
    }

    public void SwitchNight(){
        time = 0;
        isDay = false;
        BattleMap.ins.OnNightStart();
        BattleManager.ins.SwitchNight();
    }

    public void NightEnd(){
        for(var i = 0;i<unitList.Count;i++){
            unitList[i].OnNightEnd();
        }
        time = 0;
        isDay = true;
        wave++;
        coin+=50+BattleMap.ins.mapData.waveCoinOffset;
    }

    public void SwitchDay(){
        for(var i = monsterGeneratorList.Count-1;i>=0;i--){
            RemoveMonsterGenerator(monsterGeneratorList[i]);
        }
        if(wave > BattleMap.ins.mapData.wave && BattleManager.ins.gameMode<=1){
            for(var i = unitList.Count-1;i>=0;i--){
                if(unitList[i].type != UnitType.Building && unitList[i].team == TeamType.Enemy){
                   ((BaseMonster) unitList[i]).Retreat();
                }
            }
            BattleManager.ins.Win();
            return;
        }

        if(!BattleManager.ins.battleModifier.noDayRest){
            foreach(var bullet in bulletList){
                bullet.isMissed = true;
            }
        }

        BattleMap.ins.OnDayStart();
        BattleMap.ins.PutWaveMonster(wave);
        BattleManager.ins.SwitchDay();

        for(var i = unitList.Count-1;i>=0;i--){
            if(unitList[i].type == UnitType.Building){
                if(unitList[i].isDead){
                    ((BaseBuild)unitList[i]).Rebuild();
                    if(!((BaseBuild)unitList[i]).isTrap)BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(unitList[i].position.x,unitList[i].position.z))] = 1; 
                }
                else{
                    unitList[i].hp = unitList[i].GetMaxHp();
                }
            }
            else{
                if(!BattleManager.ins.battleModifier.noDayRest && unitList[i].team == TeamType.Enemy){
                    ((BaseMonster) unitList[i]).Retreat();
                }
            }
        }

        for(var i = 0;i<unitList.Count;i++){
            unitList[i].OnDayStart();
        }

        if(BattleManager.ins.battleModifier.noDayRest){
            SwitchNight();
        }
    }

    /*添加刷兵点*/
    public void AddMonsterGenerator(MonsterGenerator generator){
        monsterGeneratorList.Add(generator);
    }

    /*添加单位*/
    public void AddUnit(BaseUnit unit){
        unitList.Add(unit);
    }

    /*添加子弹*/
    public void AddBullet(BaseBullet bullet){
        bulletList.Add(bullet);
    }

    public void AddChip(BaseChip chip){
        chipList.Add(chip);
    }

    public void AddCoin(int c){
    	this.coin+=c;
        
        AchievementManager.OnGetMoney(coin);
    }

    /*摆放建筑*/
    public void PutBuild(BaseUnit build,int x,int y){
        if(((BaseBuild)build).data.isNaval)
            build.Enter(new TSVector(x,-1,y), TeamType.Self);
        else
            build.Enter(new TSVector(x,0,y), TeamType.Self);
        build.view.transform.SetParent(BattleMap.ins.transform,false);
        if(((BaseBuild)build).data.isNaval)
            build.view.transform.localPosition = new Vector3(x,-1,y);
        else
            build.view.transform.localPosition = new Vector3(x,0,y);

        if(!build.isTrap)BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(x,y)] = 1;
    }

    public void SellBuild(BaseBuild build){
        var priceRate = 100;
        for(var i=0;i<build.level;i++){
            priceRate += Configs.commonConfig.upgradeCostRate[i];
        }
        coin += build.data.cost*priceRate/100/2;

        BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(build.position.x,build.position.z))] = 0;

        unitList.Remove(build);
    }

    public void EquipChip(BaseBuild build,BaseChip chip){
		chipList.Remove(chip);
        build.AddChip(chip);
    }

    /*移除刷兵点*/
    public void RemoveMonsterGenerator(MonsterGenerator generator){
        monsterGeneratorList.Remove(generator);
        generator.view.Removed();
    }


    /*移除单位*/
    public void RemoveUnit(BaseUnit unit){
        if(unit.type == UnitType.Building)BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(new Coord(unit.position.x,unit.position.z))] = 0; 
        
        unitList.Remove(unit);
    }

    /*移除子弹*/
    public void RemoveBullet(BaseBullet bullet){
        bulletList.Remove(bullet);
    }

    public void EnterBossBattle(BaseMonster monster){
        isBossBattle = true;
        this.boss = monster;
    }

    public void Revive(){
        for(var i = monsterGeneratorList.Count-1;i>=0;i--){
            RemoveMonsterGenerator(monsterGeneratorList[i]);
        }
        for(var i = unitList.Count-1;i>=0;i--){
            if(unitList[i].type != UnitType.Building && unitList[i].team == TeamType.Enemy){
               ((BaseMonster) unitList[i]).Retreat();
            }
        }

        time = 0;
        isDay = true;
        SwitchDay();
    }
}
