using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListener : MonoBehaviour{

    [SerializeField]private Canvas mainPauseCanvas;
    void Start(){
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void Update(){
        
        if(Input.GetKeyDown("escape")){
            print("Pausing");
            PauseUnPause();
        }
    }

    public void PauseUnPause(){

        if(Time.timeScale == 1){

            Time.timeScale = 0;
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            mainPauseCanvas.gameObject.SetActive(true);
        }
        else{
            Time.timeScale = 1;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            mainPauseCanvas.gameObject.SetActive(false);
        }
    }
}
