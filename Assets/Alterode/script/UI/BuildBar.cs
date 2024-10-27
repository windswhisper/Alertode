using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class BuildBar : MonoBehaviour
{
	public static BuildBar ins;

	string heroID;
	bool isPutHero;
	public List<string> buildIDList = new List<string>();
	List<BuildBarItem> itemList = new List<BuildBarItem>();
	public string selectId;
	public bool selectHero;

	public RectTransform itemContainer;
	public GameObject itemPrefab;
	public Transform buildPreview;
	public Transform buildPreviewRange;

	public GameObject buildInfoTips;
	public Text txtBuildInfoName;
	public Text txtBuildInfoDesc;

	void Awake(){
		ins = this;
    	isPutHero = false;
	}

    void Start()
    {

    	if(!BattleManager.ins.isLoadGame){
	    	heroID = UserData.data.heroUsing;

	    	if(BattleManager.ins.gameMode == 0)
			{
				foreach(var build in UserData.data.buildsCollect){
	                if(!build.isUsing)continue;
		    		buildIDList.Add(build.buildId);
		    	}
			}    	
			else{
				buildIDList.Add("Miner");
				if(BattleManager.ins.gameMode == 2){
					buildIDList.Add("Barr");
					BattleManager.ins.AddNewTech("Barr","Ban");
					BattleManager.ins.AddNewTech("Barr","Ban");
					BattleManager.ins.AddNewTech("Barr","Ban");
				}
			}
    	}

        UpdateList();
    }

	void Update(){
		if(Input.GetKeyUp(KeyCode.Alpha1) && buildIDList.Count>0){
			SelectItem(buildIDList[0],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha2) && buildIDList.Count>1){
			SelectItem(buildIDList[1],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha3) && buildIDList.Count>2){
			SelectItem(buildIDList[2],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha4) && buildIDList.Count>3){
			SelectItem(buildIDList[3],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha5) && buildIDList.Count>4){
			SelectItem(buildIDList[4],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha6) && buildIDList.Count>5){
			SelectItem(buildIDList[5],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha7) && buildIDList.Count>6){
			SelectItem(buildIDList[6],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha8) && buildIDList.Count>7){
			SelectItem(buildIDList[7],false);
		}
		else if(Input.GetKeyUp(KeyCode.Alpha0) && !isPutHero){
			SelectItem(heroID,true);
		}
	}

    void UpdateList(){
		for(var i=0;i<itemContainer.childCount;i++)
		{
			Destroy(itemContainer.GetChild(i).gameObject);
		}	
		itemList.Clear();

		if(!isPutHero && heroID!=""){
	        var heroItem = Instantiate(itemPrefab);
	        heroItem.GetComponent<BuildBarItem>().InitHero(heroID);
	        heroItem.transform.SetParent(itemContainer,false);
	        itemList.Add(heroItem.GetComponent<BuildBarItem>());
		}

        foreach(var buildID in buildIDList){
        	var item = Instantiate(itemPrefab);
        	item.GetComponent<BuildBarItem>().Init(buildID);
        	item.transform.SetParent(itemContainer,false);
        	itemList.Add(item.GetComponent<BuildBarItem>());
        }

		itemContainer.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, buildIDList.Count * 192 + 300);
    }

    public void HideHero(){
    	isPutHero = true;
    	itemList[0].gameObject.SetActive(false);
    }

    public void SelectItem(string buildId,bool isHero){
		foreach(var item in itemList){
			if(item.buildId==buildId){
				if(item.isInCd){
					return;
				}
			}
		}

		if(selectId == buildId){
			UnselectAll();
			return;
		}


		selectId = buildId;
		selectHero = isHero;

		foreach(var item in itemList){
			if(item.buildId==selectId){
				item.Selected();
			}
			else{
				item.CancelSelected();
			}
		}

		if(buildPreview.childCount>0)Destroy(buildPreview.GetChild(0).gameObject);
		GameObject preview;
		if(isHero)
			preview = Instantiate(Resources.Load<GameObject>("prefab/hero/"+buildId));
		else
			preview = Instantiate(Resources.Load<GameObject>("prefab/build/"+buildId));
		Destroy(preview.GetComponent<BuildView>());
		preview.transform.SetParent(buildPreview,false);
		if(!isHero && Configs.GetBuild(buildId).isNaval){
			preview.transform.localPosition = new Vector3(0,-0.5f,0);
		}
		float range = 0;
		if(isHero)
			range = Configs.GetHeroWeaponRange(buildId);
		else
			range = Configs.GetBuildWeaponRange(buildId);
		if(range<0.6)range=0;
		buildPreviewRange.localScale = new Vector3(range,range,range);


		BattleManager.ins.UnselectMapObject();
	}

	public void ShowInfo(Vector3 position,string buildId,bool isHero){
		buildInfoTips.SetActive(true);
		var p = buildInfoTips.transform.position;
		p.x = position.x;
		buildInfoTips.transform.position = p;

		if(!isHero){
			var data = Configs.GetBuild(buildId);
			txtBuildInfoName.text = I18N.instance.getValue(data.uiName);
			txtBuildInfoDesc.text = I18N.instance.getValue(data.uiName+"_Desc");
		}
		else{
			var data = Configs.GetHero(buildId);
			txtBuildInfoName.text = I18N.instance.getValue(data.uiName);
			txtBuildInfoDesc.text = I18N.instance.getValue(data.uiName+"_Title");
		}
	}

	public void HideInfo(){
		buildInfoTips.SetActive(false);
	}

	public void UnselectAll(){
		selectId = "";

		foreach(var item in itemList){
			item.CancelSelected();
		}

		if(buildPreview.childCount>0)Destroy(buildPreview.GetChild(0).gameObject);

		HideInfo();
	}

	public void EnterCd(string buildId){
		foreach(var item in itemList){
			if(item.buildId==selectId){
				item.EnterCd();
				return;
			}
		}
	}

	public void RefreshCost(){
		foreach(var item in itemList){
			item.RefreshCost();
		}
	}

	public void AddBuildItem(string buildId){
		buildIDList.Add(buildId);
		UpdateList();
		return;
        var item = Instantiate(itemPrefab);
        item.GetComponent<BuildBarItem>().Init(buildId);
        item.transform.SetParent(itemContainer,false);
        itemList.Add(item.GetComponent<BuildBarItem>());

		itemContainer.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, itemContainer.childCount * 192 + 180);
	}

	public void FlashItem(string buildId){
		foreach(var item in itemList){
			if(item.buildId==buildId)item.flash.SetActive(true);
		}
	}

	public string GetHeroId(){
		if(isPutHero)return "";

		return heroID;
	}

	public void SetHeroId(string id){
		if(id == "")isPutHero = true;
		heroID = id;
	}

	public List<string> ToIdList(){
		return buildIDList;
	}

	public void FromIdList(List<string> list){
		buildIDList = list;
		UpdateList();
	}
}
