using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuildObelisk : BaseBuild
{
    public int progress;
    public bool isOccupied;
    public bool isOccupying = false;

    FP occupyCd = 2;
    FP occupyTime;
    public string tech;

    public override void Enter(TSVector p,TeamType team){
		base.Enter(p,team);

		progress = 0;
		occupyTime = occupyCd;
		isOccupied = false;
	}

	public void SetTech(string tech){
		this.tech = tech;
		AddChip(ObjectFactory.CreateChip(tech));
	}

    public override void Step(FP dt){
    	base.Step(dt);

    	isOccupying = false;

    	if(!isOccupied){
    		if(IsHeroNear()){
		    	occupyTime += dt;
    			isOccupying = true;
		    	if(occupyTime>occupyCd){
		    		occupyTime = 0;
		    		progress+=1;
		    		if(progress>=100){
		    			isOccupied = true;
		    			progress = 100;
		    			Occupied();
		    		}
		    	}
    		}
    		else{
    			occupyTime = occupyCd;
    			isOccupying = false;
    		}
    	}
    }

    bool IsHeroNear(){
    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.type == UnitType.Building && ((BaseBuild)u).isHero && !u.isDead){
    			if(TSVector.Distance(u.position,position)*2<3){
    				return true;
    			}
    		}
    	}
    	return false;
    }

    void Occupied(){
    	foreach(var chip in chips){
    		chip.OnBuildOccupied();
    	}
		BattleManager.ins.AddNewTech(data.name+"_Temp",tech);
		chips.Clear();
		AddChip(ObjectFactory.CreateChip(tech));
    }


    public override MapBuildData ToData(){
    	var dat = base.ToData();
    	dat.extraParas.Add(progress);
    	dat.extraNamePara = tech;
        return dat;
    }

    public override void FromData(MapBuildData dat){
    	base.FromData(dat);
    	progress = dat.extraParas[dat.extraParas.Count-1];
    	tech = dat.extraNamePara;
    	if(progress >= 100)isOccupied = true;
    	if(tech=="")return;
		AddChip(ObjectFactory.CreateChip(tech));
    }

}
