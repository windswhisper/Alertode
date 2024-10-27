using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDelayPlay : MonoBehaviour
{
	public AudioSource audio;

	public float delay;

	void Start(){
		StartCoroutine(Play());
	}

	IEnumerator Play(){
		yield return new WaitForSeconds(delay);

		audio.Play();
	}
}
