using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct  DamageDescriptor{

    private int          damage;
    private float        criticalChance;
    private float        criticalModifier;
    private DamageType   damageType;

    public DamageDescriptor(int dmg, float critChance, float critModifier, DamageType damageT){

        damage = dmg;
        criticalChance = critChance;
        criticalModifier = critModifier;
        damageType = damageT;
    }

    public int getDamage(){

        return damage;
    }
    
    public float getCriticalChance(){

        return criticalChance;
    }
    
    public float getCriticalModifier(){

        return criticalModifier;
    }

    public DamageType getDamageType(){

        return damageType;
    }

    public override string ToString() => $"({damage} {damageType} damage, with {criticalChance} chance to do {criticalModifier}X damage)";
}
