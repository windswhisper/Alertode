using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBtnAchieve : MonoBehaviour
{
    void Start()
    {
		bool hasAchieved = false;
		foreach(var achieve in UserData.data.achieveProgressDatas){
			var data = Configs.GetAchieve(achieve.id);
			if(achieve.progress >= data.goal && !achieve.claimReward){
				hasAchieved = true;
			}
		}

		if(!hasAchieved)gameObject.SetActive(false);
    }

}