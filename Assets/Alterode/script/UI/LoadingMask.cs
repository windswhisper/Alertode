using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingMask : MonoBehaviour
{
	public static LoadingMask ins;

	public Animation anim;

	void Start(){
		ins = this;
		gameObject.SetActive(false);
	}

	public void LoadSceneAsync(string sceneName){
		Show();
		StartCoroutine(Load(sceneName));
	}

	IEnumerator Load(string sceneName){
		yield return new WaitForSeconds(0.7f);
		yield return SceneManager.LoadSceneAsync(sceneName);
		anim.Play("loading_mask_hide");
	}

	public void Show(){
		gameObject.SetActive(true);
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
