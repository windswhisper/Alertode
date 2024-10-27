using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物视图
public class MonsterView : UnitView
{
	//动画
	public Animator animator;
    AnimatorSpeeder animatorSpeeder;
	//逻辑层对象
	public BaseMonster monster;

    public List<ParticleSystem> tailerWalk;
    public List<ParticleSystem> tailerDive;


    [HideInInspector]
    public bool isCaged = false;

    
    protected override void Start(){
        animatorSpeeder = animator.gameObject.GetComponent<AnimatorSpeeder>();
    }

	/*绑定逻辑层对象*/
	public void Bind(BaseMonster monster){
		base.Bind(monster);
		this.monster = monster;
		monster.view = this;
        isCaged = false;
	}

	/*每帧更新*/
    protected override void Update()
    {
        if(monster==null || monster.isDead){
            if(isCaged){
                if(transform.localPosition.y>-2)transform.localPosition = transform.localPosition+new Vector3(0,-2f*Time.deltaTime,0);
                if(transform.localScale.x>0)transform.localScale = transform.localScale+new Vector3(-4f*Time.deltaTime,-4f*Time.deltaTime,-4f*Time.deltaTime);
            }
            else{
                if(transform.localPosition.y>0){
                    transform.localPosition = transform.localPosition+new Vector3(0,-1f*Time.deltaTime,0);
                }
                else if(transform.localPosition.y>-2){
                    transform.localPosition = transform.localPosition+new Vector3(0,-0.2f*Time.deltaTime,0);
                }
            }
            removeTime-=Time.deltaTime * BattleManager.ins.GetGameSpeed();
            if(removeTime<=0)Removed();
            return;
        }

        base.Update();
        
        if(turret!=null && monster.hasTurret && (monster.weapons.Count==0 || monster.fireCd>0)){
        	Utils.RotateSmooth(turret.transform,new Vector3(0,(float)monster.turretRotation*180/Mathf.PI,0),Utils.LerpByTime(Time.deltaTime));
        }
        Utils.RotateSmooth(transform,new Vector3(0,(float)monster.yRotation*180/Mathf.PI,0),Utils.LerpByTime(Time.deltaTime));

        var distance = Vector3.Distance(transform.localPosition, Utils.TSVecToVec3(monster.position));
    	if(distance > 0.01f && distance<5){
    		transform.localPosition = Vector3.Lerp(transform.localPosition,Utils.TSVecToVec3(monster.position),Utils.LerpByTime(Time.deltaTime));
    	}
    	else{
    		transform.localPosition = Utils.TSVecToVec3(monster.position);

    	}
        animator.SetBool("moving",monster.isMoving && !monster.IsStunned());

        foreach(var tailer in tailerWalk){
            var e = tailer.emission;
            e.enabled = (monster.IsOnGround() && monster.isMoving);
        }
        foreach(var tailer in tailerDive){
            var e = tailer.emission;
            e.enabled = (monster.IsUnderGround() && monster.isMoving);
        }

        var animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("Move")){
            animatorSpeeder.speedRate = (monster.GetMoveSpeedIncreasePercent()+100f)/100f;
        }
        else if (animatorInfo.IsName("Attack")){
            animatorSpeeder.speedRate = (monster.GetFireSpeedIncreasePercent()+100f)/100f;
        }
        else{
            animatorSpeeder.speedRate = 1;
        }
    }

    public void PlayAttackAnim(){
    	animator.Play("Attack");
    }

    public virtual void PlayDieAnim(){
        animator.Play("Die");
        foreach(var renderer in meshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0.25f);
            renderer.materials[0].SetColor("_Color",new Color(0.15f,0.15f,0.15f));
        }
        foreach(var renderer in skinnedMeshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0.25f);
            renderer.materials[0].SetColor("_Color",new Color(0.15f,0.15f,0.15f));
        }
        RemoveDelay(2);
    }

    public virtual void PlayRetreatAnim(){
        animator.Play("Die");
        RemoveDelay(2);
    }

    public void PlaySpellAnim(){
        animator.Play("Spell");
    }
}
