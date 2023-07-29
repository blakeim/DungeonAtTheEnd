using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStats : CharacterStats{
	
    public override void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){

		if(castingCamera){
			StartCoroutine(CastRoutine());
		}

		animator.SetTrigger(triggerName);
	}

	public IEnumerator CastRoutine(){
		
		Camera mainCamera = Camera.main;
		castingCamera.enabled = true;		
		mainCamera.enabled = false;
		yield return new WaitForSeconds(3f);
		mainCamera.enabled = true;
		castingCamera.enabled = false;		
	}

	public override void EndCastTurn(float energyCost){
		
		CastRoutine();
		energy -= (int)energyCost;
		endTurnCallback(turnNumber);
	}


}
