using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SharpShooting : Ability{

    public override string name{get{return "Sharp Shooting";}}
    public override AbilityType type{get{return AbilityType.Buff;}}
    public override DamageType damageType{get{return DamageType.None;}}
    public override float magnitude{get{return 0.75f;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("CastSpell", 3f, true);
        yield return new WaitForSeconds(3f);
        target.BuffStat(magnitude, "CRITICALCHANCE");
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        endTurn(cost);
    }
}
