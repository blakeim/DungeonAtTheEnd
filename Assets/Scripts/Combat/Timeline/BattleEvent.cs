using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent{
    
    public CharacterStats target {get; set;}
    public CharacterStats source {get; set;}
    public float magnitude {get; set;} 

    public BattleEvent(CharacterStats target, CharacterStats source){
        
        this.target = target;
        this.source = source;
    }
}
