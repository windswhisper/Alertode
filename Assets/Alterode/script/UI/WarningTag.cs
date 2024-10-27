using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningTag : MonoBehaviour
{
	MonsterGeneratorView view;
	public GameObject tag;
	public Transform tagBg;

    public void Init(MonsterGeneratorView view)
    {
        this.view = view;
    }

    void Update(){
    	Vector2 p = Camera.main.WorldToViewportPoint(view.transform.position);
    	if(p.x>0&&p.x<1&&p.y>0&&p.y<1){
    		tag.SetActive(false);
    		return;
    	}
    	else{
    		tag.SetActive(true);
    	}

    	p.x = p.x*2-1;
    	p.y = p.y*2-1;

        float deltaY = 0;

    	if(Mathf.Abs(p.x)>Mathf.Abs(p.y)){
    		if(p.x>0){
    			p.y = p.y/p.x;
    			p.x = 1;
    			tagBg.rotation = Quaternion.Euler(0,0,270);
    		}
    		else{
    			p.y = -p.y/p.x;
    			p.x = -1;
    			tagBg.rotation = Quaternion.Euler(0,0,90);
    		}
    	}
    	else{
    		if(p.y>0){
    			p.x = p.x/p.y;
    			p.y = 1;
    			tagBg.rotation = Quaternion.Euler(0,0,0);
                deltaY = 170;
    		}
    		else{
    			p.x = -p.x/p.y;
    			p.y = -1;
    			tagBg.rotation = Quaternion.Euler(0,0,180);
                deltaY = 530;
    		}
    	}

        var rate = Screen.width/1920f;
        if(Screen.width/16f>Screen.height/9f)rate = Screen.height/1080f;

    	transform.localPosition = new Vector3((Screen.width/rate-120)*p.x/2,(Screen.height/rate-120 - deltaY)*p.y/2,0);
    }

    public void Removed()
    {
		Destroy(gameObject);
    }
}
