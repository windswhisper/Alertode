using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletViewLens : BulletView
{

    public ParticleSystem particle;

    /*进入场景*/
    public override void Update(){
        base.Update();
        transform.localPosition = Utils.TSVecToVec3(bullet.targetPos + bullet.position)/2;
        var main = particle.main;
        if(TSMath.Abs(bullet.targetPos.z - bullet.position.z)<1){
            transform.localScale = new Vector3((float)(TSVector.Distance(bullet.targetPos,bullet.position)),1,1);
            main.startRotationZ = 0;
        }
        else{
            transform.localScale = new Vector3(1,1,(float)(TSVector.Distance(bullet.targetPos,bullet.position)));
            main.startRotationZ = Mathf.PI/2;
        }
    }
}
