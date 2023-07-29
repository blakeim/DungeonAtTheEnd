using System.Collections;
using System.Collections.Generic;

public class Brain{

    protected virtual bool Act(){

       return false; 
    }

    protected virtual Ability ChooseAbility(AbilityList abilities){

        return abilities[0];
    }

    protected virtual CharacterStats ChooseTarget(CharacterStats[] pottentialTargets){

        return pottentialTargets[0];
    }
}