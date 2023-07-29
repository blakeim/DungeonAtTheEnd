using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	
	public bool isBossBattle;

	[SerializeField]protected List<Transform> playerSpawnPoints;
	[SerializeField]protected List<Transform> monsterSpawnPoints;
	[SerializeField]protected List<CharacterStats> monsterPrefabs;
	[SerializeField]protected GameObject turnIndicator;
	[SerializeField]protected string victoryScene;
	[SerializeField]protected string defeatScene;
	protected PlayerParty playerParty;
	protected System.Random rand = new System.Random();

	protected List<CharacterStats> playerPartyList;
	protected List<CharacterStats> monsterParty;
	protected List<CharacterStats> turnQueue;
	protected int turn;
	protected int numberOfMonsters;
	protected int partySize;
	protected bool monsterTurns = false;
	protected bool waiting = false;

	// Use this for initialization
	void Start () {

		SpawnMonsters();
		GameObject[] monsterPartyPivot = GameObject.FindGameObjectsWithTag("Enemy");
		monsterParty = new List<CharacterStats>(monsterPartyPivot.Length);
		for(int i = 0; i < monsterPartyPivot.Length; i++){
			monsterParty.Add(monsterPartyPivot[i].GetComponent<CharacterStats>());
		}
		
		playerParty = GameObject.FindWithTag("PlayerPartyManager").GetComponent<PlayerParty>();
		playerParty.EnterBattle();
		playerPartyList = playerParty.getPartyList();
		partySize = playerPartyList.Count;

		for(int i = 0; i < partySize; i++){
			playerPartyList[i].transform.position = playerSpawnPoints[i].transform.position;
			playerPartyList[i].transform.rotation = playerSpawnPoints[i].transform.rotation;
			playerPartyList[i].SetStartingPoint(playerSpawnPoints[i].transform.position);
		}
		
		turnQueue = new List<CharacterStats>(partySize + numberOfMonsters);
		PopulateTurnQueue();

		StartCoroutine(NextTurn(0));
	}

	protected virtual IEnumerator NextTurn(int turnNumber){
		
		CharacterStats currentCharacter = turnQueue[turnNumber];
		if(currentCharacter.GetAlive()){
			turnIndicator.transform.position = new Vector3(currentCharacter.transform.position.x, currentCharacter.transform.position.y + 2, currentCharacter.transform.position.z);
		}

		yield return new WaitForSeconds(0.5f);
		CheckIfDefeated(monsterParty, "VictoryScene", true);
		CheckIfDefeated(playerPartyList, "GameOverScene", false);
		currentCharacter.StartTurn(EndTurn, turnNumber);
	}
 
	public virtual void EndTurn(int turnNumber){
		turn = (turnNumber + 1) % (partySize + numberOfMonsters);
		StartCoroutine(NextTurn(turn));
	}

	protected virtual void SpawnMonsters(){
		
		int monsterSelector = -1;
		int monsterCount = monsterPrefabs.Count + 1;
		int monsterSpawns = monsterSpawnPoints.Count;

		for(int i = 0; i < monsterSpawns; i++){
			
			monsterSelector = rand.Next(monsterCount) - 1;

			if(monsterSelector >= 0){
				numberOfMonsters += 1;
				Instantiate(monsterPrefabs[monsterSelector], monsterSpawnPoints[i].position, monsterSpawnPoints[i].rotation);
			}	
		}
	}

	protected virtual void PopulateTurnQueue(){
		
		for(int i = 0; i < partySize; i++){
			playerPartyList[i].SetHasActed(false);
			turnQueue.Add(playerPartyList[i]);
		}
		for(int i = 0; i < monsterParty.Count; i++){
			monsterParty[i].SetHasActed(false);
			turnQueue.Add(monsterParty[i]);
		}

	}

	public virtual int GetTargetType(string abilityName){
		return playerPartyList[turn].GetTargetType(abilityName);
	}

	public virtual void doAction(string actionCode, GameObject target){
		
		switch(actionCode){
			case "Attack":
			print(target);
			print(playerPartyList);
			print(playerPartyList[turn]);
				playerPartyList[turn].Attack(target);
				break;
			case "Guard":
				playerPartyList[turn].Guard();
				playerPartyList[turn].EndTurn();
				break;
			case "Run Away":
				CheckIfDefeated(new List<CharacterStats>(), "SampleScene", true);
				break;
			case "Abilities":
				playerPartyList[turn].SpawnAbilityMenu();
				break;			
			case "Items":
				playerPartyList[turn].SpawnItemMenu();
				break;
			default:
				playerPartyList[turn].CastAbility(actionCode, target, false);
				break;
			}
	}

	protected virtual void CheckIfDefeated(List<CharacterStats> party, string loadOnDefeated, bool playersWin){

		bool defeated = true;

		foreach(CharacterStats c in party){
			if(c.GetAlive()){
				defeated = false;
			}
		}

		if(defeated){
			foreach(CharacterStats c in monsterParty){
				Destroy(c.gameObject);
			}
			//Destroy(playerParty.gameObject);
			if(playersWin){
				playerParty.ExitBattle();
			}
			SceneManager.LoadScene(loadOnDefeated);
		}
	}
}
