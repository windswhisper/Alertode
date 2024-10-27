using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class ChangeTeamChipPanel : MonoBehaviour
{
	public TeamBuildDetail teamBuildDetail;

	public int index;
	public ChangeTeamChipOption chipOption1;
	public ChangeTeamChipOption chipOption2;
    public Animation anim;
    public Text txtCost;

    public void Show(int index)
    {
    	gameObject.SetActive(true);

        this.index = index;

        if(teamBuildDetail.itemSelected.data.chipSlotCount == index){
             Lock();
        }
        else{
            anim.Play("changechip_unlockim");
        }

		var buildData = Configs.GetBuild(teamBuildDetail.itemSelected.data.buildId);

		var usingIndex = 0;
    	switch(index){
    		case 0:
	    		usingIndex = teamBuildDetail.itemSelected.data.chipSlot0;
	    		break;
    		case 1:
	    		usingIndex = teamBuildDetail.itemSelected.data.chipSlot1;
	    		break;
    		case 2:
	    		usingIndex = teamBuildDetail.itemSelected.data.chipSlot2;
	    		break;
	    }

        chipOption1.SetChip(buildData.defaultChips[index*2],usingIndex==1);
        chipOption2.SetChip(buildData.defaultChips[index*2+1],usingIndex==2);
    }

    public void Lock(){
        anim.Play("changechip_lock");
        txtCost.text = "(      x"+Configs.commonConfig.chipSlotPrice[index]+")";
    }

    public void ChooseChip1(){
    	switch(index){
    		case 0:
    			teamBuildDetail.itemSelected.data.chipSlot0 = 1;
    			break;
    		case 1:
    			teamBuildDetail.itemSelected.data.chipSlot1 = 1;
    			break;
    		case 2:
    			teamBuildDetail.itemSelected.data.chipSlot2 = 1;
    			break;
    	}

    	Hide();
    }

    public void ChooseChip2(){
    	switch(index){
    		case 0:
    			teamBuildDetail.itemSelected.data.chipSlot0 = 2;
    			break;
    		case 1:
    			teamBuildDetail.itemSelected.data.chipSlot1 = 2;
    			break;
    		case 2:
    			teamBuildDetail.itemSelected.data.chipSlot2 = 2;
    			break;
    	}

    	Hide();
    }

    public void BuyChipSlot(){
        if(UserData.data.silicon < Configs.commonConfig.chipSlotPrice[index]){
            ToastBar.ShowMsg(I18N.instance.getValue("@UI.NoSilicon"));
            return;
        }

        teamBuildDetail.itemSelected.data.chipSlotCount++;
        teamBuildDetail.teamPanel.RefreshItem();
        UserData.data.silicon -= Configs.commonConfig.chipSlotPrice[index];
        AchievementManager.OnSetupChip();
        UserData.Save();
        anim.Play("changechip_unlock");
    }

    public void Hide(){
    	gameObject.SetActive(false);
    	teamBuildDetail.RefreshChip();
    }

}
