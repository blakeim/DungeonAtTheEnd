using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour {

	[SerializeField]private Transform target;

	private float x = 0.0f;
    private float y = 0.0f;

	public float distance = 5;
	public float xSpeed = 100;
	public float ySpeed = 100;
	public float minDistance = 3;
	public float maxDistance = 8;
	public float height = 0.25f;

	// Use this for initialization
	void Start () {
		
		if(target == null){
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
				if(g.GetComponent<LocomotionEngine>() != null){
					target = g.transform;
				}
			}
		}
	}

	private void LateUpdate()
    {
		if(Time.timeScale == 1 && target){
            
			Quaternion rotation;
			
			if(Input.GetMouseButton(1)){
				x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				rotation = Quaternion.Euler(y, x, 0);
			}
			else{
				rotation = target.rotation;
			}
			
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, minDistance, maxDistance);
				
			Debug.DrawRay(transform.position, Vector3.down, Color.green, 500);            

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + new Vector3(target.position.x, target.position.y + height, target.position.z);

            transform.rotation = rotation;
			transform.position = position;

			transform.LookAt(target);
		}
    }
}
