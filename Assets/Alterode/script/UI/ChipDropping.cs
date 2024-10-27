using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDropping : MonoBehaviour
{
	public GameObject chipPickingEffect;
	public BaseChip chip;

	public SpriteRenderer icon;
	public SpriteRenderer bg;

	void Start(){
		StartCoroutine(Remove());
	}

	public void Init(BaseChip chip){
		this.chip = chip;

		icon.sprite = Resources.Load<Sprite> ("image/chip/"+chip.data.image) as Sprite;

		switch(chip.data.rare){
			case 0:
				icon.color = new Color(0.88f,0.88f,1f,1f);
				bg.color = new Color(0.88f,0.88f,1f,0.2f);
				break;
			case 1:
				icon.color = new Color(0.9f,0.3f,0.9f,1f);
				bg.color = new Color(0.6f,0.2f,0.6f,0.2f);
				break;
			case 2:
				icon.color = new Color(1f,1f,0.3f,1f);
				bg.color = new Color(1f,1f,0.3f,0.2f);
				break;
		}
	}

	public void ShowPickEffect(){
		var effect = Instantiate(chipPickingEffect);
		effect.transform.SetParent(GameObject.Find("EffectLayer").transform,false);
		effect.transform.position = Camera.main.WorldToScreenPoint(transform.position+new Vector3(0,0.5f,0));
		effect.GetComponent<ChipPickEffect>().Init(chip);
	}

	IEnumerator Remove(){
		yield return new WaitForSeconds(3.5f);
		ShowPickEffect();
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}
