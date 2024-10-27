using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipFreeOnce : BaseChip
{
	int count;

	public override void Init(ChipData data){
		base.Init(data);
		
		count = data.paras[0];
	}

	public override int OnGetSelfCostMutipleFactorGlobal(){
		if(count > 0){
			return -100;
		}
		return 0;
	}
	public override void OnBoughtGlobal(BaseBuild build){
		count--;
	}
	
	public override bool HasExPara(){
		return true;
	}
	public override int GetExtraPara(){
		return count;
	}
	public override void SetExtraPara(int para){
		count = para;
	}
}
