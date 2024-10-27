using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanel : MonoBehaviour
{
	public RectTransform container;
	public List<AchievementItem> itemList;
	public GameObject itemPrefab;
    public List<Button> btnsPage;

    int ROWS_IN_PAGE = 6;

    void OnEnable()
    {
        ShowPage(0);
    }

    public void ShowPage(int page){
        for(int i=0;i<container.childCount;i++){
    		Destroy(container.GetChild(i).gameObject);
    	}

        for(int i=page*ROWS_IN_PAGE;i<UserData.data.achieveProgressDatas.Count && i<(page+1)*ROWS_IN_PAGE;i++){
    	   var achieve = UserData.data.achieveProgressDatas[i];
    		var item = Instantiate(itemPrefab);
    		item.GetComponent<AchievementItem>().Init(this,achieve);
    		itemList.Add(item.GetComponent<AchievementItem>());
    		item.transform.SetParent(container,false);
    	}

        foreach(var btn in btnsPage){
            btn.interactable = true;
        }
        btnsPage[page].interactable = false;
    }
}
