using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单位视图
public class UnitView : MonoBehaviour
{

	//炮塔
	public GameObject turret;
	//渲染器列表
	public List<MeshRenderer> meshRendererList;
	//渲染器列表
	public List<SkinnedMeshRenderer> skinnedMeshRendererList;
	//逻辑层对象
	public BaseUnit unit;

	public float whiteStrength = 0;

	public float removeTime = 0;

    protected virtual void Start(){

    }
    
	/*绑定逻辑层对象*/
	public virtual void Bind(BaseUnit unit){
		this.unit = unit;
		unit.view = this;

		if(unit.type != UnitType.Monster || !((BaseMonster)unit).isBoss)
		{
        	//if(PlaySetting.FXLevel>1){
				var hpBar = Instantiate(BattleManager.ins.hpBarPrefab);
				hpBar.GetComponent<HpBar>().Bind(this.unit);
				hpBar.transform.SetParent(BattleManager.ins.hpBarLayer,false);
        	//}
		}
	}

	protected virtual void Update(){
		if(whiteStrength>0){
			whiteStrength -= Time.deltaTime/0.1f;
			if(whiteStrength<0){
				whiteStrength = 0;
				foreach(var renderer in meshRendererList){
					renderer.materials[0].SetFloat("_WhiteStrength",0f);
				}
				foreach(var renderer in skinnedMeshRendererList){
					renderer.materials[0].SetFloat("_WhiteStrength",0f);
				}
			}
			else{
				foreach(var renderer in meshRendererList){
					renderer.materials[0].SetFloat("_WhiteStrength",whiteStrength);
				}
				foreach(var renderer in skinnedMeshRendererList){
					renderer.materials[0].SetFloat("_WhiteStrength",whiteStrength);
				}
			}
		}
	}

	//播放开火特效
	public virtual void PlayMuzzleAnim(string muzzleFxName,Vector3 pOrigin,Vector3 p,float rotation)
	{
		if(muzzleFxName=="")return;
    	var effect = BattleManager.ins.PlayEffect(muzzleFxName,pOrigin);
    	if(effect==null)return;
    	if(unit.hasTurret&&turret!=null){
	    	effect.transform.SetParent(turret.transform,false);
	    	effect.transform.localPosition = pOrigin;
    	}
    	else if(unit.hasTurret&&turret==null){
	    	effect.transform.SetParent(transform,false);
	    	effect.transform.localPosition = p;

    		effect.transform.rotation = Quaternion.Euler(0,rotation,0);
    	}
    	else{
	    	effect.transform.SetParent(transform,false);
	    	effect.transform.localPosition = pOrigin;
    	}
	}

	/*播放受伤动画*/
	public virtual void PlayHurtAnim(){
		foreach(var renderer in meshRendererList){
			renderer.materials[0].SetFloat("_WhiteStrength",0.9f);
		}
		foreach(var renderer in skinnedMeshRendererList){
			renderer.materials[0].SetFloat("_WhiteStrength",0.9f);
		}
		whiteStrength = 0.9f;
	}

	public void RemoveDelay(float t){
		removeTime = t;
	}


	/*被移除*/
	public virtual void Removed(){
		Destroy(gameObject);
	}
}
