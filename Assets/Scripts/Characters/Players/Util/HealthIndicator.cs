using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthIndicator : MonoBehaviour{
    [SerializeField]private CharacterStats character;

    private TextMesh healthText;
	
    void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}

    void Start(){
        healthText = GetComponent<TextMesh>();
    }

    void Update(){

        int health = character.GetHealth();

        if(health > 0){
            healthText.text = health.ToString();
        }
        else{
            healthText.text = "";
        }
    }
}
