using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

public class HeroMovePanel : MonoBehaviour
{
	public Transform indicator;
	public BuildDetailPanel buildDetailPanel;
	BaseHero hero;

	public void Show(BaseHero hero,BuildDetailPanel buildDetailPanel){
		this.hero = hero;
		this.buildDetailPanel = buildDetailPanel;

		gameObject.SetActive(true);

		Action<Coord> callback = (Coord pos)=>{
			MoveTo(pos);
		};

		Action<Coord> hoverCallback = (Coord pos)=>{
			Hover(pos);
		};

		Action cancelCallback = ()=>{
			Hide();
		};

		BattleManager.ins.battleInput.ChoosePos(callback,hoverCallback,cancelCallback);
	}

	void Update(){
		if(hero.isDead || hero.IsStunned()){
			Cancel();
		}
	}

	public void MoveTo(Coord pos){
		if(hero.MoveTo(pos)){
			Hide();
		}
		else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.HeroCantMove"));
            Show(hero,buildDetailPanel);
		}
	}

	public void Hover(Coord pos){
		if(Utils.IsCoordInMap(pos)){
			indicator.gameObject.SetActive(true);
			indicator.localPosition = new Vector3(pos.x,0,pos.y);
		}
		else{
			indicator.gameObject.SetActive(false);
		}
	}

	public void Hide(){
		indicator.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	public void Cancel(){
		Hide();
		buildDetailPanel.Show(hero);

		BattleManager.ins.battleInput.CancelChoosePos();
	}

}
