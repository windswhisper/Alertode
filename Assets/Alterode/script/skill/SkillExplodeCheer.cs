using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SkillExplodeCheer : BaseSkill
{
    public int speedRate;
    public int fireSpeedRate;
    public FP duration;
    string bulletName;

    public override void Init(SkillData data){
        base.Init(data);

        bulletName = data.namePara;
        speedRate = data.paras[0];
        fireSpeedRate = data.paras[1];
        duration = new FP(data.paras[2])/100;
    }

    public override bool OnDie(BaseUnit damageSource){
        var bullet = BattleManager.ins.CreateBullet(bulletName);
        bullet.Enter(unit.position,unit,null,null,unit.position);
        bullet.AddBuff(new BulletBuffCheer(speedRate,fireSpeedRate,duration));
        bullet.affectsAllies = true;
        bullet.affectsEnemies = false;
        return false;
    }
}
