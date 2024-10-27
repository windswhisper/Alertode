using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletInvisoChain : BulletInviso
{
    public List<BaseUnit> hitList;

    public int chainCount;
    public FP chainRange;

    public override void Init(BulletData data){
        base.Init(data);
        chainCount = data.paras[0];
        chainRange = new FP(data.paras[1])/100;
        hitList = new List<BaseUnit>();

    }

    public override void Enter(TSVector p,BaseUnit unit,BaseWeapon weapon,BaseUnit target,TSVector targetPos){
        base.Enter(p,unit,weapon,target,targetPos);

        chainCount += unit.GetChainCount();
        
    }

    public void InitChain(List<BaseUnit> hitList,int chainCount){
        this.chainCount = chainCount;
        this.hitList = hitList;
    }

    public override void Hit(BaseUnit target){
        base.Hit(target);

        if(chainCount==0)return;
        hitList.Add(target);
        foreach(var u in BattleManager.ins.engine.unitList){
            if(u.team!=team && !u.isDead && !hitList.Contains(u) && !u.IsUnderGround()){
                if(u.type != UnitType.Monster || ( ( ((BaseMonster)u).moveType == MoveType.Fly && aimType == AimType.Air ) || ( ((BaseMonster)u).moveType != MoveType.Fly && aimType == AimType.Ground ) || aimType == AimType.All )){
                    var d = (u.position - position);
                    d.y = 0;
                    var disSQ = d.sqrMagnitude;
                    if(disSQ<chainRange*chainRange){
                        var bullet = BattleManager.ins.CreateBullet(name);
                        ((BulletInvisoChain)bullet).InitChain(hitList,chainCount-1);
                        bullet.Enter(position,unit,weapon,u,u.position);
                        bullet.damage = damage;
                        return;
                    }
                }
            }
        }

    }
}
