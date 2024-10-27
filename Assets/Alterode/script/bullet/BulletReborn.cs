using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReborn : BulletInviso
{
	public int hpRate = 30;

	public override void Explodes(){
		base.Explodes();

		((BaseBuild)target).Rebuild();
		target.hp = target.GetMaxHp()*hpRate/100;
    }
}
