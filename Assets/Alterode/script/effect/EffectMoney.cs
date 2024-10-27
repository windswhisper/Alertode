using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMoney : MonoBehaviour
{
	public TextMesh txtAmount;

	public void Init(int amount){
		txtAmount.text = "$+"+amount;
	}

	public void InitGem(int amount){
		txtAmount.text = "+"+amount;
	}
}
