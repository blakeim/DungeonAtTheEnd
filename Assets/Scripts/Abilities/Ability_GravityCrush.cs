using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_GravityCrush : Ability{
    
    public override string name{get{return "Gravity Crush";}}
    public override AbilityType type{get{return AbilityType.Damage;}}
    public override DamageType damageType{get{return DamageType.Lightning;}}
    public override float magnitude{get{return 5;}}
    public override float cost{get{return 35;}}
    public override bool channeled{get{return true;}}
    public override int duration{get{return 2;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        animate("CastSpell", 0f, true);
        yield return new WaitForSeconds(2f);
        target.DebuffStat(5, "DEFENSE");        
        target.DebuffStat(5, "DAMAGE");        
        if(target.GetAlive()){
            Instantiate(particleSystem, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            target.TakeHit(new DamageDescriptor(0, 0f, 1f, damageType), source);
            endTurn(cost);
        }
        else{
            endTurn(0);
        }
    }
}
