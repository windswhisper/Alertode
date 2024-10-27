using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class NewBuildPanel : MonoBehaviour
{
	public Text txtTitle;
	public Text txtName;
	public Text txtDesc;
	public Image icon; 

	public void Show(string buildId){
		var data = Configs.GetBuild(buildId);

		icon.sprite = Resources.Load<Sprite> ("image/build/"+buildId);
		txtName.text = I18N.instance.getValue(data.uiName);
		txtDesc.text = I18N.instance.getValue(data.uiName+"_Desc");

        AchievementManager.OnGetBuild();
        GlobalData.newBuildHint.Add(buildId);
	}
	public void ShowHero(string heroId){
		var data = Configs.GetHero(heroId);

		icon.sprite = Resources.Load<Sprite> ("image/hero/"+heroId);
		txtName.text = I18N.instance.getValue(data.uiName+"_Title")+" "+I18N.instance.getValue(data.uiName);
		txtDesc.text = I18N.instance.getValue(data.uiName+"_Desc");

        AchievementManager.OnGetHero();
        GlobalData.newHeroHint.Add(heroId);

        txtTitle.text = I18N.instance.getValue("@UI.NewHero");
	}
}
