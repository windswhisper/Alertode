using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneratorView : MonoBehaviour
{
	public MonsterGenerator generator;
	public WarningTag warningTag;
	public ParticleSystem sign;

	public Material imgFly;
	public Material imgBoss;

	int i=0;

	void Start(){
		var tag = Instantiate(Resources.Load<GameObject>("prefab/warningTag"));
		warningTag = tag.GetComponent<WarningTag>();
		tag.transform.SetParent(BattleManager.ins.warningLayer,false);
		warningTag.Init(this);
	}

	/*绑定逻辑层对象*/
	public void Bind(MonsterGenerator generator){
		this.generator = generator;
		generator.view = this;

		if(generator.isBoss()){
			sign.GetComponent<Renderer>().material = imgBoss;
		}
		else if(generator.hasFlyMonster()){
			sign.GetComponent<Renderer>().material = imgFly;
		}
	}

	/*移除自身*/
	public void Removed(){
		if(warningTag!=null)warningTag.Removed();
		Destroy(gameObject);
	}
}
