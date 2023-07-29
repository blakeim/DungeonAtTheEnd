using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability_ShieldWall : Ability{

    public override string name{get{return "Shield Wall";}}
    public override AbilityType type{get{return AbilityType.Buff;}}
    public override DamageType damageType{get{return DamageType.None;}}
    public override float magnitude{get{return 5;}}
    public override float cost{get{return 25;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("CastSpell", 0f, true);
        yield return new WaitForSeconds(2f);
        target.BuffStat(magnitude, "DEFENSE");
        Instantiate(particleSystem, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        endTurn(cost);
    }
}
