using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ChipOil : BaseChip
{
    public int triggerRate;
    public FP duration;
    public int damageRate;

    public override void Init(ChipData data){
        base.Init(data);

        this.triggerRate = data.paras[0];
        this.duration = new FP(data.paras[1])/100;
        this.damageRate = data.paras[2];
    }

    public override void OnShotBullet(BaseBullet bullet){
        if(TSRandom.instance.Next()*100<triggerRate){
            bullet.AddBuff(new BulletBuffOil(duration,damageRate));
        }
    }
}
