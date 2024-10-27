using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ChangeTeamChipOption : MonoBehaviour
{
	public Image icon;
	public Text txtName;
	public Text txtDesc;
	public GameObject tagUsing;

	public void SetChip(string chipId,bool isUsing){
		tagUsing.SetActive(isUsing);

		var chipData = Configs.GetChip(chipId);

		icon.sprite = Resources.Load<Sprite> ("image/chip/"+chipData.image);

		txtName.text = I18N.instance.getValue(chipData.uiName);

		string[] paras = new string[chipData.paras.Count];
		int i=0;
		foreach(var para in chipData.paras){
			paras[i] = Convert.ToString(para);
			i++;
		}

		txtDesc.text = string.Format(new DescFormatter(),I18N.instance.getValue(chipData.desc),paras);
	}
}
