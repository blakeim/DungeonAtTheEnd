using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroller : MonoBehaviour {

	[SerializeField]private float startingY;
	[SerializeField]private float resetY;
	[SerializeField]private float scrollSpeed;

	private Vector3 startingPos;
	// Use this for initialization
	void Start () {

		startingPos = new Vector3(transform.position.x, startingY, transform.position.z);
		transform.position = startingPos;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(transform.position.y >= resetY){
			transform.position = startingPos;
		}

		transform.Translate(0f,scrollSpeed,0f);
	}
}
