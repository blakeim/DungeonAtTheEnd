using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStats : CharacterStats{

    public override void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){

		if(castingCamera){
			StartCoroutine(CastRoutine(attackTimer));
		}

		animator.SetTrigger(triggerName);
	}

	public IEnumerator CastRoutine(float shotTime){
		
		Camera mainCamera = Camera.main;
		castingCamera.enabled = true;		
		mainCamera.enabled = false;
		yield return new WaitForSeconds(shotTime);
		mainCamera.enabled = true;
		castingCamera.enabled = false;		
	}

}
