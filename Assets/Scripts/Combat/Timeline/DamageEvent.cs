using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : BattleEvent{

    private DamageType damageType {get; set;} 
    private float damage {get; set;} 
    private bool critical {get; set;} 

    public DamageEvent(CharacterStats target, CharacterStats source, DamageType damageType, float damage, bool critical): base(target, source){

        this.damageType = damageType;
        this.damage = this.magnitude = damage;
        this.critical = critical;
    }
}
