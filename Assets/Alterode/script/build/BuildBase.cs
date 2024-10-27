using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBase : BaseBuild
{
    public override void Die(BaseUnit damageSource){
    	base.Die(damageSource);

    	BattleManager.ins.Fail();
    }
}
