using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewGiantDragon : MonsterView
{
	public RuntimeAnimatorController controller1;
	public RuntimeAnimatorController controller2;
	public AudioSource soundCry;

	public void Hurt(){
		animator.Play("Hurt");
		soundCry.Play();
	}

	public void Crash(){
		animator.runtimeAnimatorController = controller2;
        CameraEffect.ins.Shake(false);
	}

	public void ReFly(){
		soundCry.Play();
		animator.runtimeAnimatorController = controller1;
        CameraEffect.ins.Shake(false);
	}
}
