using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{

	public static CameraEffect ins;

	public GameObject firework;
	public Animation anim;

    void Start()
    {
        ins = this;
    }

    public void Shake(bool isShort){
    	if(isShort){
            anim.Play("camera_shake_s");
            anim["camera_shake_s"].time = 0;
            anim.Sample();  
        }
        else{
            anim.Play("camera_shake");
            anim["camera_shake"].time = 0;
            anim.Sample();  
        }
    }

    public void LaunchFirework(){
    	firework.SetActive(true);
    }
}
