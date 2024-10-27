using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

//英雄视图
public class HeroView : BuildView
{
    public AudioSource audioSource;

    protected override void Update()
    {
    	if(build.isDead)return;
    	base.Update();
        Utils.RotateSmooth(transform,new Vector3(0,(float)build.yRotation*180/Mathf.PI,0),Utils.LerpByTime(Time.deltaTime));

    	if(Vector3.Distance(transform.localPosition, Utils.TSVecToVec3(build.position))> 0.01f){
    		transform.localPosition = Vector3.Lerp(transform.localPosition,Utils.TSVecToVec3(build.position),Utils.LerpByTime(Time.deltaTime));
    	}
    	else{
    		transform.localPosition = Utils.TSVecToVec3(build.position);
    	}
        animator.SetBool("moving",((BaseHero)build).isMoving && !build.IsStunned());
    }

    public void MoveImmediately(){
        transform.localPosition = Utils.TSVecToVec3(build.position);
    }

    public void CastSkill(){
        animator.Play("Skill");
    }

    public void PlayEnterVoice(){
        audioSource.clip = Resources.Load<AudioClip>("voice/"+I18N.instance.gameLang+"/"+build.data.name+"_enter");
        audioSource.Play();
    }

    public void PlayMoveVoice(){
        audioSource.clip = Resources.Load<AudioClip>("voice/"+I18N.instance.gameLang+"/"+build.data.name+"_move");
        audioSource.Play();
    }

    public void PlayDieVoice(){
        audioSource.clip = Resources.Load<AudioClip>("voice/"+I18N.instance.gameLang+"/"+build.data.name+"_die");
        audioSource.Play();
    }

    public void PlaySkillVoice(string skillName){
        audioSource.clip = Resources.Load<AudioClip>("voice/"+I18N.instance.gameLang+"/skill_"+skillName);
        audioSource.Play();
    }
}
