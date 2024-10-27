using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TipsSkillInfo : MonoBehaviour
{
	public Text txtName;
	public Text txtDesc;
	public Text txtCd;

	public void Init(string skillId){
		var data = Configs.GetUltimateSkill(skillId);
		txtName.text = I18N.instance.getValue(data.uiName);
		txtDesc.text = I18N.instance.getValue(data.desc);
		txtCd.text = data.cd/100+"s";
	}
}
