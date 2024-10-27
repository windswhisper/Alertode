using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TechBuildItem : MonoBehaviour
{
	public TechPanel techPanel;
	public string buildID;

	public Text txtName;
	public Text txtDesc;
	public Image icon;
	public Animation anim;
	public GameObject btnReroll;


	public void Init(string buildID){
		this.buildID = buildID;
		gameObject.SetActive(true);

		var buildData = Configs.GetBuild(buildID);
		txtName.text = I18N.instance.getValue(buildData.uiName);
		txtDesc.text = I18N.instance.getValue(buildData.uiName+"_Desc");

		icon.sprite = Resources.Load<Sprite>("image/build/"+buildID);
		
		anim.Play();
	}

}
