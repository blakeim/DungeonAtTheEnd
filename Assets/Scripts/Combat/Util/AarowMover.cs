using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AarowMover : MonoBehaviour {

	[SerializeField]private float spinSpeed;
	[SerializeField]private float motionMagnitude;
	
	// Update is called once per frame
	void Update () {

		if(Time.timeScale == 1){
			transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude);
		}				
	}
}
