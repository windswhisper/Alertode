using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletLensWall : BaseBullet
{
    FP wallRange;
    int damageRate;
    int radiusRate;
    BaseBuild node1;
    BaseBuild node2;
    public Coord c1;
    public Coord c2;
    bool isBlocked;
    public List<BaseBullet> ignoreBulletList = new List<BaseBullet>();

    public void InitLens(int damageRate,int radiusRate,BaseBuild node1,BaseBuild node2){
        this.damageRate = damageRate;
        this.radiusRate = radiusRate;
        this.node1 = node1;
        this.node2 = node2;
        c1 = new Coord(node1.position);
        c2 = new Coord(node2.position);
    }

    public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
        base.Enter(p,unit,weapon,target,targetPos);

        wallRange = TSVector.Distance(targetPos,p);
    }

    /*逻辑帧更新，立刻到达目标点，并击中目标*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
        base.Step(dt);
        DetectMiss();
        if(isMissed)return;
        //无限时长
        t=0;
        if(isBlocked){
            if(view!=null)view.gameObject.SetActive(false);
            return;
        }else{
            if(view!=null)view.gameObject.SetActive(true);
        }
        DetectCollision();
    }

    void DetectMiss(){
        if(node1==null || node1.isDead || node2==null || node2.isDead){
            isMissed = true;
            return;
        }

        isBlocked = false;
        foreach(var unit in BattleManager.ins.engine.unitList){
            if(unit.type == UnitType.Building && !((BaseBuild)unit).isTrap){
                if(unit!=node1 && unit!=node2){
                    var c = new Coord(unit.position.x,unit.position.z);
                    if(c1.x==c2.x && c.x == c1.x && ((c1.y>c2.y && c1.y>c.y && c.y>c2.y) || (c2.y>c1.y && c2.y>c.y && c.y>c1.y))){
                        isBlocked = true;
                        return;
                    }
                    if(c1.y==c2.y && c.y == c1.y && ((c1.x>c2.x && c1.x>c.x && c.x>c2.x) || (c2.x>c1.x && c2.x>c.x && c.x>c1.x))){
                        isBlocked = true;
                        return;
                    }
                }
            }
        }
    }

    /*碰撞检测*/
    public override void DetectCollision(){
        foreach(var buff in buffs){
            if(buff.IsDisCollide()){
                return;
            }
        }
        
        var p = new TSVector2(position.x,position.z);
        var d = targetPos - position;
        var dir= d.normalized;
        var dir2 = new TSVector2(dir.z,-dir.x);
        var p2 = p+new TSVector2(dir.x,dir.z)*wallRange;
        TSVector2 ra;
        TSVector2 rb;
        TSVector2 rc;
        TSVector2 rd;
        TSVector2 mp2;
        foreach(var bullet in BattleManager.ins.engine.bulletList){
            if(bullet.team==unit.team && !ignoreBulletList.Contains(bullet)){
                if(bullet.type == BulletType.Straight || bullet.type == BulletType.Arcing || bullet.type == BulletType.Missile){
                    ra = p+dir2*radius/2;
                    rb = p-dir2*radius/2;
                    rc = p2-dir2*radius/2;
                    rd = p2+dir2*radius/2;
                    mp2 = new TSVector2(bullet.position.x,bullet.position.z);
                    if(Utils.IsPointInRect(mp2,ra,rb,rc,rd)){
                        EnchantBullet(bullet);
                    }
                }
            }
        }

    }

    public void EnchantBullet(BaseBullet bullet){
        ignoreBulletList.Add(bullet);
        bullet.damage = bullet.damage*(100+damageRate)/100;
        bullet.largePercent = bullet.largePercent+radiusRate;
        bullet.radius = bullet.radius*(100+bullet.largePercent)/100;
        bullet.explodeRadius = bullet.explodeRadius*(100+bullet.largePercent)/100;
        bullet.speed = bullet.speed*2;
        if(bullet.speed>20)bullet.speed = 20;
    }
}