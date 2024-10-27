using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPureAnimView : BuffView
{
	public void SetAnim(string animName){
	    var effect = ObjectFactory.CreateEffect(animName);
	    effect.transform.SetParent(transform,false);
	}
}
