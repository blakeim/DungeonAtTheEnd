using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour{

    public abstract string name{get;}
    public abstract AbilityType type{get;}
    public abstract DamageType damageType{get;}
    public abstract float magnitude{get;}
    public abstract float cost{get;}
    public abstract bool channeled{get;}

    public virtual int duration{get{return 1;}}
    public GameObject particleSystem;
    public abstract IEnumerator CastAbility(CharacterStats target, CharacterStats source, System.Action<float> endTurn, System.Action<string, float, bool> animate);
}
