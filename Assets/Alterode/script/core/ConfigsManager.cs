using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommonConfig{
	//游戏逻辑帧率
	public int frameRate;
	//开局金币
	public int startMoney;
	//白天时间，单位秒
	public int dayTime;
	//夜晚时间，单位秒
	public int nightTime;
	//芯片掉落概率*1000
	public int chipDropMPercent;
	//芯片稀有度掉落权重
	public List<int> chipDropWeight;
	//怪物飞行高度
	public int flyHeight;
	//散弹芯片分散角度
	public int blossomDegree;
	//建筑最大等级
	public int buildMaxLevel;
	//芯片槽解锁等级
	public List<int> buildEvoLevel;
	//建筑升级价格比例
	public List<int> upgradeCostRate;
	//建筑升级血量加成
	public List<int> upgradeStrengthRate;
	//建筑升级攻击加成
	public List<int> upgradeDamageRate;
	//建筑升级射速加成
	public List<int> upgradeFireSpeedRate;
	//建筑升级射程加成
	public List<int> upgradeRangeRate;
	//建筑升级采矿加成
	public List<int> upgradeMinerRate;
	//出售建筑返还比例
	public int sellPriceRate;
	//金币掉落概率*1000
	public int monsterBountyMPercent;
	//金币掉落数量
	public int monsterBountyAmount;
	//每波金币奖励
	public int waveBounty;
	//难度进阶参数，分别对应敌人血量，攻击力，数量，防御塔价格，敌人速度
	public List<int> difficultParas;
	//科技进化路线，0-防御塔，1-芯片升级
	public List<int> techPathTypes;
	//每波经验奖励
	public int waveExp;
	//每个怪物的经验奖励
	public int monsterExp;
	//英雄最大等级
	public int heroMaxLevel;
	//英雄升级经验价格比
	public List<int> upgradeExp;
	//建筑芯片插槽解锁价格
	public List<int> chipSlotPrice;
	//战争迷雾驱散半径
	public int disfogRadius;
	//进化模式科技价格
	public List<int> chipTechPrice;
	//地图格子最多容纳几个怪物
	public int crowdedMaxInCoord;
	//怪物行进随机偏差范围
	public int monsterMoveDeviation;
}

//三维向量数据类
[Serializable]
public class VectorData{
	//x坐标
	public int x;
	//y坐标
	public int y;
	//z坐标
	public int z;
}

//建筑配置
[Serializable]
public class BuildConfig{
	//建筑数据列表
	public List<BuildData> datas;
}
//建筑数据类
[Serializable]
public class BuildData{
	//建筑注册名
	public string name = "ERR-BUILD";
	//类名
	public string className = "BaseBuild";
	//建筑显示名称
	public string uiName = "@Error_Building";
	//价格
	public int cost = 10;
	//血量
	public int strength = 100;
	//体积半径*100
	public int radius = 50;
    //攻击前摇
    public int fireUp = 0;
	//武器列表
	public List<string> weapons = new List<string>();
	//被动技能列表
	public List<string> skills = new List<string>();
	//主动技能列表
	public List<string> ultimateSkills = new List<string>();
	//是否有炮塔
	public bool hasTurret = false;
	//是否是水面建筑
	public bool isNaval = false;
	//是否是矿井
	public bool isMiner = false;
	//是否是陷阱，可以通行不会被攻击
	public bool isTrap = false;
	//可以从面板中调节的类型，0-瞄准优先级，1-炮台朝向
	public int settingType = 0;
    //额外参数
    public List<int> paras;
    //标签，用于芯片分类
    public List<string> tags;
    //预装芯片
    public List<string> defaultChips;
    //不可收集
    public bool uncollectable;
}

//英雄配置
[Serializable]
public class HeroConfig{
	//英雄数据列表
	public List<HeroData> datas;
}
//英雄数据类
[Serializable]
public class HeroData:BuildData{
}
//武器配置
[Serializable]
public class WeaponConfig{
	//武器数据列表
	public List<WeaponData> datas;
}
//武器数据类
[Serializable]
public class WeaponData{
	//武器注册名
	public string name = "ERR-WEAPON";
	//类名
	public string className = "BaseWeapon";
	//伤害
	public int damage = 1;
	//射程*100
	public int range = 100;
	//最小射程*100
	public int minRange = 0;
	//射速*100
	public int fireSpeed = 100;
	//偏差范围*100
	public int scatterRadius = 0;
	//开火位置*100
	public List<VectorData> muzzles = new List<VectorData>();
	//开火倾角
	public int fireAngle = 0;
    //朝脚下开火
    public bool areaFire = false;
	//开火动画
	public string muzzleFX = "";
	//目标类型
	public int aimType = 4;
	//发射的子弹注册名
	public string bulletName = "";
}

//子弹配置
[Serializable]
public class BulletConfig{
	//子弹数据列表
	public List<BulletData> datas;
}
//子弹数据类
[Serializable]
public class BulletData{
	//子弹注册名
	public string name = "ERR-BULLET";
	//类名
	public string className = "BaseBullet";
	//子弹类型
	public int type = 0;
	//子弹半径*100
	public int radius = 0;
	//子弹飞行速度*100
	public int speed = 400;
	//子弹寿命*100
	public int life = 100;
    //爆炸半径*100
    public int explodeRadius = 0;
    //命中动画
    public string hitFX = "";
    //爆炸动画
    public string explodeFX = "";
    //是否影响敌人
    public bool affectsEnemies = true;
    //是否影响友军
    public bool affectsAllies = false;
    //是否不旋转
    public bool fixedRotate;
    //额外参数
    public List<int> paras;
}

//怪物配置
[Serializable]
public class MonsterConfig{
	//怪物数据列表
	public List<MonsterData> datas;
}
//怪物数据类
[Serializable]
public class MonsterData{
	//怪物注册名
	public string name = "ERR-MONSTER";
	//类名
	public string className = "BaseMonster";
	//显示名称
	public string uiName = "@Error_Monster";
	//血量
	public int strength = 50;
    //体积半径*100
    public int radius = 50;
    //攻击前摇
    public int fireUp = 0;
	//移动速度*100
	public int speed = 100;
	//是否拥有炮塔
	public bool hasTurret = false;
	//武器列表
	public List<string> weapons = new List<string>();
	//移动方式
	public int moveType = 0;
	//被动技能列表
	public List<string> skills = new List<string>();
    //额外参数
    public List<int> paras;
}

//地图配置
[Serializable]
public class MapConfig{
	public List<TileData> tileDatas;
	public List<MapObjectData> mapObjectDatas;
}
//地表配置
[Serializable]
public class TileData{
	//序号
	public int id;
	//显示名称
	public string uiName;
	//是否可以建造
	public bool buildable = true;
	//是否可以通行
	public bool walkable = true;
	//是否是登陆点
	public bool landing = false;
	//登陆点类型
	public int landingType = 0;
}
//地图物体配置
[Serializable]
public class MapObjectData{
	//序号
	public int id;
	//显示名称
	public string uiName;
	//是否可以建造
	public bool buildable = false;
	//是否可以通行
	public bool walkable = false;
	//是否是矿石
	public bool isOre = false;
	//是否是预置建筑
	public bool isBuild = false;
	//是否是无形物
	public bool isInviso = false;
	//是否是地图脚本
	public bool isScript = false;
	//建筑ID等额外参数
	public string paraStr = "";
}

//关卡配置
[Serializable]
public class StageConfig{
	//关卡数据列表
	public List<ChapterData> chapters;
}
//关卡数据类
[Serializable]
public class ChapterData{
	public int id;
	public string uiName;
	public List<StageData> stages;
}
//关卡数据类
[Serializable]
public class StageData{
	public int id;
	public string uiName;
	public string mapName;
	public bool isPBMode;
	public int enemyHpRate;
	public int enemyAtkRate;
}
//芯片数据类
[Serializable]
public class ChipConfig{
	public List<ChipData> datas;
}

//芯片配置
[Serializable]
public class ChipData{
	//芯片注册名
	public string name = "ERR-CHIP";
	//类名
	public string className = "BaseChip";
	//显示名称
	public string uiName = "@Error_Chip";
	//显示图像，默认为name
	public string image = "";
	//稀有度
	public int rare = 0;
	//属于英雄
	public bool hero = false;
	//全局生效
	public bool isGlobal = false;
	//参数
	public List<int> paras;
	//字符串参数
	public string namePara;
	//描述文本
	public string desc = "@Error_ChipDesc";
	//统计文本
	public string record = "";
	//需要前置芯片
	public string prerequisite = "";
	//可以升级
	public bool canUpgrade = false;
    //仅可用于指定建筑
    public List<string> ownerBuilds;
    //需要标签，建筑需拥有任意一个标签
    public List<string> requireTags;
    //排除标签，建筑不能拥有任意一个标签
    public List<string> negativeTags;
    //等级
    public int level = 0;
}

//状态数据类
[Serializable]
public class BuffConfig{
	public List<BuffData> datas;
}

//状态配置
[Serializable]
public class BuffData{
	//状态注册名
	public string name = "ERR-BUFF";
	//类名
	public string className = "BaseBuff";
	//显示名称
	public string uiName = "@Error_Buff";
	//参数
	public List<int> paras;
	//描述文本
	public string desc = "@Error_Buff";
	//是负面状态
	public bool isNegative = false;
	//是永久状态
	public bool isForever = false;
}


//被动技能数据类
[Serializable]
public class SkillConfig{
	public List<SkillData> datas;
}

//被动技能配置
[Serializable]
public class SkillData{
	//状态注册名
	public string name = "ERR-SKILL";
	//类名
	public string className = "BaseSkill";
	//显示名称
	public string uiName = "@Error_Skill";
	//参数
	public List<int> paras;
	//文本参数
	public string namePara;
	//描述文本
	public string desc = "@Error_Skill";
}

//终极技能数据类
[Serializable]
public class UltimateSkillConfig{
	public List<UltimateSkillData> datas;
}

//终极技能配置
[Serializable]
public class UltimateSkillData{
	//状态注册名
	public string name = "ERR-SKILL";
	//类名
	public string className = "BaseSkill";
	//显示名称
	public string uiName = "@Error_Skill";
	//冷却时间*100
	public int cd;
	//目标类型
	public int targetType;
	//角色动作
	public string anim;
	//文本参数
	public string namePara;
	//参数
	public List<int> paras;
	//描述文本
	public string desc = "@Error_Skill";
}

//成就数据类
[Serializable]
public class AchievementConfig{
	public List<AchievementData> datas;
}

//成就配置
[Serializable]
public class AchievementData{
	//状态注册名
	public string name = "ERR-ACHIEVE";
	//成就类型
	public int type;
	//显示名称
	public string uiName = "@Error_Achieve";
	//描述
	public string desc = "@Error_AchieveDesc";
	//文本参数
	public string namePara;
	//目标数量要求
	public int goal;
	//奖励列表
	public List<AchieveReawrdData> rewards;
}

//成就奖励配置
[Serializable]
public class AchieveReawrdData{
	//奖励类型，0-钻石，1-钢铁，2-芯片，3-英雄
	public int type;
	//奖励数量
	public int count;
	//额外参数
	public string para;
}

//全局静态配置
public class Configs{
	//通用配置
	public static CommonConfig commonConfig;
	//建筑配置
	public static BuildConfig buildConfig;
	//英雄配置
	public static HeroConfig heroConfig;
	//武器配置
	public static WeaponConfig weaponConfig;
	//怪物配置
	public static MonsterConfig monsterConfig;
	//子弹配置
	public static BulletConfig bulletConfig;
	//地图配置
	public static MapConfig mapConfig;
	//芯片配置
	public static ChipConfig chipConfig;
	//状态配置
	public static BuffConfig buffConfig;
	//技能配置
	public static SkillConfig skillConfig;
	//技能配置
	public static UltimateSkillConfig ultimateSkillConfig;
	//关卡配置
	public static StageConfig stageConfig;
	//成就配置
	public static AchievementConfig achievementConfig;

	/*按注册名获取建筑数据*/
	public static BuildData GetBuild(string name){
		foreach(var build in buildConfig.datas){
			if(build.name == name)return build;
		}
		return null;
	}
	/*按注册名获取建筑射程*/
	public static float GetBuildWeaponRange(string name){
		foreach(var build in buildConfig.datas){
			if(build.name == name) {
				if(build.weapons.Count==0)return 0f;
				return GetWeapon(build.weapons[0]).range/100f;
			}
		}
		return 0f;
	}
	/*按注册名获取英雄数据*/
	public static HeroData GetHero(string name){
		foreach(var hero in heroConfig.datas){
			if(hero.name == name)return hero;
		}
		return null;
	}
	/*按注册名获取建筑射程*/
	public static float GetHeroWeaponRange(string name){
		foreach(var hero in heroConfig.datas){
			if(hero.name == name) {
				if(hero.weapons.Count==0)return 0f;
				return GetWeapon(hero.weapons[0]).range/100f;
			}
		}
		return 0f;
	}
	/*按注册名获取武器数据*/
	public static WeaponData GetWeapon(string name){
		foreach(var weapon in weaponConfig.datas){
			if(weapon.name == name)return weapon;
		}
		return null;
	}
	/*按注册名获取技能数据*/
	public static SkillData GetSkill(string name){
		foreach(var skill in skillConfig.datas){
			if(skill.name == name)return skill;
		}
		return null;
	}
	/*按注册名获取终极技能数据*/
	public static UltimateSkillData GetUltimateSkill(string name){
		foreach(var skill in ultimateSkillConfig.datas){
			if(skill.name == name)return skill;
		}
		return null;
	}
	/*按注册名获取Buff数据*/
	public static BuffData GetBuff(string name){
		foreach(var buff in buffConfig.datas){
			if(buff.name == name)return buff;
		}
		return null;
	}
	/*按注册名获取怪物数据*/
	public static MonsterData GetMonster(string name){
		foreach(var monster in monsterConfig.datas){
			if(monster.name == name)return monster;
		}
		return null;
	}
	/*按注册名获取子弹数据*/
	public static BulletData GetBullet(string name){
		foreach(var bullet in bulletConfig.datas){
			if(bullet.name == name)return bullet;
		}
		return null;
	}
	/*按注册名获取芯片数据*/
	public static ChipData GetChip(string name){
		foreach(var chip in chipConfig.datas){
			if(chip.name == name)return chip;
		}
		return null;
	}
	/*按ID获取关卡数据*/
	public static StageData GetStage(int chapterId,int id){
		foreach(var chapter in stageConfig.chapters){
			if(chapter.id == chapterId){
				foreach(var stage in chapter.stages){
					if(stage.id == id)return stage;
				}
			}
		}
		return null;
	}
	/*按注册名获取成就数据*/
	public static AchievementData GetAchieve(string name){
		foreach(var achieve in achievementConfig.datas){
			if(achieve.name == name)return achieve;
		}
		return null;
	}
}

//配置管理器
public class ConfigsManager
{
	/*读取配置文件*/
    public static void Load()
    {	
        Configs.commonConfig = JsonUtility.FromJson<CommonConfig> (Resources.Load<TextAsset>("config/CommonConfigs").text);
        Configs.buildConfig = JsonUtility.FromJson<BuildConfig> (Resources.Load<TextAsset>("config/BuildConfigs").text);
        Configs.heroConfig = JsonUtility.FromJson<HeroConfig> (Resources.Load<TextAsset>("config/HeroConfigs").text);
        Configs.weaponConfig = JsonUtility.FromJson<WeaponConfig> (Resources.Load<TextAsset>("config/WeaponConfigs").text);
        Configs.bulletConfig = JsonUtility.FromJson<BulletConfig> (Resources.Load<TextAsset>("config/BulletConfigs").text);
        Configs.monsterConfig = JsonUtility.FromJson<MonsterConfig> (Resources.Load<TextAsset>("config/MonsterConfigs").text);
        Configs.mapConfig = JsonUtility.FromJson<MapConfig> (Resources.Load<TextAsset>("config/MapConfigs").text);
        Configs.chipConfig = JsonUtility.FromJson<ChipConfig> (Resources.Load<TextAsset>("config/ChipConfigs").text);
        Configs.buffConfig = JsonUtility.FromJson<BuffConfig> (Resources.Load<TextAsset>("config/BuffConfigs").text);
        Configs.skillConfig = JsonUtility.FromJson<SkillConfig> (Resources.Load<TextAsset>("config/SkillConfigs").text);
        Configs.ultimateSkillConfig = JsonUtility.FromJson<UltimateSkillConfig> (Resources.Load<TextAsset>("config/UltimateSkillConfigs").text);
        Configs.stageConfig = JsonUtility.FromJson<StageConfig> (Resources.Load<TextAsset>("config/StageConfigs").text);
        Configs.achievementConfig = JsonUtility.FromJson<AchievementConfig> (Resources.Load<TextAsset>("config/AchievementConfigs").text);

        foreach(var chipData in  Configs.chipConfig.datas){
        	if(chipData.image == "")chipData.image = chipData.name;
        	if(chipData.prerequisite != ""){
        		var chip = Configs.GetChip(chipData.prerequisite);
        		chip.canUpgrade = true;
        		chipData.level = chip.level+1;
        	}
        }
    }

}
