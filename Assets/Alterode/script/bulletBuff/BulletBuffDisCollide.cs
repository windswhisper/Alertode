using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BulletBuffDisCollide : BaseBulletBuff
{
	FP time;

	public bool isDisCollide;

	public BulletBuffDisCollide(FP time){
		this.time = time;
		isDisCollide = true;
	}

	public override void OnStep(FP dt){
		base.OnStep(dt);

		time -= dt;
		if(time<0){
			isDisCollide = false;
		}
	}

	public override bool IsDisCollide(){
		return isDisCollide;
	}
}
