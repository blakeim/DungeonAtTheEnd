using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BossBattleManager : TurnManager{

    [SerializeField]private Transform bossSpawnPoint;
	[SerializeField]private CharacterStats bossPrefab;
	[SerializeField]private List<Transform> minionSpawnPoints;
	[SerializeField]private List<CharacterStats> minionPrefabs;
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

	protected override IEnumerator NextTurn(int turnNumber){
		
        PopulateTurnQueue();

		CharacterStats currentCharacter = turnQueue[turnNumber];
		if(currentCharacter.GetAlive()){
			turnIndicator.transform.position = new Vector3(currentCharacter.transform.position.x, currentCharacter.transform.position.y + 2, currentCharacter.transform.position.z);
		}

		yield return new WaitForSeconds(0.5f);
		CheckIfDefeated(monsterParty, victoryScene, true);
		CheckIfDefeated(playerPartyList, defeatScene, false);
		currentCharacter.StartTurn(EndTurn, turnNumber);
	}
 
	protected override void SpawnMonsters(){
		
        Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        numberOfMonsters += 1;
		int monsterSelector = -1;
		int monsterCount = minionPrefabs.Count + 1;
		int monsterSpawns = minionSpawnPoints.Count;

		for(int i = 0; i < monsterSpawns; i++){
			
			monsterSelector = rand.Next(monsterCount -1);

			if(monsterSelector >= 0){
				numberOfMonsters += 1;
				Instantiate(minionPrefabs[monsterSelector], minionSpawnPoints[i].position, minionSpawnPoints[i].rotation);
			}	
		}
	}

	protected override void PopulateTurnQueue(){
		
		for(int i = 0; i < partySize; i++){
			playerPartyList[i].SetHasActed(false);
			turnQueue.Add(playerPartyList[i]);
		}
		for(int i = 0; i < monsterParty.Count; i++){
            print(monsterParty[i].redableName);
			monsterParty[i].SetHasActed(false);
			turnQueue.Add(monsterParty[i]);
		}

	}

	public override void doAction(string actionCode, GameObject target){
		
		switch(actionCode){
			case "Attack":
				playerPartyList[turn].Attack(target);
				break;
			case "Guard":
				playerPartyList[turn].Guard();
				playerPartyList[turn].EndTurn();
				break;
			case "Run Away":
				print("You can not escape this fight");
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

	protected override void CheckIfDefeated(List<CharacterStats> party, string loadOnDefeated, bool playersWin){

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
			if(playersWin){
				playerParty.ExitBattle();
			}
			SceneManager.LoadScene(loadOnDefeated);
		}
	}
}
