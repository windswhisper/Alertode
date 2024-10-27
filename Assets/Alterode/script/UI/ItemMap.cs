using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMap : MonoBehaviour
{
	public GameObject selectFrame;
	public Image icon;

	public int id = 0;

	bool isSelected = false;

	public void Init(int type,int id){
		this.id = id;
		if(type == 0){
			icon.sprite = Resources.Load<Sprite> (string.Format("image/mapEditor/mgt{0:D3}",id));
		}
		else if(type == 1){
			icon.sprite = Resources.Load<Sprite> (string.Format("image/mapEditor/mobj{0:D3}",id));
		}
	}

	public void OnClick(){
		MapEditor.ins.SelectItem(id);
	}

	public void Selected(){
		isSelected = true;
		selectFrame.SetActive(true);
	}
	public void CancelSelected(){
		isSelected = false;
		selectFrame.SetActive(false);
	}
}
