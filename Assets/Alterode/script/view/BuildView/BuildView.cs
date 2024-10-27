using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//建筑视图
public class BuildView : UnitView
{
	//动画
	public Animator animator;
    AnimatorSpeeder animatorSpeeder;
	//逻辑层对象
	public BaseBuild build;

    public BuildLevelTag levelTag;

    bool isSold = false;

    void Start(){
        animatorSpeeder = animator.gameObject.GetComponent<AnimatorSpeeder>();
        var tag = Instantiate(Resources.Load<GameObject>("prefab/buildLevelTag"));
        levelTag = tag.GetComponent<BuildLevelTag>();
        tag.transform.SetParent(transform,false);
    }
    
	/*绑定逻辑层对象*/
	public void Bind(BaseBuild build){
		base.Bind(build);
		this.build = build;
		build.view = this;
	}

	/*每帧更新*/
    protected override void Update()
    {
        if(isSold){
            removeTime-=Time.deltaTime * BattleManager.ins.GetGameSpeed();
            if(removeTime<=0)Removed();
        }
        if(build.isDead)return;
    	base.Update();
        if(turret!=null && build.hasTurret && (build.weapons.Count==0 || build.fireCd>0)){
        	Utils.RotateSmooth(turret.transform,new Vector3(0,(float)build.turretRotation*180/Mathf.PI,0),Utils.LerpByTime(Time.deltaTime));
        }

        var animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("Attack")){     
            animatorSpeeder.speedRate = (build.GetFireSpeedIncreasePercent()+100f)/100f;
        }
        else{
            animatorSpeeder.speedRate = 1;
        }

        levelTag.SetLevel(build.level);
    }

	/*播放建造动画*/
    public void BuildUp(){
    	if(animator!=null)
    	{
    		animator.enabled = true;
			animator.Play("BuildUp");
    	}
    }

	/*播放开火动画*/
    public void Fire(){
    	if(animator!=null && animator.HasState(0,Animator.StringToHash("Attack")))
    	{
    		animator.enabled = true;
	    	animator.Play("Attack");
	    }
    }

    /*播放升级动画*/
    public void Upgrade(){
        var effect = GameObject.Instantiate(Resources.Load<GameObject>("prefab/effect/EffectUpgrade"));
        effect.transform.SetParent(transform.parent,false);
        effect.transform.position = transform.position;
    }

    /*播放变卖动画*/
    public void Sell(){
        levelTag.gameObject.SetActive(false);
    	if(animator!=null )
    	{
    		animator.enabled = true;
	    	animator.Play("Sell");
            isSold = true;
	    	RemoveDelay(1.5f);
	    }
	    else{
	    	Removed();
	    }
    }

    /*被摧毁*/
    public void Destroyed(){
    	if(animator!=null){
    		animator.enabled = true;
    		animator.Play("Die");
            levelTag.gameObject.SetActive(false);
    	}
        foreach(var renderer in meshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0.25f);
            renderer.materials[0].SetColor("_Color",new Color(0.15f,0.15f,0.15f));
        }
        foreach(var renderer in skinnedMeshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0.25f);
            renderer.materials[0].SetColor("_Color",new Color(0.15f,0.15f,0.15f));
        }
        whiteStrength = 0.5f;

        var effect = GameObject.Instantiate(Resources.Load<GameObject>("prefab/effect/EffectBuildDestroyed"));
        effect.transform.SetParent(transform.parent,false);
        effect.transform.position = transform.position;
    }

    /*重建*/
    public void Rebuild(){
    	if(animator!=null)
    	{
    		animator.enabled = true;
			animator.Play("BuildUp");
            levelTag.gameObject.SetActive(true);
    	}
        foreach(var renderer in meshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0f);
            renderer.materials[0].SetColor("_Color",new Color(1f,1f,1f));
        }
        foreach(var renderer in skinnedMeshRendererList){
            renderer.materials[0].SetFloat("_WhiteStrength",0f);
            renderer.materials[0].SetColor("_Color",new Color(1f,1f,1f));
        }
    }
}
