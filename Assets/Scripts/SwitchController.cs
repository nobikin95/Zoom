using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {
	public float ElevatedDistance;
	public GameObject LockedDoor;

	private bool LockedDoorIsOpen;

	// Use this for initialization
	void Start () {
		LockedDoorIsOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(){
		if (LockedDoorIsOpen == false) {
			LockedDoor.transform.position += new Vector3 (0, ElevatedDistance, 0);
			LockedDoorIsOpen = true;
		}

	}
}
