using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipCheer : BaseChip
{
    public int fireSpeedRate;
    public FP duration;

    public override void Init(ChipData data){
        base.Init(data);

        this.fireSpeedRate = data.paras[0];
        this.duration = new FP(data.paras[1])/100;
    }

    public override void OnShotBullet(BaseBullet bullet){
        bullet.AddBuff(new BulletBuffFireSpeed(fireSpeedRate,duration,"ChipCheer"));
        bullet.affectsAllies = true;
    }
}
