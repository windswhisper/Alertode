using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRecycler : MonoBehaviour
{
	public void Remove(){
		Destroy(gameObject);
	}

	public void OnParticleSystemStopped(){
		Remove();
	}
}
