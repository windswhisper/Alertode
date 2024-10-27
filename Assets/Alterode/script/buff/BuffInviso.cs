using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffInviso : BaseBuff
{
	public override bool OnGetInviso(){
		return true;
	}

	public override int OnGetHurtIncreasePercent(int damage, BaseUnit damageSource){
		return -100;
	}
}
