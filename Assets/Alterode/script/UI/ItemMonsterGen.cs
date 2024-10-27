using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMonsterGen : MonoBehaviour
{
	public Image icon;
	public Text txtNum;

	public void Init(string name,int num){
		txtNum.text = "x"+num;
		icon.sprite = Resources.Load<Sprite>("image/monster/"+name);
	}
}
