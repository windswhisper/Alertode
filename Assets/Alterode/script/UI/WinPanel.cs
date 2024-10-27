using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Honeti;

public class WinPanel : MonoBehaviour
{
	public Text txtMsg;

	public GameObject gemRerward;
	public GameObject siliconRerward;
	public GameObject metalRerward;

	public Text gemCount;
	public Text siliconCount;
	public Text metalCount;

	public GameObject newBuildPanel;
    public GameObject btnAdDouble;
    public GameObject tagDouble;

	string unlockBuild;

    int gem;
    int silicon;
    int metal;

 	public void Show(){
		VolumeManager.ins.PauseBgm();
 		gameObject.SetActive(true);

 		txtMsg.text = string.Format(I18N.instance.getValue("@UI.StageComplete"),I18N.instance.getValue(Configs.GetStage(GlobalData.selectedChapter,GlobalData.selectedStage).uiName),GlobalData.selectedDifficult);
 	
 		var waveFactor = 0;
 		if(BattleMap.ins.mapData.wave < 8){
 			waveFactor = 0;
 		}else if(BattleMap.ins.mapData.wave < 12){
 			waveFactor = 40;
 		}else{
 			waveFactor = 60;
 		}

 		gem = 20*(100+GlobalData.selectedDifficult*20)/100;
 		silicon = (3+6*(100+GlobalData.selectedDifficult*10)/100*(100+waveFactor*2)/100)/2;
 		metal = (4+6*(100+GlobalData.selectedDifficult*16)/100*(100+waveFactor)/100)/2 ;
        

        if(UserData.data.stageProgress[GlobalData.selectedChapter+"-"+GlobalData.selectedStage] <= GlobalData.selectedDifficult){
            gem*=5;
            silicon*=5;
            metal*=5;
        }

        #if UNITY_ANDROID || UNITY_IPHONE

            btnAdDouble.SetActive(true);
            gem/=2;
            silicon = silicon*2/3;
            metal = metal*2/3;

        #endif
        
 		UserData.data.gem+=gem;
 		UserData.data.silicon+=silicon;
 		UserData.data.metal+=metal;

 		gemCount.text = "x"+gem;
 		siliconCount.text = "x"+silicon;
 		metalCount.text = "x"+metal;

        if(GlobalData.selectedChapter == 0)GlobalData.selectedDifficult = 3;
        
        if(UserData.data.stageProgress[GlobalData.selectedChapter+"-"+GlobalData.selectedStage] <= GlobalData.selectedDifficult){
            UserData.data.stageProgress[GlobalData.selectedChapter+"-"+GlobalData.selectedStage] = GlobalData.selectedDifficult+1;

            if(GlobalData.selectedChapter == 0 && GlobalData.selectedStage==0){
                foreach(var build in UserData.data.buildsCollect){
                    if(build.buildId == "Snipe")return;
                }
            	UserData.data.buildsCollect.Add(new TeamBuildData("Snipe"));
            	unlockBuild = "Snipe";
            	Invoke("ShowUnlock",3);
            }
            if(GlobalData.selectedChapter == 0 && GlobalData.selectedStage==1)
            {
                foreach(var build in UserData.data.buildsCollect){
                    if(build.buildId == "Mort")return;
                }
            	UserData.data.buildsCollect.Add(new TeamBuildData("Mort"));
            	unlockBuild = "Mort";
            	Invoke("ShowUnlock",3);
            }
            if(GlobalData.selectedChapter == 0 && GlobalData.selectedStage==2)
            {
                foreach(var build in UserData.data.buildsCollect){
                    if(build.buildId == "Tesla")return;
                }
            	UserData.data.buildsCollect.Add(new TeamBuildData("Tesla"));
            	unlockBuild = "Tesla";
            	Invoke("ShowUnlock",3);
            }
        }
 	}

 	public void ShowUnlock(){
        var panel = Instantiate(newBuildPanel);
        panel.GetComponent<NewBuildPanel>().Show(unlockBuild);
        panel.transform.SetParent(transform.parent,false);
 	}

 	public void Home(){
        LoadingMask.ins.LoadSceneAsync("StageScene");
 	}

    public void PlayAd(){
        MobileAdManager.ins.ShowRewardedAd(()=>{
            AdReward();
            });
    }

    public void AdReward(){

        UserData.data.gem+=gem;
        UserData.data.silicon+=silicon;
        UserData.data.metal+=metal;
        UserData.Save();

        gem*=2;
        silicon*=2;
        metal*=2;

        gemCount.text = "x"+gem;
        siliconCount.text = "x"+silicon;
        metalCount.text = "x"+metal;

        tagDouble.SetActive(true);
        btnAdDouble.SetActive(false);

    }
}
