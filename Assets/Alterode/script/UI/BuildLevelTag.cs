using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildLevelTag : MonoBehaviour
{
	public TextMesh txtLevel;

	public void SetLevel(int level){
		txtLevel.text = level+"";
	}

	void Update(){
		transform.position = transform.parent.position + new Vector3(-0.35f,0.16f,0.35f);

		if(transform.position.y<0)transform.position+=new Vector3(0,0.5f,0);	
		transform.LookAt(transform.position+new Vector3(0,0,1));
	}
}
