using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnergyIndicator : MonoBehaviour{    
    
    [SerializeField]private CharacterStats character;

    private TextMesh healthText;
	
    void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}

    void Start(){
        healthText = GetComponent<TextMesh>();
    }

    void Update(){

        int health = character.GetEnergy();

        if(health > 0){
            healthText.text = "\n" + health.ToString();
        }
        else{
            healthText.text = "";
        }
    }
}
