using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillSummon : BaseSkill
{
	public FP summonCd;
	public int radius;
	public string monsterName;
	public int maxSummon;
	public int monsterCount;

	int summonCount;
	FP t;

	public override void Init(SkillData data){
		base.Init(data);

		summonCd = new FP(data.paras[0])/100;
		radius = data.paras[1];
		maxSummon = data.paras[2];
		monsterName = data.namePara;
		monsterCount = data.paras[3];

		summonCount = 0;
	}

	public override void OnAttach(BaseUnit unit){
		base.OnAttach(unit);

		t = summonCd;
	}

	public override void Step(FP dt){
	    base.Step(dt);

		t-=dt;
		if(t<0 && summonCount<maxSummon){
			t=summonCd;
			Summon();
		}
	}

	public void Summon(){
		summonCount++;

		if(unit.view!=null)((BaseMonster)unit).view.PlaySpellAnim();

		unit.fireCd = 2;

		var coord = new Coord(unit.position.x,unit.position.z);
		var list = new List<Coord>();
		for(var i=coord.x-radius;i<=coord.x+radius;i++){
			for(var j=coord.y-radius;j<coord.y+radius;j++){
				if(BattleMap.ins.IsCoordInMap(i,j) && !(i==coord.x && j==coord.y)){
					if(BattleMap.ins.IsTileCanWalk(i,j)){
						list.Add(new Coord(i,j));
					}
				}
			}
		}

		if(list.Count==0)return;
		for(var i=0;i<list.Count&&i<monsterCount;i++){
			var pos = list[TSRandom.Range(0,list.Count)];
			var bullet = BattleManager.ins.CreateBullet("BulletSummon");
	        bullet.Enter(new TSVector(pos.x,0,pos.y),unit,null,null,new TSVector(pos.x,0,pos.y));
			((BulletSummon)bullet).monsterName = monsterName;
		}
	}
}
