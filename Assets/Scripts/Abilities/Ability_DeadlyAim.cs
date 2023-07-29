using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_DeadlyAim : Ability{
    public override string name{get{return "Deadly Aim";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Physical;}} 
    public override float magnitude{get{return 155;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("Ability_" + string.Concat(name.Where(c => !char.IsWhiteSpace(c))), 0f, false);
        yield return new WaitForSeconds(0.75f);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        target.TakeHit(new DamageDescriptor((int)magnitude, 1f, 2f, damageType), source);
        yield return new WaitForSeconds(1f);
        endTurn(cost);
    }
}
