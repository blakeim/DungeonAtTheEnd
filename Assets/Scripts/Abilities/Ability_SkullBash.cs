using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_SkullBash : Ability{
    public override string name{get{return "Skull Bash";}}
    public override AbilityType type{get{return AbilityType.DeBuff;}}
    public override DamageType damageType{get{return DamageType.Physical;}} 
    public override float magnitude{get{return 200;}}
    public override float cost{get{return 10;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("Ability_" + string.Concat(name.Where(c => !char.IsWhiteSpace(c))), 3f, false);
        yield return new WaitForSeconds(1.5f);
        target.TakeHit(new DamageDescriptor((int)magnitude, 0f, 1f, damageType), source);
        target.DebuffStat(5, "DEFENSE");        
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);

        endTurn(cost);
    }
}