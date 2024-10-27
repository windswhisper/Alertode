using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLabel : MonoBehaviour
{
	public TextMesh text;

	public void Init(int value,int type){
		text.text = value+"";
	}
}
