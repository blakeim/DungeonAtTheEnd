using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
	[SerializeField]private List<CharacterStats> partyPrefabList;
    [SerializeField]private PlayerParty playerPartyPrefab;
    [SerializeField]private EncounterRandomizer spawner;
    [SerializeField]private Transform playerSpawn;

    private GameObject playerPartyObj;
    private PlayerParty playerParty;

    private List<CharacterStats> characters = new List<CharacterStats>();
    
    void Awake(){
        
        playerPartyObj = GameObject.FindWithTag("PlayerPartyManager");
        if(playerPartyObj == null){
            
            playerParty = Instantiate(playerPartyPrefab, playerSpawn.position, playerSpawn.rotation);

            foreach(CharacterStats c in partyPrefabList){
                characters.Add(Instantiate(c, playerSpawn.position, playerSpawn.rotation));
            }

            playerParty.SetPartyList(characters);
            playerParty.DistributeParty();

        }else{
            GameObject.FindWithTag("Player").transform.position = new Vector3(PlayerPrefs.GetFloat("SpawnX"),PlayerPrefs.GetFloat("SpawnY"),PlayerPrefs.GetFloat("SpawnZ"));
            GameObject.FindWithTag("Player").transform.rotation = new Quaternion(transform.rotation.x,PlayerPrefs.GetFloat("SpawnRotationY"),transform.rotation.z, transform.rotation.w);
        }
        
        spawner.gameObject.SetActive(true);
        PlayerPrefs.SetInt("InputEnabled", 1);
    }
}
