using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TipsChipInfo : MonoBehaviour
{
	public Text txtName;
	public Text txtDesc;
	
	public void Init(string chipId){
		var data = Configs.GetChip(chipId);
		txtName.text = I18N.instance.getValue(data.uiName);

		string[] paras = new string[data.paras.Count];
		int i=0;
		foreach(var para in data.paras){
			paras[i] = Convert.ToString(para);
			i++;
		}
		txtDesc.text = string.Format(new DescFormatter(),I18N.instance.getValue(data.desc),paras);

	}
}
