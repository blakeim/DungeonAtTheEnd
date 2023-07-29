using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIButton : MonoBehaviour {

	[SerializeField]private TurnManager turnManager;
	[SerializeField]private string actionCode;
	[SerializeField]private bool subMenu;
	
	private bool targettingEnemy = false;
	private bool targettingFriendly = false;

	void Start(){

		turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager>();
	}

	void Update(){
		
		if(targettingEnemy){
			if( Input.GetMouseButtonDown(0) ){
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit[] hits = Physics.RaycastAll( ray );
				foreach( RaycastHit hit in hits ){ 
					if(hit.transform.gameObject.tag.Equals("Enemy")){
						doActionToTarget(hit.transform.gameObject);
					}
				}
			}
		}
		else if(targettingFriendly){
			if( Input.GetMouseButtonDown(0) ){
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit[] hits = Physics.RaycastAll( ray );
				foreach( RaycastHit hit in hits ){ 
					if(hit.transform.gameObject.tag.Equals("Player")){
						doActionToTarget(hit.transform.gameObject);
					}
				}
			}
		}
	}

	public void SetAction(string action){
		actionCode = action;
	}

	public void doAction(){

		if(!actionCode.Equals("Run Away")){
			foreach(GameObject b in GameObject.FindGameObjectsWithTag("CombatMenuButton")){
				b.GetComponent<Button>().interactable = false;
			}
		}

		switch(actionCode){
			case "Attack":
				targettingEnemy = true;
				break;
			case "Guard":
				turnManager.doAction(actionCode, null);
				break;
			case "Run Away":
				if(!turnManager.isBossBattle){
					turnManager.doAction(actionCode, null);
				}
				break;
			case "Abilities":
				turnManager.doAction(actionCode, null);
				break;
			case "Items":
				turnManager.doAction(actionCode, null);
				break;
			default:
				targettingEnemy = turnManager.GetTargetType(actionCode) == 0 ? true : false;
				targettingFriendly = turnManager.GetTargetType(actionCode) == 1 ? true : false;
				break;
			}

	}	

	private void doActionToTarget(GameObject target){
		targettingEnemy = false;
		targettingFriendly = false;
 		turnManager.doAction(actionCode, target);
	}
}
