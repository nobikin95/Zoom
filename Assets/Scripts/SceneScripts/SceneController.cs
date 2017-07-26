using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {


	public void OnStartButtonClicked(){
		//SoundManager.instance.PlayButtonSound ();
		TKSceneManager.ChangeScene ("IntroScene");
	}

	public void OnReplayButtonClicked(){
		TKSceneManager.ChangeScene ("PlayScene");
	}
}
