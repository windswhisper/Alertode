using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrailer : MonoBehaviour
{
	public Transform parent;
	public TrailRenderer trailRenderer;

	float originWidth;

	void Start(){
		originWidth = trailRenderer.startWidth;
	}

    void Update()
    {
        trailRenderer.startWidth = originWidth*parent.localScale.x;
    }
}
