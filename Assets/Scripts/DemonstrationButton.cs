using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationButton : MonoBehaviour
{
    [SerializeField]private List<CharacterStats> characters;
    [SerializeField]private string action;

	public void doAction(){
		
        foreach(CharacterStats c in characters){
            c.ToggleCelebration();
        }
	}	
}
