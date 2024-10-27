using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildViewCage : BuildView
{
	public void Reset(){
    	animator.enabled = true;
		animator.Play("Reset");
	}
}
