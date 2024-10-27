using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ItemChip : MonoBehaviour
{
	public Image icon;
	public Image bg;

	public BaseChip chip;

	public void Init(BaseChip chip){
		this.chip = chip;

		icon.sprite = Resources.Load<Sprite> ("image/chip/"+chip.data.image) as Sprite;

		// switch(chip.data.rare){
		// 	case 0:
		// 		icon.color = new Color(0.88f,0.88f,1f,1f);
		// 		bg.color = new Color(0.88f,0.88f,1f,0.2f);
		// 		break;
		// 	case 1:
		// 		icon.color = new Color(0.9f,0.3f,0.9f,1f);
		// 		bg.color = new Color(0.6f,0.2f,0.6f,0.2f);
		// 		break;
		// 	case 2:
		// 		icon.color = new Color(1f,1f,0.3f,1f);
		// 		bg.color = new Color(1f,1f,0.3f,0.2f);
		// 		break;
		// }
	}

	public void Init(ChipData data){
		icon.sprite = Resources.Load<Sprite> ("image/chip/"+data.image) as Sprite;

		// switch(data.rare){
		// 	case 0:
		// 		icon.color = new Color(0.88f,0.88f,1f,1f);
		// 		bg.color = new Color(0.88f,0.88f,1f,0.2f);
		// 		break;
		// 	case 1:
		// 		icon.color = new Color(0.9f,0.3f,0.9f,1f);
		// 		bg.color = new Color(0.6f,0.2f,0.6f,0.2f);
		// 		break;
		// 	case 2:
		// 		icon.color = new Color(1f,1f,0.3f,1f);
		// 		bg.color = new Color(1f,1f,0.3f,0.2f);
		// 		break;
		// }
	}

	public void Init(string chipId){
		var data = Configs.GetChip(chipId);
		Init(data);
	}
}
