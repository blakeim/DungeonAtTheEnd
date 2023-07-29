using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HealingLight : Ability{
    
    public override string name{get{return "Healing Light";}}
    public override AbilityType type{get{return AbilityType.Heal;}}
    public override DamageType damageType{get{return DamageType.None;}}
    public override float magnitude{get{return 150;}}
    public override float cost{get{return 10;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("CastSpell", 0f, true);
        yield return new WaitForSeconds(2f);
        target.Heal(magnitude);
        yield return new WaitForSeconds(1f);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);        
        yield return new WaitForSeconds(0.5f);
        print("Spell cast");
        endTurn(cost);
    }
}
