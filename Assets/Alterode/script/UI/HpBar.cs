using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
	public Animation anim;
	public Image bar;

	float displayPercent = 1;

	BaseUnit unit;
	bool isShow;

	public void Bind(BaseUnit unit){
		this.unit = unit;
		isShow = false;
		displayPercent = 1;
	}

	void Update(){
		if(unit == null || unit.view == null){
			Destroy(gameObject);
			return;
		}


		if(unit.team == TeamType.Enemy){
			bar.color = new Color(252/255f,46/255f,85/255f);
		}
		else{
			bar.color = Color.yellow;
		}

		if(isShow && (unit.hp == unit.GetMaxHp() || unit.isDead)){
			isShow = false;
			anim.Play("hpbar_hide");
		}
		else if(!isShow && !(unit.hp == unit.GetMaxHp() || unit.isDead)) {
			isShow = true;
			anim.Play("hpbar_show");
		}

		if(isShow){
			transform.position = Camera.main.WorldToScreenPoint(unit.view.transform.position);
			transform.localPosition = transform.localPosition + new Vector3(0,90,0);

			displayPercent = Mathf.Lerp(displayPercent, 1.0f * unit.hp / unit.GetMaxHp(),0.1f);
			bar.fillAmount = displayPercent;
		}
	}
}
