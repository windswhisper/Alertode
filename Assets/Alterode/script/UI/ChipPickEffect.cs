using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipPickEffect : MonoBehaviour
{
	public Image glow;

	Vector3 targetPos;

    void Start()
    {
    	var target =  GameObject.Find("BtnChip");

    	if(target!=null)
    	{
        	targetPos = target.transform.position;
    	}
    	else{
    		targetPos = transform.position;
    	}
    }

    void Update()
    {
    	var dt = Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position,targetPos,dt*2);

        if(Vector3.Distance(targetPos,transform.position) < 2)Destroy(gameObject);
    }

	public void Init(BaseChip chip){
		switch(chip.data.rare){
			case 0:
				glow.color = new Color(0.88f,0.88f,1f,0.9f);
				break;
			case 1:
				glow.color = new Color(0.9f,0.3f,0.9f,0.9f);
				break;
			case 2:
				glow.color = new Color(1f,1f,0.3f,0.9f);
				break;
		}
	}
}
