using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSuicide : BaseSkill
{
    public override void OnShotBullet(BaseBullet bullet){
        unit.Die(null);
    }
}
