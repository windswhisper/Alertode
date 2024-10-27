using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffOil : BaseBulletBuff
{
    public FP duration;
    public int damageRate;

    public BulletBuffOil(FP duration,int damageRate){
        this.duration = duration;
        this.damageRate = damageRate;
    }

    public override void OnExplode(){
        var oilBullet = BattleManager.ins.CreateBullet("BulletGroundFire");
        oilBullet.damage = bullet.damage * damageRate / (int)(duration*2) / 100;   
        oilBullet.life = duration;
        var c=new Coord(bullet.position.x,bullet.position.z);
        var p=new TSVector(c.x,0,c.y);
        oilBullet.Enter(p,bullet.unit,bullet.weapon,null,p);
    }
}
