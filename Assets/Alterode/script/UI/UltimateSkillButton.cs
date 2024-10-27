using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class UltimateSkillButton : MonoBehaviour
{
	BuildDetailPanel buildDetailPanel;
	BaseUltimateSkill ultimateSkill;
	public Image icon;
	public Image cdMask;
	public Text txtCd;

	public void Init(BuildDetailPanel buildDetailPanel,BaseUltimateSkill ultimateSkill){
		this.buildDetailPanel = buildDetailPanel;
		this.ultimateSkill = ultimateSkill;

		icon.sprite = Resources.Load<Sprite>("image/ultimateSkill/"+ultimateSkill.data.name);
	}

	void Update(){
		if(!ultimateSkill.isReady){
			cdMask.gameObject.SetActive(true);
			cdMask.fillAmount = (float)(ultimateSkill.t/ultimateSkill.cd);
			txtCd.text = ""+(int)ultimateSkill.t;
		}else{
			cdMask.gameObject.SetActive(false);
		}
	}

	public void Click(){
		if(!ultimateSkill.isReady)return;
		if(ultimateSkill.targetType == TargetType.None){
			ultimateSkill.Cast(new TSVector());
			buildDetailPanel.Hide();
		}
		else{
			buildDetailPanel.castSkillPanel.Show(ultimateSkill,buildDetailPanel);
		}
	}
}
