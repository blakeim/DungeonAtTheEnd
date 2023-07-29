using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour {

	[SerializeField]private List<CharacterStats> playerPartyList;
	[SerializeField]private List<Item> itemList;
	private AbilityList inventory = new AbilityList();
	private string[] itemNames;

	private bool battleMode;

	void Awake(){
		DontDestroyOnLoad(this.gameObject);
		Item tempItem;

		foreach(Item i in itemList){
			inventory.Add(i);
		}
		
		itemNames = inventory.GetNames();
	}

	public void AddCharacter(CharacterStats newChar){

		newChar.SetPartyManager(this);
		playerPartyList.Add(newChar);
	}

	public List<CharacterStats> getPartyList(){
		return playerPartyList;
	}

	public AbilityList GetInventory(){
		return inventory;
	}

	public void SetPartyList(List<CharacterStats> partyList){

		playerPartyList = new List<CharacterStats>();
		foreach(CharacterStats c in partyList){
			c.SetPartyManager(this);
			playerPartyList.Add(c);
		}
	}


	public void EnterBattle(){
		
		battleMode = true;
		PlayerPrefs.SetInt("InputEnabled", 0);
		DistributeParty();
	}

	public void ExitBattle(){
		
		battleMode = false;
		PlayerPrefs.SetInt("InputEnabled", 1);
		DistributeParty();
	}

	public bool Contains(CharacterStats character){
		
		foreach (CharacterStats c in playerPartyList){
			if(c.redableName.Equals(character.redableName)){
				return true;
			}
		}

		return false;
	}


	public void DistributeParty(){
		
		for(int i = 0; i < playerPartyList.Count; i++){
			switch(i){
				case 0:
					if(!battleMode){
						playerPartyList[i].gameObject.GetComponent<LocomotionEngine>().SetCanMove(true);
						playerPartyList[i].gameObject.GetComponent<CharacterStats>().EnablePhysics();
					}
					else{
						playerPartyList[i].gameObject.GetComponent<LocomotionEngine>().SetCanMove(false);
						playerPartyList[i].gameObject.GetComponent<CharacterStats>().DisablePhysics();
					}
					playerPartyList[i].gameObject.GetComponent<CharacterStats>().ResetAnimation();
					playerPartyList[i].gameObject.GetComponent<CharacterStats>().ResetBattleVars();
					break;
				default:
					if(!battleMode){
						playerPartyList[i].gameObject.GetComponent<LocomotionEngine>().SetCanMove(false);
						playerPartyList[i].gameObject.SetActive(false);
					}
					else{
						playerPartyList[i].gameObject.GetComponent<LocomotionEngine>().SetCanMove(false);
						playerPartyList[i].gameObject.SetActive(true);
						playerPartyList[i].gameObject.GetComponent<CharacterStats>().DisablePhysics();
					}
					playerPartyList[i].gameObject.GetComponent<CharacterStats>().ResetBattleVars();
					break;
			}
		}
	}
}
