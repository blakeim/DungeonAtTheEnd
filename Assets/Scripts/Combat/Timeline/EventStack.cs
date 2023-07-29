using System.Collections;
using System.Collections.Generic;

public class EventStack{
    
    public int eventMemory = 5;

    private List<BattleEvent> events = new List<BattleEvent>();
    
    public void Add(BattleEvent battlEvent){

        if(events.Count == eventMemory){
            events.RemoveAt(0);
        }

        events.Add(battlEvent);
    }

    public List<BattleEvent> getEventStack(){
        return events;
    }

    public void DumpStack(){
        events = new List<BattleEvent>();
    }
}
