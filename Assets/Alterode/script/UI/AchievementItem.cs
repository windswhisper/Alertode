using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class AchievementItem : MonoBehaviour
{
	public AchievementPanel panel;
	public Image icon;
	public Text txtName;
	public Text txtDesc;
	public Text txtProgress;
	public Image progressBar;
	public GameObject progressFull;
	public GameObject hint;
	public GameObject tagClaimed;
	public Transform rewardRoot;
	public GameObject rewardGem;
	public GameObject rewardMetal;
	public GameObject rewardChip;
	public Text txtRewardGem;
	public Text txtRewardMetal;
	public Text txtRewardChip;
	public Button btnClaim;

	ProgressData achieve;
	AchievementData data;

	public void Init(AchievementPanel panel,ProgressData achieve){
		this.panel = panel;
		this.achieve = achieve;
		data = Configs.GetAchieve(achieve.id);

		icon.sprite = Resources.Load<Sprite>("image/achieve/"+achieve.id);
		txtName.text = I18N.instance.getValue(data.uiName);
		txtDesc.text = string.Format(I18N.instance.getValue(data.desc),data.goal);
		txtProgress.text = achieve.progress+"/"+data.goal;
		progressBar.fillAmount = (achieve.progress*1f/data.goal);
		if(achieve.progress>=data.goal){
			if(achieve.claimReward){
				tagClaimed.SetActive(true);
				hint.SetActive(false);
				rewardRoot.gameObject.SetActive(false);
				btnClaim.interactable = false;
			}
			else{
				tagClaimed.SetActive(false);
				hint.SetActive(true);
				rewardRoot.gameObject.SetActive(true);
				btnClaim.interactable = true;
			}
			progressFull.SetActive(true);
		}
		else{
			tagClaimed.SetActive(false);
			btnClaim.interactable = false;
			rewardRoot.gameObject.SetActive(true);
			hint.SetActive(false);
			progressFull.SetActive(false);
		}

		foreach(var reward in data.rewards){
			if(reward.type == 0){
				rewardGem.SetActive(true);
				txtRewardGem.text = "x"+reward.count;
			}
			else if(reward.type == 1){
				rewardMetal.SetActive(true);
				txtRewardMetal.text = "x"+reward.count;
			}
			else if(reward.type == 2){
				rewardChip.SetActive(true);
				txtRewardChip.text = "x"+reward.count;
			}
		}

	}

	public void GetReward(){
		tagClaimed.SetActive(true);
		hint.SetActive(false);
		rewardRoot.gameObject.SetActive(false);
		btnClaim.interactable = false;

		foreach(var reward in data.rewards){
			if(reward.type == 0){
				UserData.data.gem += reward.count;
			}
			else if(reward.type == 1){
				UserData.data.metal += reward.count;
			}
			else if(reward.type == 2){
				UserData.data.silicon += reward.count;
			}
		}

		achieve.claimReward = true;
		AchievementManager.GetAchievement(data.name);
		UserData.Save();

		#if UNITY_STANDALONE
			var ach = new Steamworks.Data.Achievement( data.name );
	    	ach.Trigger();
		#endif
	}
}
