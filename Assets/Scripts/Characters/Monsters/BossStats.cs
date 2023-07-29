using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonsterStats{
    
    [SerializeField]protected int minionCount;
	[SerializeField]protected List<CharacterStats> minionList;
	[SerializeField]protected List<Transform> minionSpawns;
    [SerializeField]protected GameObject summonParticles;

    void Start(){

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("MinionSpawnPoints")){
            minionSpawns.Add(g.transform);
        }
    }

    protected bool SpawnMinion(){

		int monsterSelector = rand.Next(minionList.Count);
        int spawnSelector = rand.Next(minionSpawns.Count);

		if(monsterSelector >= 0){
            Instantiate(summonParticles, minionSpawns[spawnSelector].position, minionSpawns[spawnSelector].rotation);
			Instantiate(minionList[monsterSelector], minionSpawns[spawnSelector].position, minionSpawns[spawnSelector].rotation);
            return true;
		}	

        return false;
    }

    protected IEnumerator SpawnMinionRoutine(){

        Camera mainCamera = Camera.main;
        castingCamera.enabled = true;
		mainCamera.enabled = false;
        yield return new WaitForSeconds(0.25f);
        TriggerAnimate("Attack", 0f, true);
        yield return new WaitForSeconds(1.15f);
        mainCamera.enabled = true;
        castingCamera.enabled = false;
        SpawnMinion();
        hasActed = true;
        yield return new WaitForSeconds(1f);
        EndTurn();
    }

    public override void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){
        
		print("Animating with " + triggerName);
		animator.SetTrigger(triggerName);
    }

}
