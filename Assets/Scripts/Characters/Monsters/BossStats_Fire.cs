using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats_Fire : BossStats{
    

    public override void StartTurn(System.Action<int> endTurn, int turn){

        endTurnCallback = endTurn;
        turnNumber = turn;
        int abilityIndex = 0;
        bool hasActed = false;

        if(alive){
            hasActed = false;
            int livingMonsters = 0;
            GameObject[] playerParty = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] monsterParty = GameObject.FindGameObjectsWithTag("Enemy");
            
            foreach(GameObject m in GameObject.FindGameObjectsWithTag("Enemy")){
                if(m.GetComponent<CharacterStats>().GetAlive()){
                    livingMonsters++;
                }
            }
            
            if(livingMonsters == 1){
                StartCoroutine(SpawnMinionRoutine());
            }
            else if(!hasActed){   
                if(!hasActed && DebuffingRoutine(monsterParty)){
                    print("Done da buff sir");
                    hasActed = true;
                }
                else if(!hasActed && OffensiveSkillRoutine(monsterParty)){
                    print("Done da attack abilty sir");
                    hasActed = true;
                }
                else if(!hasActed){
                    target = playerParty[rand.Next(playerParty.Length)].GetComponent<CharacterStats>();
                    StartCoroutine(AttackRoutine());
                }
            }
        }
        else{
            EndTurn(); 
        }

    }
}
