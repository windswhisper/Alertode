using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterViewBomber : MonsterView
{

	/*每帧更新*/
    protected override void Update()
    {
    	var y = transform.localPosition.y;

        base.Update();

        if(monster.isDead)transform.localPosition = new Vector3(transform.localPosition.x,y,transform.localPosition.z);
    }

    public override void PlayDieAnim(){
    	base.PlayDieAnim();
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
