using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子弹视图
public class BulletView : MonoBehaviour
{
	//逻辑层对象
	public BaseBullet bullet;

	float t;
	public float removeDelay;

	Vector3 lastPos;

	/*绑定逻辑层对象*/
	public void Bind(BaseBullet bullet){
		this.bullet = bullet;
		bullet.view = this;
	}

	/*进入场景*/
	public virtual void Enter(){
		lastPos = transform.localPosition =  Utils.TSVecToVec3(bullet.position);
		var p = Utils.TSVecToVec3(bullet.targetPos);
		p.y = transform.localPosition.y;
		if(!bullet.data.fixedRotate)transform.LookAt(p);
		t=0;
		gameObject.SetActive(true);
	}

	/*每帧更新*/
    public virtual void Update()
    {
    	if(bullet==null || bullet.isMissed){
    		t+=Time.deltaTime;
    		if(t>removeDelay)
    			Remove();
			else{
    			transform.localPosition = Vector3.Lerp(transform.localPosition,lastPos,Utils.LerpByTime(Time.deltaTime));
			}
    		return;
    	}

    	if(!bullet.data.fixedRotate)transform.LookAt(Utils.TSVecToVec3(bullet.position));
    	transform.localPosition = Vector3.Lerp(transform.localPosition,Utils.TSVecToVec3(bullet.position),Utils.LerpByTime(Time.deltaTime));
    	lastPos = Utils.TSVecToVec3(bullet.position);

		transform.localScale = new Vector3(1, 1, 1) * ((bullet.largePercent + 100f) / 100);
	}

    /*击中*/
    public virtual void Hit(BaseUnit target){
    	if(bullet.hitFX!=""){
	    	if(target!=null){
	    		BattleManager.ins.PlayEffect(bullet.hitFX,Utils.TSVecToVec3(target.position));
	    	}
	    	else {
	    		BattleManager.ins.PlayEffect(bullet.hitFX,Utils.TSVecToVec3(bullet.position));
	    	}
    	}
    }

    /*爆炸*/
    public virtual void Explodes(){
    	if(bullet.explodeFX!=""){
	    	var effect =BattleManager.ins.PlayEffect(bullet.explodeFX,Utils.TSVecToVec3(bullet.position));
	    	effect.transform.localScale = new Vector3(1,1,1)*((bullet.largePercent+100f)/100);
    	}
    }

    public void Remove(){
    	ObjectPool.ins.PutBullet(this);
    }
}
