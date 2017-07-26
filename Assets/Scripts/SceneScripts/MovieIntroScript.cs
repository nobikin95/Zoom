using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieIntroScript : MonoBehaviour {
	public GameObject panel;
	public float timeToChangeScene;
	public string nextScene;
	void Start () {		
		StartCoroutine (FadePanel());
	}

	IEnumerator FadePanel(){
		CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup> ();
		while (canvasGroup.alpha>0) {
			canvasGroup.alpha -= Time.deltaTime / 2;
			yield return null;
		}
		canvasGroup.interactable = false;
		yield return new WaitForSeconds(timeToChangeScene);
		TKSceneManager.ChangeScene (nextScene);
	}
		
}
