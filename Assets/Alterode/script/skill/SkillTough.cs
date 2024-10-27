using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTough : BaseSkill
{
	public override void OnUnitGetBuff(BaseBuff buff){
		if(buff.data.isNegative){
			buff.Remove();
		}
	}
}
