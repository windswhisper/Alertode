using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;
using TrueSync;

public class CastSkillPanel : MonoBehaviour
{
	public Text txtTips;
	public Transform indicator;
	public BuildDetailPanel buildDetailPanel;
	BaseUltimateSkill skill;

	public void Show(BaseUltimateSkill skill,BuildDetailPanel buildDetailPanel){
		this.skill = skill;
		this.buildDetailPanel = buildDetailPanel;

		gameObject.SetActive(true);

		switch(skill.targetType){
			case TargetType.Any:
				txtTips.text = I18N.instance.getValue("@UI.SkillChooseAny");
				break;
			case TargetType.Ground:
				txtTips.text = I18N.instance.getValue("@UI.SkillChooseGround");
				break;
			case TargetType.Allies:
				txtTips.text = I18N.instance.getValue("@UI.SkillChooseAllies");
				break;
		}

		Action<Coord> callback = (Coord pos)=>{
			CastOn(pos);
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
		if(skill.unit.isDead || skill.unit.IsStunned()){
			Cancel();
		}
	}

	public void CastOn(Coord pos){
		if(skill.targetType == TargetType.Any || (skill.targetType == TargetType.Ground && BattleMap.ins.buildMap[BattleMap.ins.IndexByCoord(pos)]==0 && BattleMap.ins.fogMap[BattleMap.ins.IndexByCoord(pos)]==0 && BattleMap.ins.IsTileCanWalk(pos.x,pos.y))){
			skill.Cast(new TSVector(pos.x,0,pos.y));
			Hide();
		}
		else{
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.CantCastSkill"));
            Show(skill,buildDetailPanel);
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
		buildDetailPanel.Show((BaseBuild)skill.unit);

		BattleManager.ins.battleInput.CancelChoosePos();
	}

}
