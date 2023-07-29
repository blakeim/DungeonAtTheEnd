using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour{
    
    [SerializeField] private CharacterStats player;
    [SerializeField] private PlayerParty party;

    private Save CreateSaveGameObject(){
        
        Save save = new Save();

        save.player = player;
        save.party = party;

        return save;
    }
    
    public void SaveGame(){
        
        if(player == null){
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
				if(g.GetComponent<LocomotionEngine>() != null){
					player = g.GetComponent<CharacterStats>();
				}
			}
        }

        if(party == null){
            party = GameObject.FindWithTag("PlayerPartyManager").GetComponent<PlayerParty>();
        }

        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }
}
