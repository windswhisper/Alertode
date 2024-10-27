using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ItemChapter : MonoBehaviour
{
	StageScene stageScene;
	public int chapterId = 0;
	public Button button;
	public Text txtChapter;

	public GameObject star;
	public GameObject starFull;

	bool isShow = false;

	public void Init(StageScene stageScene,int chapterId,string chapterName){
		this.stageScene = stageScene;
		this.chapterId = chapterId;
		this.txtChapter.text = I18N.instance.getValue(chapterName);

		bool isAllWin = true;
		bool isAllFull = true;

        foreach(var chapter in Configs.stageConfig.chapters){
        	if(chapter.id == chapterId){
	            foreach(var stageData in chapter.stages){
					if(UserData.data.stageProgress[chapterId+"-"+stageData.id]==0){
						isAllWin = false;
					}
					if(UserData.data.stageProgress[chapterId+"-"+stageData.id]<4){
						isAllFull = false;
					}
				}
        	}
		}

		star.SetActive(!isAllFull&&isAllWin);
		starFull.SetActive(isAllFull);
	}

	public void Selected(){
		isShow = !isShow;
		stageScene.SelectChapter(chapterId,isShow);
	}
}
