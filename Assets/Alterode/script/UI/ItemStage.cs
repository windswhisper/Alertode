using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ItemStage : MonoBehaviour
{
	StageScene stageScene;
	public Text txtName;
	public Button button;
	public StageData data;

	public GameObject icoLock;
	public GameObject icoStar;
	public GameObject icoStarFull;

	public int chapterId;

	public void Init(StageScene stageScene,int chapterId,StageData data){
		this.stageScene = stageScene;
		this.chapterId = chapterId;
		this.data = data;
		this.txtName.text = I18N.instance.getValue(data.uiName);

		if(data.id !=0 && !UserData.data.stageProgress.ContainsKey(chapterId+"-"+(data.id-1))){
			UserData.data.stageProgress.Add(chapterId+"-"+(data.id-1),0);
		}
		if(!UserData.data.stageProgress.ContainsKey(chapterId+"-"+data.id)){
			UserData.data.stageProgress.Add(chapterId+"-"+data.id,0);
		}
		if(!UserData.data.stageProgress.ContainsKey("0-2")){
			UserData.data.stageProgress.Add("0-2",0);
		}

		int easyWin = 0;
		int normalWin = 0;
        foreach(var chapter in Configs.stageConfig.chapters){
        	if(chapter.id == 1){
			    foreach(var stageData in chapter.stages){
					if(UserData.data.stageProgress[chapter.id+"-"+stageData.id]!=0){
						easyWin++;
					}
				}
			}
        	if(chapter.id == 2){
			    foreach(var stageData in chapter.stages){
					if(UserData.data.stageProgress[chapter.id+"-"+stageData.id]!=0){
						normalWin++;
					}
				}
			}
		}

		if((chapterId == 0 && data.id !=0 && UserData.data.stageProgress[chapterId+"-"+(data.id-1)]==0 ) || ((chapterId!=0 && UserData.data.stageProgress["0-2"]==0)
)){
			button.enabled = false;
			icoLock.SetActive(true);
			button.GetComponent<Image>().color = new Color(0,0,0);
		}else if(chapterId == 2 && easyWin<3 || chapterId == 3 && normalWin<3){
			button.enabled = false;
			icoLock.SetActive(true);
			button.GetComponent<Image>().color = new Color(0,0,0);
		}
		else if(UserData.data.stageProgress[chapterId+"-"+data.id]>0){
			if(UserData.data.stageProgress[chapterId+"-"+data.id]>3){
				icoStarFull.SetActive(true);
			}
			else{
				icoStar.SetActive(true);
			}
		}
	}

	public void Selected(){
		button.interactable = false;
		stageScene.SelectStage(chapterId,data);
	}

	public void Unselected(){
		button.interactable = true;
	}
}
