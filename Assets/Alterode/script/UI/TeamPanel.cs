using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class TeamPanel : MonoBehaviour
{
	public Transform rootMyTeam;
	public RectTransform rootAllTeam;
    public ScrollRect allTeamScrollRect;
	public GameObject buildDetail;

    public List<ItemTeam> listMyTeam;
    public List<ItemTeam> listAllTeam;
    public Text txtMyteam;
    public Text txtMyCollect;

    public TeamBuildDetail teamBuildDetail;

	public GameObject itemTeamPrefab;
    public Text txtSilicon;
    
    [HideInInspector]
    public ItemTeam itemSelected;


    void OnEnable(){
        listAllTeam.Clear();
        listMyTeam.Clear();

        for(var i=0;i<rootAllTeam.childCount;i++){
            Destroy(rootAllTeam.GetChild(i).gameObject);
        }
        for(var i=0;i<rootMyTeam.childCount;i++){
            Destroy(rootMyTeam.GetChild(i).gameObject);
        }

        foreach(var build in UserData.data.buildsCollect){
            var item = Instantiate(itemTeamPrefab);
            item.GetComponent<ItemTeam>().Init(this,build,false);
            listAllTeam.Add(item.GetComponent<ItemTeam>());
            item.transform.SetParent(rootAllTeam,false);

            if(build.isUsing){
                var item2 = Instantiate(itemTeamPrefab);
                item2.GetComponent<ItemTeam>().Init(this,build,true);
                listMyTeam.Add(item2.GetComponent<ItemTeam>());
                item2.transform.SetParent(rootMyTeam,false);
            }
        }

        foreach(var data in Configs.buildConfig.datas){
            bool unlock = false;
            if(data.uncollectable)continue;
            foreach(var build in UserData.data.buildsCollect){
                if(build.buildId==data.name){
                    unlock = true;
                    break;
                }
            }
            if(unlock)continue;

            var item = Instantiate(itemTeamPrefab);
            item.GetComponent<ItemTeam>().Init(this,data);
            listAllTeam.Add(item.GetComponent<ItemTeam>());
            item.transform.SetParent(rootAllTeam,false);
        }

        var collectableCount = 0;
        foreach(var data in Configs.buildConfig.datas){
            if(!data.uncollectable)collectableCount++;
        }
        txtMyCollect.text = I18N.instance.getValue("@UI.AllTeam")+"("+UserData.data.buildsCollect.Count+"/"+collectableCount+")";

        rootAllTeam.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, (rootAllTeam.childCount/8) * 162 + 200);

        StartCoroutine(ScrollBack());

        RefreshItem();
    }

    IEnumerator ScrollBack(){
        yield return new WaitForSeconds(0.1f);
        allTeamScrollRect.verticalNormalizedPosition = 1;
    }

    void Update(){
        txtSilicon.text = UserData.data.silicon+"";
    }

    public void SelectItem(ItemTeam item){
        itemSelected = item;

        teamBuildDetail.SelectItem(item);

        RefreshItem();
    }

    public void RefreshItem(){
        foreach(var item in listMyTeam){
            item.Refresh();
        }
        foreach(var item in listAllTeam){
            item.Refresh();
        }

        txtMyteam.text = I18N.instance.getValue("@UI.MyTeam")+"("+listMyTeam.Count+"/8)";
    }

    public void RefreshList(){
        listMyTeam.Clear();
        
        for(var i=0;i<rootMyTeam.childCount;i++){
            Destroy(rootMyTeam.GetChild(i).gameObject);
        }
        foreach(var build in UserData.data.buildsCollect){
            if(build.isUsing){
                var item2 = Instantiate(itemTeamPrefab);
                item2.GetComponent<ItemTeam>().Init(this,build,true);
                listMyTeam.Add(item2.GetComponent<ItemTeam>());
                item2.transform.SetParent(rootMyTeam,false);
            }
        }

        RefreshItem();
    }

    public bool UseBuild(ItemTeam item){
        int count = 0;
        foreach(var build in UserData.data.buildsCollect){
            if(build.isUsing){
                count++;
            }
        }

        if(count>=8){
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.TeamFull"));
            return false;
        }
        item.data.isUsing = true;
        
        RefreshList();

        return true;
    }

    public void RemoveBuild(ItemTeam item){
        item.data.isUsing = false;

        RefreshList();
    }

    public void Close(){
        gameObject.SetActive(false);
        UserData.Save();
    }
}
