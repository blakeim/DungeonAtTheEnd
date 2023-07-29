using System.Collections;
using System.Collections.Generic;

public struct  Aggressor{
    
    public Aggressor(CharacterStats source, float avgMagnitude){
        Source = source;
        AvgMagnitude = avgMagnitude;
    }

    public CharacterStats Source { get; }
    public float AvgMagnitude { get; }

    public override string ToString() => $"({Source}, {AvgMagnitude})";
}

