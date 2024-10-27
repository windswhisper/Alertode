using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewScorpionKing : MonsterViewStealth
{
    protected override void Start(){
    	base.Start();
		var effect = ObjectFactory.CreateEffect("EffectSandStorm");
		effect.transform.SetParent(transform.parent,false);
		effect.transform.position = new Vector3(BattleMap.ins.mapData.width/2,1,BattleMap.ins.mapData.height/2);

		effect = ObjectFactory.CreateEffect("EffectSandDive");
		effect.transform.SetParent(transform.parent,false);
		effect.transform.position = transform.position;
	}

	public void Stealth(){
		animator.Play("Spell");
		
		var effect = ObjectFactory.CreateEffect("EffectSandDive");
		effect.transform.SetParent(transform.parent,false);
		effect.transform.position = transform.position;
	}

	public void Jump2(){
		animator.Play("Jump");
		
		var effect = ObjectFactory.CreateEffect("EffectSandDive");
		effect.transform.SetParent(transform.parent,false);
		effect.transform.position = transform.position;
	}
}
