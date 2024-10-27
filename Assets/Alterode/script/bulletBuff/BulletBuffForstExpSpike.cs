using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffFrostExpSpike : BaseBulletBuff
{
	int spikeCount;
	int damagePercent;

	public BulletBuffFrostExpSpike(int spikeCount,int damagePercent){
		this.spikeCount = spikeCount;
		this.damagePercent = damagePercent;
	}

	public override void OnExplode(){
		var startAngle = TSRandom.instance.NextFP()*2*TSMath.Pi;
		for(var i=0;i<spikeCount;i++){
			var angle = i*2*TSMath.Pi/spikeCount + startAngle;
			var offset = Utils.VectorRotate(new TSVector(0,0,30)/100,angle);
			var aimPos = Utils.VectorRotate(new TSVector(0,0,150)/100,angle);

	        var splitBullet = BattleManager.ins.CreateBullet("IceSpike");
	        splitBullet.damage = bullet.damage * damagePercent / 100;	
	        splitBullet.Enter(bullet.position+offset,bullet.unit,null,null,bullet.position+aimPos);
	        if(bullet.unit!=null)bullet.unit.OnShotBullet(splitBullet);
		}
	}
}
