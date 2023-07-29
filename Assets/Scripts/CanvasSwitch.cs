using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitch : MonoBehaviour {

[SerializeField]private Canvas canvasToShow;

private Canvas parentCanvas;

	// Use this for initialization
	void Start () {
		
		parentCanvas = gameObject.GetComponentInParent<Canvas>();
	}

	public void SwitchCanvas(){

		parentCanvas.enabled = false;
		canvasToShow.enabled = true;
		canvasToShow.gameObject.SetActive(true);

		print(canvasToShow.enabled);
	}

}
