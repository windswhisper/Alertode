using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BaseMapScript
{
	public int x;
	public int y;
	public int intPara;
	public string namePara;
	public bool enable = true;
	public bool isRemove = false;

	public void Init(int x,int y,int intPara,string namePara){
		this.x = x;
		this.y = y;
		this.intPara = intPara;
		this.namePara = namePara;

		OnInit();
	}

	virtual public void OnInit(){}

	virtual public void Step(FP dt){}

	virtual public void OnDayStart(){}

	virtual public void OnNightStart(){}
}
