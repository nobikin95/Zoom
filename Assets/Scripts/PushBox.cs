using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour {
	public List<GameObject> box;
	public  float pushSpeed;
	public float distanceToPush;
	[SerializeField] AudioClip soundOnPush;
	private BoxController boxController;
	private CustomCharacterController characterController;
	private MegamanController megamanController;

	private void Awake()
	{
		characterController = GetComponent<CustomCharacterController> ();
		megamanController = GetComponent<MegamanController> ();
	}
	private void FixedUpdate()
	{
		if (characterController.collisionInfo.collideRight && characterController.collisionInfo.isBox == true && megamanController.playerStatus.isUpsized )
		{	
			for (int i = 0; i < box.Count; i++) {
				if (Vector2.Distance (box [i].transform.position, transform.position) < distanceToPush) {
					SoundManager.instance.PlaySoundEffect (soundOnPush);
					boxController = box [i].GetComponent<BoxController> ();
					boxController.currentVelocity.x += pushSpeed;
					megamanController.anim.SetBool ("isPushing", true);
				}
			}
		} else if (characterController.collisionInfo.collideLeft && characterController.collisionInfo.isBox == true && megamanController.playerStatus.isUpsized)
		{	
			for (int i = 0; i < box.Count; i++) {
				if (Vector2.Distance (box [i].transform.position, transform.position) < distanceToPush) {
					SoundManager.instance.PlaySoundEffect (soundOnPush);
					boxController = box [i].GetComponent<BoxController> ();
					boxController.currentVelocity.x -= pushSpeed;	
					megamanController.anim.SetBool ("isPushing", true);
				}
			}
		}
		else megamanController.anim.SetBool ("isPushing",false);

//		if(characterController.collisionInfo.collideRight && characterController.collisionInfo.isBox == true && megamanController.playerStatus.isSliding ){
//			for (int i = 0; i < box.Count; i++) {
//				if(Vector2.Distance(box[i].transform.position,transform.position) < distanceToPush)
//				boxController = box[i].GetComponent<BoxController>();
//				megamanController.playerStatus.currentVelocity = boxController.currentVelocity; 
//			}
//		} else if(characterController.collisionInfo.collideLeft && characterController.collisionInfo.isBox == true && megamanController.playerStatus.isSliding){
//			for (int i = 0; i < box.Count; i++) {
//				if(Vector2.Distance(box[i].transform.position,transform.position) < distanceToPush)
//					boxController = box[i].GetComponent<BoxController>();
//				megamanController.playerStatus.currentVelocity = boxController.currentVelocity; 
//			}
//		}
			
	}

}
