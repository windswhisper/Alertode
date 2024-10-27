using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffView : MonoBehaviour
{
	//逻辑层对象
	public BaseBuff buff;

	public bool center;

	public string buffName;

	/*绑定逻辑层对象*/
	public void Bind(BaseBuff buff){
		this.buff = buff;
		buff.view = this;
		buffName = buff.data.name;
	}

	public void OnAttach(){
		transform.SetParent(buff.unit.view.transform,false);
		if(center)transform.localPosition = new Vector3(0,(float)buff.unit.radius,0);
	}


    public void Update()
    {
    	if(buff==null || buff.unit == null || buff.unit.isDead)Remove();
    }

	public void Remove(){
    	ObjectPool.ins.PutBuff(this);
	}

}
