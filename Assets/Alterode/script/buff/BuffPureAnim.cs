using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuffPureAnim : BaseBuff
{
	public string effectName;

	public void SetAnim(string effectName){
		this.effectName = effectName;
	}

	public override void OnAttach(BaseUnit unit,FP duration){
		base.OnAttach(unit,duration);

		if(view!=null)((BuffPureAnimView)view).SetAnim(effectName);
	}
}
