using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHeroSkill : MonoBehaviour
{
	public Image icon;

	public void Init(string skillId){
		icon.sprite = Resources.Load<Sprite>("image/ultimateSkill/"+skillId);
	}
}
