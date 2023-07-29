using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BossEncounter : MonoBehaviour{

	[SerializeField]private string[] tutorialText;
	[SerializeField]private Button buttonPrefab;
	[SerializeField]private string bossLevel;
	[SerializeField]private Canvas mainCanvas;
	[SerializeField]private Camera dialogueCamera;

	private Button[] popUpBottons;
	private Camera mainCamera;
	private int tutorialSteps;
	private int currentButon;
	private bool showingTutorial;

	void Start () {
		
		dialogueCamera.enabled = false;
		
		mainCamera = Camera.main;
		tutorialSteps = tutorialText.Length;
		popUpBottons = new Button[tutorialSteps];

		for(int i = 0; i < tutorialSteps; i++){
			Button tempButton = Instantiate(buttonPrefab);
			tempButton.GetComponentInChildren<Text>().text = tutorialText[i];
			tempButton.transform.SetParent(mainCanvas.transform);

			tempButton.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			tempButton.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().pivot = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().anchorMax = new Vector2(0,0);
			tempButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width - ((Screen.width / 10) * 2), Screen.height / 4);
			tempButton.GetComponent<RectTransform>().position = new Vector2(0 + (Screen.width / 10), 0 + (Screen.height / 8));
       		tempButton.gameObject.SetActive(false);

			popUpBottons[i] = tempButton;
		}
	}
	
	// Update is called once per frame
	void Update () {
				
		if(showingTutorial){
			if(!popUpBottons[currentButon].gameObject.activeSelf){
				currentButon++;
				
				if(currentButon >= tutorialSteps){
					PlayerPrefs.SetInt("InputEnabled", 1);
					showingTutorial = false;
					mainCamera.enabled = true;
					dialogueCamera.enabled = false;
                    SceneManager.LoadScene(bossLevel);
				}
				else{
					popUpBottons[currentButon].gameObject.SetActive(true);
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider){

		if(collider.gameObject.tag.Equals("Player")){
			dialogueCamera.enabled = true;
			mainCamera.enabled = false;
			PlayerPrefs.SetInt("InputEnabled", 0);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			showingTutorial = true;
			if(currentButon == 0){
				popUpBottons[currentButon].gameObject.SetActive(true);
			}
		}
	}
}
