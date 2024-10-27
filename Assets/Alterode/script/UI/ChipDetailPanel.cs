using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ChipDetailPanel : MonoBehaviour
{
	public Text txtName;
	public Text txtDesc;

	BaseChip chip;

	public void Show(BaseChip chip){
		this.chip = chip;

		txtName.text = I18N.instance.getValue(chip.data.uiName);
		if(chip.data.prerequisite!="")txtName.text+=" +"+chip.data.level;

		string[] paras = new string[chip.data.paras.Count];
		int i=0;
		foreach(var para in chip.data.paras){
			paras[i] = Convert.ToString(para);
			i++;
		}
		string[] recordParas = new string[chip.recordParas.Count];
		i=0;
		foreach(var para in chip.recordParas){
			recordParas[i] = Convert.ToString(para);
			i++;
		}

		txtDesc.text = string.Format(new DescFormatter(),I18N.instance.getValue(chip.data.desc),paras);
		if(chip.data.record!=""){
			txtDesc.text += "\n"+string.Format(new DescFormatter(),I18N.instance.getValue(chip.data.record),recordParas);
		}

		gameObject.SetActive(true);
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
