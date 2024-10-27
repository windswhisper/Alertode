using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewRush : MonsterView
{

    protected override void Update()
    {   
        base.Update();
        
        if(monster!=null && !monster.isDead){
            animator.SetBool("rushing",((MonsterRush)monster).isRushing && !monster.IsStunned());
        }
    }
}
