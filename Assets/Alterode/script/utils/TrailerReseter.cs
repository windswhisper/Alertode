using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerReseter : MonoBehaviour
{
	public TrailRenderer trail;

	void OnEnable(){
		trail.Clear();
	}
}
