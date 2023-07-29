using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HealingPotion : Item{

    public override string name{get{return "Health Potion";}}
    public override AbilityType type{get{return AbilityType.Heal;}}
    public override DamageType damageType{get{return DamageType.Physical;}} 
    public override float magnitude{get{return 300;}}
    public override float cost{get{return 0;}}
    public override bool channeled{get{return false;}}

    public override IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate){
        
        if(quantity > 0){
            print(quantity);
            target.Heal(magnitude);        
            yield return new WaitForSeconds(1f);
            quantity --;
        }

        yield return new WaitForSeconds(0);
        endTurn(cost);
    }
}
