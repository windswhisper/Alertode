using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对象工厂类，用于创建各种游戏对象
public class ObjectFactory
{
    /*根据注册名创建英雄*/
    public static BaseHero CreateHero(string name){
        var data = Configs.GetHero(name);
        var hero = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseHero;
        if(hero!=null){
            hero.Init(data);
        }
        return hero;
    }

	/*根据注册名创建建筑*/
	public static BaseBuild CreateBuild(string name){
        var data = Configs.GetBuild(name);
        var build = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseBuild;
        if(build!=null){
            build.Init(data);
        }
        return build;
	}

	/*根据注册名创建武器*/
	public static BaseWeapon CreateWeapon(string name){
        var data = Configs.GetWeapon(name);
        var weapon = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseWeapon;
        if(weapon!=null){
            weapon.Init(data);
        }
        return weapon;
	}

    /*根据注册名创建被动技能*/
    public static BaseSkill CreateSkill(string name){
        var data = Configs.GetSkill(name);
        var skill = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseSkill;
        if(skill!=null){
            skill.Init(data);
        }
        return skill;
    }

    /*根据注册名创建终极技能*/
    public static BaseUltimateSkill CreateUltimateSkill(string name){
        var data = Configs.GetUltimateSkill(name);
        var skill = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseUltimateSkill;
        if(skill!=null){
            skill.Init(data);
        }
        return skill;
    }

	/*根据注册名创建子弹*/
	public static BaseBullet CreateBullet(string name){
        var data = Configs.GetBullet(name);
        var bullet = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseBullet;
        if(bullet!=null){
            bullet.Init(data);
        }
        return bullet;
	}

	/*根据注册名创建怪物*/
	public static BaseMonster CreateMonster(string name){
        var data = Configs.GetMonster(name);
        var monster = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseMonster;
        if(monster!=null){
            monster.Init(data);
        }
        return monster;
	}

    /*根据注册名创建芯片*/
    public static BaseChip CreateChip(string name){
        var data = Configs.GetChip(name);
        var chip = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseChip;
        if(chip!=null){
            chip.Init(data);
        }
        return chip;
    }

    /*根据注册名创建Buff*/
    public static BaseBuff CreateBuff(string name,string source){
        var data = Configs.GetBuff(name);
        var buff = Activator.CreateInstance(Type.GetType(data.className,true)) as BaseBuff;
        if(buff!=null){
            buff.Init(data,source);
        }
        return buff;
    }

    /*根据配置创建刷兵点*/
    public static MonsterGenerator CreateMonsterGenerator(MonsterGeneratorData data){
        var monsGen = new MonsterGenerator();
        monsGen.Init(data);
        return monsGen;
    }

    /*创建英雄视图对象*/
    public static BuildView CreateHeroView(BaseHero hero){
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>("prefab/hero/"+hero.name));
        var view = gameObject.GetComponent<HeroView>();
        view.Bind(hero);
        return view;
    }

	/*创建建筑视图对象*/
	public static BuildView CreateBuildView(BaseBuild build){
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>("prefab/build/"+build.name));
        var view = gameObject.GetComponent<BuildView>();
        view.Bind(build);
        return view;
	}

	/*创建子弹视图对象*/
	public static BulletView CreateBulletView(BaseBullet bullet){
        var gameObject = ObjectPool.ins.GetBullet(bullet.name);
        if(gameObject==null)gameObject = GameObject.Instantiate(Resources.Load<GameObject>("prefab/bullet/"+bullet.name));
        var view = gameObject.GetComponent<BulletView>();
        view.Bind(bullet);
        return view;
	}

	/*创建怪物视图对象*/
	public static MonsterView CreateMonsterView(BaseMonster monster){
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>("prefab/monster/"+monster.name));
        var view = gameObject.GetComponent<MonsterView>();
        view.Bind(monster);
        return view;
	}

    /*创建Buff视图对象*/
    public static BuffView CreateBuffView(BaseBuff buff){
        var gameObject = ObjectPool.ins.GetBuff(buff.data.name);
        if(gameObject==null){
            var prefab = Resources.Load<GameObject>("prefab/buff/"+buff.data.name);
            if(prefab==null)prefab = Resources.Load<GameObject>("prefab/buff/Inviso");
            gameObject = GameObject.Instantiate(prefab);
        }
        var view = gameObject.GetComponent<BuffView>();
        view.Bind(buff);
        return view;
    }

    /*创建刷兵点视图对象*/
    public static MonsterGeneratorView CreateMonsterGeneratorView(MonsterGenerator monsGen){
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>("prefab/monsterGenerator"));
        var view = gameObject.GetComponent<MonsterGeneratorView>();
        view.Bind(monsGen);
        return view;
    }

	/*根据名称创建特效*/
	public static GameObject CreateEffect(string name){
        var effect = ObjectPool.ins.GetEffect(name);
        if(effect==null)effect  = GameObject.Instantiate(Resources.Load<GameObject>("prefab/effect/"+name));
        effect.GetComponent<EffectView>().Init(name);
        return effect;
	}

    /*根据注册名创建地图脚本*/
    public static BaseMapScript CreateMapScript(string name){
        var script = Activator.CreateInstance(Type.GetType(name,true)) as BaseMapScript;
        return script;
    }

}
