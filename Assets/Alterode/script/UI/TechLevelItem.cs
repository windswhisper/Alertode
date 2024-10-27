using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechLevelItem : MonoBehaviour
{
	public Image frame;
	public Image icon;
	public Sprite[] spFrames;
	public Sprite[] spIcons;

	public Text txtLevel;
	public GameObject lineNext;

	public void Init(int type,int level,bool isUnlock,bool isNewest){
		frame.sprite = spFrames[isUnlock?1:0];
		icon.sprite = spIcons[type];
		txtLevel.text = "LV"+level;
		lineNext.SetActive(!isNewest);
	}
}
