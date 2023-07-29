using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TriggerLevelLoad : MonoBehaviour {

	public string nameOfLevelToLoad  = "";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.tag == "Player" )
		{
			PlayerPrefs.SetFloat("SpawnX", transform.position.x);
			PlayerPrefs.SetFloat("SpawnY", transform.position.y);
			PlayerPrefs.SetFloat("SpawnZ", transform.position.z);
			PlayerPrefs.SetFloat("SpawnRotationX", Quaternion.Euler(transform.position).x);
			PlayerPrefs.SetFloat("SpawnRotationY", Quaternion.Euler(transform.position).y);
			PlayerPrefs.SetFloat("SpawnRotationZ", Quaternion.Euler(transform.position).z);
			PlayerPrefs.SetFloat("SpawnRotationW", Quaternion.Euler(transform.position).w);

			SceneManager.LoadScene(nameOfLevelToLoad);
			Destroy(gameObject);
		}
	}
}
