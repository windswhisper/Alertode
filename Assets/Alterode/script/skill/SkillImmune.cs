using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillImmune : BaseSkill
{
    public override bool OnGetImmune(){
        return true;
    }
}
