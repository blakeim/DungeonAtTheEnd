using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EncounterRandomizer : MonoBehaviour
{
    [SerializeField]private int spawnRangeX;
    [SerializeField]private int spawnRangeZ;
    [SerializeField]private float spawnLifeTime;
    [SerializeField]private float spawnInterval;
    [SerializeField]private GameObject battleTriggerPrefab;
    
    [SerializeField]private CharacterStats player;
    private System.Random rand = new System.Random();
    private float timeToNextSpawn;

    void Start(){
        
		if(player == null){
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
				if(g.GetComponent<LocomotionEngine>() != null){
					player = g.GetComponent<CharacterStats>();
				}
			}
		}
    }

    void Update(){

        transform.position = player.transform.position;
        
        if(timeToNextSpawn <= 0){
            int randomX = rand.Next(spawnRangeX);
            int randomZ = rand.Next(spawnRangeZ);
            Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
               
            if(randomX <= 1 && randomZ <= 1){
                //do nothing, I find this generally clearer than inverting the condition, and not much less efficient
            }
            else{                    
                GameObject tempObject = Instantiate(battleTriggerPrefab, spawnPosition, transform.rotation);
                tempObject.GetComponent<TimedObjectDestructor>().timeOut = spawnLifeTime;
            }

            timeToNextSpawn = spawnInterval;
        }

        timeToNextSpawn -= Time.deltaTime;
    }

}
