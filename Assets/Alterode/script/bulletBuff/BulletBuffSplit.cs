using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffSplit : BaseBulletBuff
{
	int splitCount;
	int damagePercent;

	public BulletBuffSplit(int splitCount,int damagePercent){
		this.splitCount = splitCount;
		this.damagePercent = damagePercent;
	}

	public override void OnExplode(){
		if(bullet.type == BulletType.Inviso)return;

		var startAngle = TSRandom.instance.NextFP()*2*TSMath.Pi;
		for(var i=0;i<splitCount;i++){
			var angle = i*2*TSMath.Pi/splitCount + startAngle;
			var offset = Utils.VectorRotate(new TSVector(0,0,30)/100,angle);
			var aimPos = Utils.VectorRotate(new TSVector(0,0,150)/100,angle);

	        var splitBullet = BattleManager.ins.CreateBullet(bullet.name);
	        splitBullet.damage = bullet.damage * damagePercent / 100;	
	        splitBullet.largePercent = bullet.largePercent-30;
	        if(bullet.type == BulletType.Straight || bullet.type == BulletType.Missile)splitBullet.life = bullet.life/2;
	        splitBullet.Enter(bullet.position+offset,bullet.unit,bullet.weapon,null,bullet.position+aimPos);
	        splitBullet.AddBuff(new BulletBuffDisCollide(new FP(10)/100));
	        if(bullet.ignoreList.Count>0){
	        	splitBullet.ignoreList.Add(bullet.ignoreList[bullet.ignoreList.Count-1]);
	        }
	        foreach(var buff in bullet.buffs){
	        	if(buff.GetType().Name != "BulletBuffSplit")splitBullet.AddBuff(buff);
	        }
		}
	}
}
