using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateLevelSelect : MonoBehaviour {

	[SerializeField] Button loadButtonPrefab;

	private int levels;

	void Start () {
		
		
			Button tempButton = Instantiate(loadButtonPrefab,new Vector3(0, 0, 0), Quaternion.identity);
			tempButton.transform.SetParent(gameObject.transform.parent);


			tempButton.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			tempButton.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().pivot = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().anchorMax = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120,120);		
	}
}
