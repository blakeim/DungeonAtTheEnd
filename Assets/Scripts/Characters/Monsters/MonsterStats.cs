using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MonsterStats : CharacterStats{

    [SerializeField]protected bool aggresive;//consider breaking this out into an enum of "monster temperment"
    private bool acting;
	
    void Awake(){
				
		alive = true;
		castTimeModifer = 1;
		health = tempBaseHealth = baseHealth;
		defense = tempBaseDefense = baseDefense;
		crit = tempBaseCrit = baseCrit;
		critModifer = tempBaseCritModifier = baseCritModifier;
		damage = tempBaseDamage = baseDamage;
		energy = tempBaseDamage = baseEnergy;

		foreach(Ability a in abilityList){
			abilities.Add(a);
		}
		
		abilityNames = abilities.GetNames();
		eventStack = new EventStack();
		animator = GetComponent<Animator>();
		footStepSource = GetComponent<AudioSource>();
		skillMenuGenerator = GetComponent<SkillMenu>();
		GameObject statusObject = GameObject.Find("StatusText");
		statusText = statusObject != null ? statusObject.GetComponent<Text>() : null;

		if(castingCamera != null){
			castingCamera.enabled = false;
		}
	}

    protected override IEnumerator AttackRoutine(){

        Camera mainCamera = Camera.main;
        castingCamera.enabled = true;
		mainCamera.enabled = false;
        yield return new WaitForSeconds(0.25f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.15f);
        mainCamera.enabled = true;
        castingCamera.enabled = false;
        yield return new WaitForSeconds(0.75f);
        target.TakeHit(new DamageDescriptor(baseDamage, baseCrit, baseCritModifier, DamageType.Physical), this);
        hasActed = true;
		menuSpawned = false;
        acting = false;        
        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }

    IEnumerator GuardRoutine(){

        Camera mainCamera = Camera.main;

        castingCamera.enabled = true;
		mainCamera.enabled = false;
        yield return new WaitForSeconds(0.5f);
        statusText.text = (redableName + " Is Guarding");
        Guard();
        yield return new WaitForSeconds(1f);
        mainCamera.enabled = true;
        castingCamera.enabled = false;
        statusText.text = ("");        
        yield return new WaitForSeconds(1f);
        EndTurn();
    }

    IEnumerator DieRoutine(){
        
        yield return new WaitForSeconds(2f);
        gameObject.GetComponentInChildren<Renderer>().enabled = false;
		Destroy(gameObject.GetComponentInChildren<Collider>());
    }

    protected override void Die(){
		animator.SetTrigger("Die");        
        alive = false;
        StartCoroutine(DieRoutine());
    }

    public override void StartTurn(System.Action<int> endTurn, int turn){
		
		if(guarding){
			guarding = false;
			defense = tempBaseDefense;
		}

        endTurnCallback = endTurn;
        turnNumber = turn;
        int abilityIndex = 0;
        bool hasActed = false;

        if(alive){
            hasActed = false;
            GameObject[] playerParty = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] monsterParty = GameObject.FindGameObjectsWithTag("Enemy");
            if(!hasActed && HealingRoutine(monsterParty)){
                    hasActed = true;
            }
            if(!hasActed && health <= (tempBaseHealth * CombatConstants.guardTolerance)){
                if(rand.NextDouble() >= CombatConstants.guardTendancy){
                    guarding = true;
                    StartCoroutine(GuardRoutine());
                }
            }
            if(!hasActed && !guarding){   
                if(!hasActed && BuffingRoutine(monsterParty)){
                    hasActed = true;
                }
                else if(!hasActed && DebuffingRoutine(monsterParty)){
                    hasActed = true;
                }
                else if(!hasActed && OffensiveSkillRoutine(monsterParty)){
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

    public override void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){
        animator.SetTrigger("Attack");
    }

    protected bool HealingRoutine(GameObject[] allies){

        if(!abilities.HasAbilityType(AbilityType.Heal)){
            return false;
        }
        else{
            CharacterStats lowestHealthAlly = allies[0].GetComponent<CharacterStats>();

            for(int i = 1; i < allies.Length; i++){
                if(allies[i].GetComponent<CharacterStats>().GetAlive() && allies[i].GetComponent<CharacterStats>().GetHealth() <= lowestHealthAlly.GetHealth()){
                    lowestHealthAlly = allies[i].GetComponent<CharacterStats>();
                }
            }

            float allyHealth = lowestHealthAlly.GetHealth();
            float allyHealthThreshold = CombatConstants.healTolerance * lowestHealthAlly.GetBaseHealth();

            if(allyHealth <= allyHealthThreshold){
                if(rand.NextDouble() <= ((allyHealthThreshold - allyHealth) / allyHealthThreshold) + CombatConstants.vulnerabilityThreshold){
                    List<Ability> healingAblities = abilities.GetAbilities(AbilityType.Heal);
                    int abilityIndex = PickAbility(healingAblities); 
                    if(lowestHealthAlly.GetAlive() && abilityIndex >= 0){
                        CastAbility(healingAblities[abilityIndex].name, lowestHealthAlly.gameObject, true);
                        return true;
                    }
                    else{
                        return false;
                    }
                }
                return false;
            }
        }
        return false;
    }
    
    protected bool BuffingRoutine(GameObject[] allies){

        if(!abilities.HasAbilityType(AbilityType.Buff)){            
            return false;
        }
        else{
            CharacterStats mostVulnerableAlly = allies[0].GetComponent<CharacterStats>();

            for(int i = 1; i < allies.Length; i++){
                if(allies[i].GetComponent<CharacterStats>().GetAlive() && allies[i].GetComponent<CharacterStats>().NeedsDefenseBuff() >= 
                    mostVulnerableAlly.NeedsDefenseBuff()){
                    mostVulnerableAlly = allies[i].GetComponent<CharacterStats>();
                }
            }
            if(rand.NextDouble() <= (mostVulnerableAlly.NeedsDefenseBuff())){
                List<Ability> buffAbilities = abilities.GetAbilities(AbilityType.Buff);
                int abilityIndex = PickAbility(buffAbilities); 
                mostVulnerableAlly.eventStack.DumpStack();
                if(mostVulnerableAlly.GetAlive() && abilityIndex >= 0){
                    CastAbility(buffAbilities[abilityIndex].name, mostVulnerableAlly.gameObject, true);
                    return true;
                }
                else{
                    return false;
                }
            }
        }

        return false;
    }
    
    protected bool DebuffingRoutine(GameObject[] enemies){        

        if(!abilities.HasAbilityType(AbilityType.Buff)){
            return false;
        }
        else{
            CharacterStats mostAnnoyingEnemy = enemies[0].GetComponent<CharacterStats>();

            for(int i = 1; i < enemies.Length; i++){
                if(enemies[i].GetComponent<CharacterStats>().GetAlive() && enemies[i].GetComponent<CharacterStats>().NeedsDefenseBuff() <= 
                    mostAnnoyingEnemy.NeedsDefenseBuff()){
                    mostAnnoyingEnemy = enemies[i].GetComponent<CharacterStats>();
                }
            }            
            if(rand.NextDouble() <= (mostAnnoyingEnemy.NeedsDefenseBuff())){
                List<Ability> debuffAbilities = abilities.GetAbilities(AbilityType.Buff);
                int abilityIndex = PickAbility(debuffAbilities); 
                if(mostAnnoyingEnemy.GetAlive() && abilityIndex >= 0){
                    CastAbility(debuffAbilities[abilityIndex].name, mostAnnoyingEnemy.gameObject, true);
                    return true;
                }
                else{
                    return false;
                }
            }
        }

        return false;
    }
    
    protected bool OffensiveSkillRoutine(GameObject[] allies){
        
        if(!abilities.HasAbilityType(AbilityType.Damage)){
            return false;
        }
        else{
            List<BattleEvent> allEvents = new List<BattleEvent>();
            List<CharacterStats> uniqueAggressors = new List<CharacterStats>();
            List<Aggressor> aggressors = new List<Aggressor>();

            foreach(GameObject a in allies){
                foreach(BattleEvent e in a.GetComponent<CharacterStats>().eventStack.getEventStack()){
                    allEvents.Add(e);
                }
            }

            foreach(BattleEvent e in allEvents){
                if(!uniqueAggressors.Contains(e.source)){
                    uniqueAggressors.Add(e.source);
                }
            }

            foreach(CharacterStats c in uniqueAggressors){

                int numEvents = 0;
                float totalMagnitude = 0;

                foreach(BattleEvent e in allEvents){
                    if(e.GetType() == typeof(DamageEvent) && e.source == c){
                        numEvents++;
                        totalMagnitude+= e.magnitude;
                    }
                }

                aggressors.Add(new Aggressor(c, (totalMagnitude / numEvents)));
            }
            if(aggressors.Count > 0){
                Aggressor mostDangerous = aggressors[0];

                foreach(Aggressor a in aggressors){
                    if(a.AvgMagnitude >= mostDangerous.AvgMagnitude){
                        mostDangerous = a;
                    }
                }
                if(rand.NextDouble() <= (mostDangerous.AvgMagnitude / baseHealth)){
                    List<Ability> damageAbilities = abilities.GetAbilities(AbilityType.Damage);
                    int abilityIndex = PickAbility(damageAbilities); 
                    if(mostDangerous.Source.GetAlive() && abilityIndex >= 0){
                        CastAbility(damageAbilities[abilityIndex].name, mostDangerous.Source.gameObject, true);
                        return true;
                    }
                    else{                        
                        return false;
                    }
                }
            }
        }

        return false;
    }
}
