using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_OneThousandKnives : Ability{
    
    public override string name{get{return "Thousand Knives";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Physical;}} 
    public override float magnitude{get{return 300;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("Ability_" + string.Concat(name.Where(c => !char.IsWhiteSpace(c))), 6f, false);
        yield return new WaitForSeconds(1.75f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        target.TakeHit(new DamageDescriptor((int)magnitude, 0f, 1f, damageType), source);
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.75f);
        endTurn(cost);
    }
}
