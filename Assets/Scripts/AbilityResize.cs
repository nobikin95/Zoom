using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(CustomCharacterController))]
[RequireComponent (typeof(MegamanController))]
[RequireComponent (typeof(InputManager))]
[RequireComponent (typeof(GameObject))]
public class AbilityResize : MonoBehaviour
{
	public float value = 1f;
	[SerializeField] AudioClip soundOnZoom;
	private CustomCharacterController characterController;
	private MegamanController megamanController;
	private InputManager inputManager;
	public GameObject map;
	public GameObject player;
	//private RectTransform rectTransform;
	private Vector3 endScale;


	private void Awake ()
	{
		map.transform.localScale = new Vector3 (1, 1, 1);
		megamanController = GetComponent<MegamanController> ();
		inputManager = GetComponent<InputManager> ();
		//rectTransform = GetComponentInChildren<RectTransform>(map);
		characterController = GetComponent<CustomCharacterController> ();

		//map = GetComponent<GameObject>();
		inputManager.OnResizeButtonPressed += OnResizeButtonPressed;
	}

	private IEnumerator Downsize ()
	{
		SoundManager.instance.Play (soundOnZoom, 1);
		float resizeCurrent = 1;
		while (resizeCurrent < megamanController.resizeScale) {
			Vector3 mapPosition = map.transform.position;
			Vector3 pivotPosition = player.transform.position;
			Vector3 difference = mapPosition - pivotPosition; // diff from object pivot to desired pivot/origin
			Vector3 startScale = map.transform.localScale;

			endScale = map.transform.localScale * megamanController.resizeRate;
			Vector3 finalPosition = (difference * megamanController.resizeRate) + pivotPosition;       // calc final position post-scale
			map.transform.localScale = endScale;
			map.transform.position = finalPosition;
			resizeCurrent *= megamanController.resizeRate;

			yield return new WaitForSeconds (0.01f);
		}
        
		yield return new WaitForSeconds (megamanController.ResizeCooldownTime);
		megamanController.playerStatus.resizeAvailable = true;
	}

	private IEnumerator Upsize ()
	{
		SoundManager.instance.Play (soundOnZoom, 1);
		float resizeCurrent = 1;
		while (resizeCurrent < megamanController.resizeScale) {
			characterController.RaycastTouchHorizontal (ref megamanController.playerStatus.currentVelocity);
			characterController.RaycastTouchVertical (ref megamanController.playerStatus.currentVelocity);
			Vector3 mapPosition = map.transform.position;
			Vector3 pivotPosition = player.transform.position;
			Vector3 difference = mapPosition - pivotPosition; // diff from object pivot to desired pivot/origin
			Vector3 startScale = map.transform.localScale;

			endScale = map.transform.localScale / megamanController.resizeRate;
			Vector3 finalPosition = (difference / megamanController.resizeRate) + pivotPosition;       // calc final position post-scale
			map.transform.localScale = endScale;
			map.transform.position = finalPosition;

			resizeCurrent *= megamanController.resizeRate;

			if (characterController.collisionInfo.touchBottom && !characterController.collisionInfo.touchTop) {
				player.transform.position += new Vector3 (0f, 10f, 0f);
				Debug.Log ("Collide Bottom!");
			}
			if (characterController.collisionInfo.touchTop && !characterController.collisionInfo.touchBottom) {
				player.transform.position += new Vector3 (0f, -10f, 0f);
				Debug.Log ("Collide Top!");
			}
			if (characterController.collisionInfo.touchLeft && !characterController.collisionInfo.touchRight) {
				player.transform.position += new Vector3 (10f, 0f, 0f);
				Debug.Log ("Collide Left!");
			}
			if (characterController.collisionInfo.touchRight && !characterController.collisionInfo.touchLeft) {
				player.transform.position += new Vector3 (-10f, 0f, 0f);
				Debug.Log ("Collide Right!");
			}

			yield return new WaitForSeconds (0.01f);
		}

		yield return new WaitForSeconds (0.5f);
		if (characterController.collisionInfo.touchLeft && characterController.collisionInfo.touchRight &&
		          characterController.collisionInfo.touchBottom && characterController.collisionInfo.touchTop) {
			megamanController.Lose();
		}
		/*
        while (characterController.collisionInfo.touchLeft && characterController.collisionInfo.touchRight &&
                characterController.collisionInfo.collideBottom)
        {
            player.transform.position += new Vector3(0f, 20f, 0f);
        }

        while (characterController.collisionInfo.touchLeft && characterController.collisionInfo.touchRight &&
                characterController.collisionInfo.touchTop)
        {
            player.transform.position += new Vector3(0f, -20f, 0f);
        }

        while (characterController.collisionInfo.touchLeft && characterController.collisionInfo.touchBottom &&
               characterController.collisionInfo.touchTop)
        {
            player.transform.position += new Vector3(20f, 0f, 0f);
        }

        while (characterController.collisionInfo.touchRight && characterController.collisionInfo.touchBottom &&
               characterController.collisionInfo.touchTop)
        {
            player.transform.position += new Vector3(-20f, 0f, 0f);
        }
        */
		if (characterController.collisionInfo.touchLeft && characterController.collisionInfo.touchRight &&
		          characterController.collisionInfo.touchBottom && characterController.collisionInfo.touchTop) {
			Debug.Log ("Lose");
		}

		yield return new WaitForSeconds (megamanController.ResizeCooldownTime);
		megamanController.playerStatus.resizeAvailable = true;
	}

	private void OnResizeButtonPressed ()
	{
		if (megamanController.playerStatus.isUpsized == true && megamanController.playerStatus.resizeAvailable) {
			megamanController.playerStatus.resizeAvailable = false;
			StartCoroutine (Downsize ());
			megamanController.playerStatus.isUpsized = false;
            
			Debug.Log ("Downsize!!!");

		} else if (megamanController.playerStatus.isUpsized == false && megamanController.playerStatus.resizeAvailable) {
			megamanController.playerStatus.resizeAvailable = false;
			StartCoroutine (Upsize ());
			megamanController.playerStatus.isUpsized = true;
			Debug.Log ("Upsize!!!");
            
		}
	}

 
}
