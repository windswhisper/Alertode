using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLearnUtmSkill : BaseChip
{
	public string utmSkillName;

	public override void Init(ChipData data){
		base.Init(data);
		
		utmSkillName = data.namePara;
	}

	public override void OnEquiped(BaseBuild build){
		base.OnEquiped(build);

        var utmSkill = ObjectFactory.CreateUltimateSkill(utmSkillName);
        build.utmSkills.Add(utmSkill);
        utmSkill.Equiped(build);
	}

}
