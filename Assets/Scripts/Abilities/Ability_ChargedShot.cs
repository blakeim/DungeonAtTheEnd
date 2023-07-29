using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_ChargedShot : Ability{
    public override string name{get{return "Charged Shot";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Physical;}} 
    public override float magnitude{get{return 250;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return true;}}
    public override int duration{get{return 1;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("Ability_" + string.Concat(name.Where(c => !char.IsWhiteSpace(c))), 1.5f, true);
        yield return new WaitForSeconds(1.75f);
        if(target.GetAlive()){
            target.TakeHit(new DamageDescriptor((int)magnitude, 0f, 1f, damageType), source);
            Instantiate(particleSystem, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            endTurn(cost);
        }
        else{
            endTurn(0);
        }
    }
}
