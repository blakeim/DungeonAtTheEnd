using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HowlingWind : Ability{

    public override string name{get{return "Howling Wind";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Ice;}}
    public override float magnitude{get{return 5;}}
    public override float cost{get{return 50;}}
    public override bool channeled{get{return true;}}
    public override int duration{get{return 2;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("CastSpell", 0f, true);
        yield return new WaitForSeconds(3f);
        if(target.GetAlive()){
            Instantiate(particleSystem, target.transform.position, Quaternion.identity);
            target.DebuffStat((int)magnitude, "CRITICALCHANCE");
            yield return new WaitForSeconds(1f);
            target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
            endTurn(cost);
        }
        else{
            endTurn(0);
        }
    }
}
