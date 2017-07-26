using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
	public Transform position;
	public GameObject laser;
	// Use this for initialization
	//private Vector3 startPos ;
	void Start () {
		laser.GetComponent<GameObject> ();
		//startPos = position.position;
		StartCoroutine (Laser());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Laser(){
		
		while (1 == 1) {
			laser.transform.localScale= new Vector3(0.000001f,0.00001f,0);
			//laser.transform.position = new Vector2 (-1964, 577);
			yield return new WaitForSeconds (0.5f);
			//laser.transform.position = startPos;
			laser.transform.localScale= new Vector3(1,1,0);
			yield return new WaitForSeconds (0.5f);
		}
	}
}
