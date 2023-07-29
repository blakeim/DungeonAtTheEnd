using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityList : List<Ability>{

    private bool hasHeal;
    private bool hasDeBuff;
    private bool hasBuff;
    private bool hasDamage;
    
    private bool hasCheckedHeal = false;
    private bool hasCheckedBuff = false;
    private bool hasCheckedDeBuff = false;
    private bool hasCheckedDamage = false;

    private List<Ability> healList;
    private List<Ability> buffList;
    private List<Ability> deBuffList;
    private List<Ability> damageList;

    public bool HasAbilityType(AbilityType typeFilter){

        bool hasAbilityType = false;

        switch(typeFilter){
            case AbilityType.Heal:
                hasAbilityType = HasHeals();
                break;
            case AbilityType.Buff:
                hasAbilityType = HasBuffs();
                break;            
            case AbilityType.DeBuff:
                hasAbilityType = HasDeBuffs();
                break;            
            case AbilityType.Damage:
                hasAbilityType = HasDamage();
                break;
        }

        return hasAbilityType;
    }

    public List<Ability> GetAbilities(AbilityType typeFilter){
        
        List<Ability> sublist = new List<Ability>();

        switch(typeFilter){
            case AbilityType.Heal:
                sublist = GetListHeal();
                break;
            case AbilityType.Buff:
                sublist = GetListBuff();
                break;            
            case AbilityType.DeBuff:
                sublist = GetListDeBuff();
                break;            
            case AbilityType.Damage:
                sublist = GetListDamage();
                break;
        }

        return sublist;
    }

    public Ability Get(string abilityName){

        for(int i = 0; i < Count; i++){
                if(this[i].name.Equals(abilityName)){
                    return this[i];
                }
            }

        return null;
    }
    
    public string[] GetNames(){

        string[] abilityNames = new string[Count];
        for(int i = 0; i < Count; i++){
                abilityNames[i] = this[i].name;
            }

        return abilityNames;
    }
        
    private bool HasHeals(){

        if(!hasCheckedHeal){
            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Heal)){
                    hasHeal = true;
                    hasCheckedHeal = true;
                    return true;
                }
            }

            hasCheckedHeal = true;
        }

        return hasHeal;
    }
    
    private bool HasBuffs(){
        
        if(!hasCheckedBuff){
            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Heal)){
                    hasBuff = true;
                    hasCheckedBuff = true;
                    return true;
                }
            }

            hasCheckedBuff = true;
        }

        return hasBuff;
    }
    
    private bool HasDeBuffs(){
        
        if(!hasCheckedDeBuff){
            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Heal)){
                    hasDeBuff = true;
                    hasCheckedDeBuff = true;
                    return true;
                }
            }

            hasCheckedDeBuff = true;
        }

        return hasDeBuff;
    }
    
    private bool HasDamage(){
        
        if(!hasCheckedDamage){
            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Heal)){
                    hasDamage = true;
                    hasCheckedDamage = true;
                    return true;
                }
            }

            hasCheckedDamage = true;
        }

        return hasDamage;
    }

    private List<Ability> GetListHeal(){

        if(healList == null){
            
            healList = new List<Ability>();

            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Heal)){
                    healList.Add(this[i]);
                }
            }
        }

        return healList;
    }
    
    private List<Ability> GetListBuff(){
        
        if(buffList == null){
            
            buffList = new List<Ability>();

            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Buff)){
                    buffList.Add(this[i]);
                }
            }
        }

        return buffList;
    }
    
    private List<Ability> GetListDeBuff(){
        
        if(deBuffList == null){
            
            deBuffList = new List<Ability>();

            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.DeBuff)){
                    deBuffList.Add(this[i]);
                }
            }
        }

        return deBuffList;
    }
    
    private List<Ability> GetListDamage(){
        
        if(damageList == null){
            
            damageList = new List<Ability>();

            for(int i = 0; i < Count; i++){
                if(this[i].type.Equals(AbilityType.Damage)){
                    damageList.Add(this[i]);
                }
            }
        }

        return damageList;
    }
}
