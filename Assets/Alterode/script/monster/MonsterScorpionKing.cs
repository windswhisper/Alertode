using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterScorpionKing : MonsterStealth
{
	int stage = 0;
	bool isStealthing = false;
	bool isStealthDone = false;
	FP stealthTime = 0;

	public FP skillCd = 25;
	public FP skillTime = 25;
	public int skillDamage = 100;

    public override void Init(MonsterData data){
    	base.Init(data);

    	skillCd = new FP(data.paras[0])/100;
    	skillDamage = data.paras[1];
    	skillTime = 0;
    }

    public override void Enter(TSVector p,TeamType team){
    	base.Enter(p,team);
    	position.y = 0;
    }

	public override void Step(FP dt){
		base.Step(dt);

		if(isStealthDone){
			if(view!=null){
				((MonsterViewScorpionKing)view).Jump2();
			}
			isStealthDone = false;
		}

		if(isStealthing){
			stealthTime+=dt;
			if(stealthTime>1 && stealthTime<4){
				position.y = -3;
			}
			else if(stealthTime>4){
				StealthDone();
			}
		}
		else{
			skillTime-=dt;
			if(skillTime<=0)
			{
				skillTime = skillCd;
				Spell();
			}
		}

		if(stage==0 && hp<GetMaxHp()*60/100)
		{
			Stealth();
			stage++;
		}
		else if(stage==1 && hp<GetMaxHp()*30/100)
		{
			Stealth();
			stage++;
		}
	}


	public override void Move(FP dt){
		if(!isStealthing)Walk(dt);
	}

	public void StealthDone(){
		position.y = 0;
		var list = new List<Coord>();
		for(var i=0;i<BattleMap.ins.mapData.width;i++){
			for(var j=0;j<BattleMap.ins.mapData.height;j++){
				if(BattleMap.ins.IsTileCanWalk(i,j) && BattleMap.ins.pathMapWalk[BattleMap.ins.IndexByCoord(i,j)]!=999 && BattleMap.ins.pathMapWalk[BattleMap.ins.IndexByCoord(i,j)] >= 10 && TSMath.Abs(i-coord.x)+TSMath.Abs(j-coord.y)>2)
				{
					list.Add(new Coord(i,j));
				}
			}
		}
		var goalPos = list[TSRandom.Range(0,list.Count)];
		position.x = goalPos.x;
		position.z = goalPos.y;
		isStealthing = false;
		stealthTime = 0;
		coord = null;
		nextStep = null;

		isStealthDone = true;

		list.Remove(goalPos);
		for(var i=0;i<5;i++){
			var c = list[TSRandom.Range(0,list.Count)];
			var monster = BattleManager.ins.PutMonster("Scorpling",c.x,c.y,false);
			((MonsterStealth)monster).Jump();
		}
	}

	public void Stealth(){
		isStealthing = true;
		if(view!=null){
			((MonsterViewScorpionKing)view).Stealth();
		}
		fireUpTime = 3;
	}

	public void Spell(){
		BaseUnit target = null;
		FP minDis = 65536;
    	foreach(var u in BattleManager.ins.engine.unitList){
    		if(u.team!=team && !u.isDead && !u.IsImmune() && !IsUnderGround()){
                if(u.type != UnitType.Monster || ((BaseMonster)u).moveType != MoveType.Fly){
                    var d = (u.position - position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<minDis)target = u;
                }
    		}
    	}

	    var bullet = BattleManager.ins.CreateBullet("SandTornado");
	    bullet.damage = skillDamage;	
	    bullet.Enter(position,this,null,target,target.position);

	    fireCd = 2;

	    if(view!=null)view.PlaySpellAnim();
	}
}
