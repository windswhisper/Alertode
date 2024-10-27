using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class BuildMiner : BaseBuild
{	
	public int productDelta;
	public int coinAmount;

	FP minerTimer;

	public override void Init(BuildData data){
		base.Init(data);

		productDelta = data.paras[0];
		coinAmount = data.paras[1];

		minerTimer = 0;
	}

    public override void Step(FP dt){
    	base.Step(dt);

    	if(isDead)return;
        if(BattleManager.ins.engine.time > BattleManager.ins.engine.nightTime*2)return;
    	minerTimer+=dt;
    	if(minerTimer>productDelta){
    		ProductCoin();
    		minerTimer-=productDelta;
    	}
    }

    public int GetProductAmount(){
        var coin = coinAmount*Configs.commonConfig.upgradeMinerRate[level]/100;

        var plusNum = 0;
        var factor = 100;
        foreach(var chip in chips){
            factor+=chip.OnGetMoneyIncreaseFactor();
            plusNum+=chip.OnGetMoneyIncreaseNum();
        }
        
        coin = (coin+plusNum)*factor/100;

        return coin;
    }

    public void ProductCoin(){
        var coin = GetProductAmount();

    	BattleManager.ins.engine.AddCoin(coin);
        ((BuildViewMiner)view).ProductCoin(coin);
    }
}
