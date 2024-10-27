using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBlossom : BaseSkill
{
	public int blossomCount;

	public override void Init(SkillData data){
		base.Init(data);
		
		blossomCount = data.paras[0];
	}

	public override int OnGetBlossomCount(){
		return blossomCount;
	}
}
