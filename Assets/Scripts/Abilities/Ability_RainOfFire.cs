using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_RainOfFire : Ability{
    public override string name{get{return "Rain Of Fire";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Fire;}} 
    public override float magnitude{get{return 425;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("Ability_" + string.Concat(name.Where(c => !char.IsWhiteSpace(c))), 3.0f, true);
        yield return new WaitForSeconds(3.5f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.25f);
        target.TakeHit(new DamageDescriptor((int)magnitude, 0f, 1f, damageType), source);
        yield return new WaitForSeconds(0.5f);
        endTurn(cost);
    }
}
