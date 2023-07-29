using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAdder : MonoBehaviour
{

    [SerializeField]private CharacterStats characterToAdd;
    private PlayerParty party;

    void Start(){

        if(party == null){
            party = GameObject.FindWithTag("PlayerPartyManager").GetComponent<PlayerParty>();
        }    

    }
}
