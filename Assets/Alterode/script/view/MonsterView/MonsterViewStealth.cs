using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewStealth : MonsterView
{
	public void Jump(){
		animator.Play("Jump");
	}
}
