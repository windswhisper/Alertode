using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

//瞬间命中子弹
public class BulletInviso : BaseBullet
{
    /*逻辑帧更新，立刻到达目标点，并击中目标*/
    /*参数列表：时间间隔*/
    public override void Step(FP dt){
        if(t+dt>=life){
            if(target!=null){
                position = targetPos;
                if(explodeRadius == 0){
                    Hit(target);
                }
                Explodes();
            }
            else{
                if(explodeRadius == 0){
                    DetectCollision();
                }
                Explodes();
            }

            return;
        }

    	base.Step(dt);
    }
}
