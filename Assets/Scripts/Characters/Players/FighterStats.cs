using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStats : CharacterStats {

	[SerializeField]private Transform targetPos;

	private float attackAnimationTime = 1.25f;
	private float currentAttackTimer = 0f;
	private float currentReturnTimer = 0f;

	void Update(){

		if(health <= 0){
			Die();
		}		

		if(currentAttackTimer > 0){
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x + 2, transform.position.y, target.transform.position.z), 0.1f);
			currentAttackTimer -= Time.deltaTime;
			if(currentAttackTimer <= 0){
				currentReturnTimer = attackAnimationTime;
			}
		}

		if(currentReturnTimer > 0){			
			transform.position = Vector3.MoveTowards(transform.position, startingPosition, 0.1f);
			currentReturnTimer -= Time.deltaTime;
		}

	}

	protected override IEnumerator AttackRoutine(){

		TriggerAnimate("Attack_00", (attackAnimationTime*2) + 0.5f, false);
		yield return new WaitForSeconds(attackAnimationTime - 0.5f);
		yield return new WaitForSeconds(1f);
		target.TakeHit(new DamageDescriptor(damage, crit, critModifer, DamageType.Physical), this);
		RecoverEnergy();
		yield return new WaitForSeconds(0.5f);		
		EndTurn();
	}

	public override void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){

		currentAttackTimer = attackTimer - attackAnimationTime;
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
}
