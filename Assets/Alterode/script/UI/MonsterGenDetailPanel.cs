using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenDetailPanel : MonoBehaviour
{
	MonsterGenerator monsterGenerator;
	public GameObject itemPrefab;
	public Transform container;

	public void Show(MonsterGenerator monsterGenerator){
		gameObject.SetActive(true);
		this.monsterGenerator = monsterGenerator;
        BattleManager.ins.selectorRange.localScale = new Vector3(0,0,0);
		Refresh();
	}

	void Update(){

	}

	void Refresh(){
		var monterNames = new Dictionary<string,int>();

		foreach(var monster in monsterGenerator.data.monsters){
			if(monster=="")continue;
			if(!monterNames.ContainsKey(monster)){
				monterNames.Add(monster,1);
			}
			else{
				monterNames[monster]++;
			}
		}

		for(var i=0;i<container.childCount;i++){
			Destroy(container.GetChild(i).gameObject);
		}

		foreach(var monster in monterNames){
			var item = Instantiate(itemPrefab);
			item.GetComponent<ItemMonsterGen>().Init(monster.Key,monster.Value*monsterGenerator.data.repeat);
			item.transform.SetParent(container,false);
		}
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
