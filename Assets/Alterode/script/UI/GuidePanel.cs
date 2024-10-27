using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class GuidePanel : MonoBehaviour
{
	public Image card;
	public Text txtTips;

	public void Show(){
		gameObject.SetActive(true);

		var index = UserData.data.guideProgress;

		card.sprite = Resources.Load<Sprite>("image/guide/guide_"+index);
		txtTips.text = I18N.instance.getValue("@Guide.G"+index);

		BattleManager.ins.Pause();
	}

	public void Close(){
		UserData.data.guideProgress++;
		gameObject.SetActive(false);

		BattleManager.ins.Resume();
	}
}
