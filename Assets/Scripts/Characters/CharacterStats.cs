using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
	
	public string redableName;
	public EventStack eventStack;

	[SerializeField]protected int baseHealth;	
	[SerializeField]protected int baseEnergy;
	[SerializeField]protected int baseDamage;
	[SerializeField]protected float baseCrit;
	[SerializeField]protected float baseCritModifier;
	[SerializeField]protected int baseDefense;
	protected int tempBaseHealth;	
	protected int tempBaseEnergy;
	protected int tempBaseDamage;
	protected float tempBaseCrit;
	protected float tempBaseCritModifier;
	protected int tempBaseDefense;

	[SerializeField]protected DamageType weakness = DamageType.None;
	[SerializeField]protected float weaknessLevel = 2;
	[SerializeField]protected DamageType resistance = DamageType.None;
	[SerializeField]protected float resistanceLvel = 2;
	[SerializeField]protected Camera castingCamera;
	[SerializeField]protected List<Ability> abilityList;	
	[SerializeField]protected List<Item> itemList;

	protected bool hasActed;
	protected bool guarding;
	protected int health;		
	protected int defense;	
	protected int damage;	
	protected float crit;	
	protected float critModifer;	
	protected int energy;

	protected int turnNumber;
	protected bool alive;
	protected bool menuSpawned;
	protected SkillMenu skillMenuGenerator;
	protected Text statusText;
	protected List<Ability> Abilities;
	protected Animator animator;
	protected Vector3 startingPosition;
	protected System.Random rand = new System.Random();
	protected System.Action<int> endTurnCallback;
	protected CharacterStats target;
	protected AbilityList abilities = new AbilityList();
	protected AbilityList inventory = new AbilityList();
	protected string[] combatMenu;
	protected string[] abilityNames;
	protected string[] itemNames;
	protected AudioSource footStepSource;
	protected PlayerParty partyManager;
	protected AudioClip attackClip;
	protected AudioClip deathClip;	
	protected AudioClip damageClip;
	protected AudioClip castingClip;
	protected bool casting;
	protected int remainigCastTime;	
	protected float castTimeModifer;
	protected Ability channledAbility;

	// Use this for initialization
	void Awake(){
		
		DontDestroyOnLoad(this.gameObject);
		
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

	void Update(){

		if(health <= 0){
			Die();
		}
	}
	
	public void Heal(float healing){

		if(health + healing > tempBaseHealth){
			health = tempBaseHealth;
		}
		else{
			health += Mathf.RoundToInt(healing);
		}
	}

	public void BuffStat(float magnitude, string stat){
		
		switch(stat){
			case "DEFENSE":
				defense += (int)magnitude;
				tempBaseDefense = defense;
				break;
			case "CRITICALCHANCE":
				crit += magnitude;
				tempBaseCrit = crit;
				break;
			case "DAMAGE":
				damage += (int)magnitude;
				tempBaseDamage = damage;
				break;
			default:
				break;
		}
	}	
	
	public void DebuffStat(int magnitude, string stat){
		
		switch(stat){
			case "DEFENSE":
				defense -= magnitude;
				if(defense <= 0){
					defense = 1;
				}

				tempBaseDefense = defense;
				break;
			case "CRITICALCHANCE":
				crit -= magnitude;
				
				if(crit <= 0){
					crit = 0;
				}

				tempBaseCrit = crit;
				break;
			case "DAMAGE":
				damage -= magnitude;				
				
				if(damage <= 0){
					damage = 1;
				}

				tempBaseDamage = damage;
				break;
			default:
				break;
		}
	}

	public void TakeHit(DamageDescriptor damage, CharacterStats source){
		
		float instDamage = (damage.getDamage() * (Time.deltaTime * 1000)) / defense;
		bool critical = false;

		if(rand.NextDouble() < damage.getCriticalChance()){
			print("Critical hit");
			instDamage *= damage.getCriticalModifier();
			critical = true;
		}
		if(damage.getDamageType() == weakness){
			instDamage *= weaknessLevel;
		}
		else if(damage.getDamageType() == resistance){
			instDamage /= resistanceLvel;
		}

		health -= Mathf.RoundToInt(instDamage);
		//print("Took " + instDamage + "damage. " + "Health is " + health);
		if(!guarding){
			animator.SetTrigger("Damage");
		}
		else{
			animator.SetTrigger("Block");
		}

		if(damage.getDamageType() == resistance){
			float resisted = (instDamage * resistanceLvel);
			instDamage -= resisted;
		}

		if(instDamage > 0){
			eventStack.Add(new DamageEvent(this, source, damage.getDamageType(), instDamage, critical));
		}
	}

	public void ToggleCelebration(){
		bool celebrating = animator.GetBool("Victory");
		animator.SetBool("Victory", false);
	}

	public void SetStartingPoint(Vector3 position){

		startingPosition = position;
	}

	public void ResetAnimation(){

		animator.SetBool("Moving", false);
		animator.SetTrigger("ExitBattleMode");
		animator.SetInteger("Direction", 0);
	}

	public void ResetStats(){

		defense = tempBaseDefense = baseDefense;
		crit = tempBaseCrit = baseCrit;
		critModifer = tempBaseCritModifier = baseCritModifier;
		damage = tempBaseDamage = baseDamage;
	}

	public void ResetBattleVars(){
		
		hasActed = guarding = menuSpawned = false;
	}

	public void SetPartyManager(PlayerParty party){
		partyManager = party;
	}

	public void DisablePhysics(){

		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<BoxCollider>().enabled = false;
	}

	public void EnablePhysics(){

		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<BoxCollider>().enabled = true;
	}

	public bool GetHasActed(){
		return hasActed;
	}
	
	public void SetHasActed(bool acted){
		hasActed = acted;
	}

	public bool GetAlive(){
		return alive;
	}

	public int GetHealth(){
		return health;
	}
	
	public int GetEnergy(){
		return energy;
	}

	public int GetBaseHealth(){
		return tempBaseHealth;
	}

	public virtual void StartTurn(System.Action<int> endTurn, int turn){

		if(alive){				
			
			endTurnCallback = endTurn;
			turnNumber = turn;

			if(!casting){
				inventory = partyManager.GetInventory();
			
				foreach(Item i in itemList){
					if(i.quantity <= 0){
						inventory.Remove(i);
					}
				}
				
				itemNames = inventory.GetNames();

				if(abilities.Count > 0 && inventory.Count > 0){
					combatMenu = new string[]{"Attack", "Abilities", "Guard", "Items", "Run Away"};
				}
				else if(abilities.Count == 0 && inventory.Count > 0){
					combatMenu = new string[]{"Attack", "Guard", "Items", "Run Away"};
				}
				else if(abilities.Count > 0 && inventory.Count == 0){
					combatMenu = new string[]{"Attack", "Abilities", "Guard", "Run Away"};
				}
				else if(abilities.Count == 0 && inventory.Count == 0){
					combatMenu = new string[]{"Attack", "Guard", "Run Away"};
				}

				if(guarding){
					guarding = false;
					defense = tempBaseDefense;
				}
				if(skillMenuGenerator != null && !menuSpawned){
					skillMenuGenerator.CreateMenu(0f, 2.5f, combatMenu);
					menuSpawned = true;
				}
			}
			else{
				remainigCastTime -= 1;
				
				if(remainigCastTime == 0){
					casting = false;
					animator.SetBool("Channeling", false);
					CastAbility(channledAbility.name, target.gameObject, true);
				}
				else{
					print(redableName + " is currently channeling " + channledAbility.name);
					print("Remaining cast time " + remainigCastTime);
					EndTurn();
				}
			}
		}
		else{
			EndTurn();
		}
	}

	public void SpawnAbilityMenu(){
		
		if(skillMenuGenerator != null){
			skillMenuGenerator.CreateMenu(1f, 2f, abilities, energy);
		}
	}	

	public void SpawnItemMenu(){
		
		if(skillMenuGenerator != null){
			skillMenuGenerator.CreateMenu(1f, 2f, itemNames);
		}		
	}	

		
	public void DestroyMenus(){
		
		if(menuSpawned){
			skillMenuGenerator.DestroyMenu();
			menuSpawned = false;
		}

	}
		
	public void CastAbility(string abilityName, GameObject target, bool ignoreChannel){
		
		this.target = target.GetComponent<CharacterStats>();
		Ability tempAbility = abilities.Get(abilityName);
		Ability tempItem = inventory.Get(abilityName);
		if(tempAbility != null){
			DestroyMenus();
			if(ignoreChannel || !tempAbility.channeled){
				StartCoroutine(tempAbility.CastAbility(target.GetComponent<CharacterStats>(), this, EndCastTurn, TriggerAnimate));
			}
			else{
				casting = true;
				animator.SetBool("Channeling", true);
				channledAbility = tempAbility;
				remainigCastTime = (int)(tempAbility.duration * castTimeModifer);
				this.target = target.GetComponent<CharacterStats>();
				EndTurn();
			}
		}
		else if(tempItem != null){
			DestroyMenus();
			StartCoroutine(tempItem.CastAbility(target.GetComponent<CharacterStats>(), this, EndCastTurn, TriggerAnimate));
		}
		else{
			print("Ability " + abilityName + " not found");
		}
	}
	
	public string[] GetAbilityNames(){
		
		return abilityNames;
	}


	public void EndTurn(){

		endTurnCallback(turnNumber);
	}

	public virtual void EndCastTurn(float energyCost){

		energy -= (int)energyCost;
		endTurnCallback(turnNumber);
	}
	
	public virtual void Attack(GameObject target){
		DestroyMenus();
		this.target = target.GetComponent<CharacterStats>();
		StartCoroutine(AttackRoutine());
	}

	public int GetTargetType(string abilityName){
		
		Ability tempAbility = abilities.Get(abilityName);
		Ability 	tempItem  = inventory.Get(abilityName);

		if( tempAbility != null){
			return GetTargetType(tempAbility);
		}
		else if(tempItem != null){
			return GetTargetType(tempItem);
		}

		return -1;
	}

	protected int GetTargetType(Ability ability){

		switch(ability.type){
			case AbilityType.Heal:
				return 1;
			case AbilityType.Buff:
				return 1;
			case AbilityType.Damage:
				return 0;
			case AbilityType.DeBuff:
				return 0;
		}

		return -1;
	}
	protected virtual IEnumerator AttackRoutine(){
		
		DestroyMenus();
		TriggerAnimate("Attack_00", 0f, false);
		target.TakeHit(new DamageDescriptor(damage, crit, critModifer, DamageType.Physical), this);
		yield return new WaitForSeconds(0.5f);
		RecoverEnergy();
		EndTurn();
	}

	public void RecoverEnergy(){

		int recovery = rand.Next(2, (int)(10*critModifer));

		if(energy + recovery >= baseEnergy){
			energy = baseEnergy;
		} 
		else{
			energy+= recovery;
		}
	}

	public virtual void Guard(){

		DestroyMenus();
		guarding = true;
		defense = defense * 2;
	}

	protected virtual void Die(){
		
		if(alive){
			alive = false;		
			animator.SetBool("Channeling", false);
			animator.SetBool("Die", true);
			gameObject.GetComponentInChildren<Renderer>().enabled = false;
			Destroy(gameObject.GetComponentInChildren<Collider>());
		}
	}

	public virtual void TriggerAnimate(string triggerName, float attackTimer, bool castingCamera){
		print("Animating with " + triggerName);
		animator.SetTrigger(triggerName);
	}

	public float NeedsDefenseBuff(){

        float damageHistSum = 0f;
        float damageHistMean = 0f;
        int damageEventCount =0;

        foreach(BattleEvent b in eventStack.getEventStack()){
            if(b.GetType() == typeof(DamageEvent)){
                damageHistSum += b.magnitude;
                damageEventCount++;
            }
        }

        damageHistMean = damageHistSum / damageEventCount;
		if(damageHistMean / baseHealth >= CombatConstants.highDamageRatio){
        	return ((damageHistMean / baseHealth) - CombatConstants.highDamageRatio) * 10;
		}

		return 0f;
    }

	public int PickAbility(List<Ability> abilities){

		if(abilities == null || abilities.Count <= 0){
			return -1;
		}
		
		Ability chosenAbility = abilities[0];
		int index = 0;

		for(int i = 0; i < abilities.Count; i++){
			if(abilities[i].cost <= energy && ((System.Math.Abs(chosenAbility.magnitude - abilities[i].magnitude) < CombatConstants.abilityMagnitudeThreshhold 
					&& abilities[i].cost < chosenAbility.cost && energy <= (tempBaseEnergy * CombatConstants.energyThreshold)) 
					|| abilities[i].magnitude >= chosenAbility.magnitude)){
				index = i;
				chosenAbility = abilities[i];
			}
		}

		if(chosenAbility.cost > energy){
			return -1;
		}
		else{
			return index;
		}
	}

    public float NeedsDefenseDebuff(){
    
        return 0f;
    }

    public float IsVulnerable(DamageType damageType){

        return 0f;
    }
}
