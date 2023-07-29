using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour{

    [SerializeField]private int flickerIntervalMax;
    [SerializeField]private int flickerIntervalMin;

    [SerializeField]private int intensityMax;
    [SerializeField]private int intensityMin;
    
    [SerializeField]private float flickerStep;

    [SerializeField]private Light lightSource;

    private int flickerInterval;    
    private int flickerDirection;    
    private int desiredIntensity;
    private System.Random rand = new System.Random();

    void Start(){

        flickerInterval = rand.Next(flickerIntervalMin, flickerIntervalMax);
        lightSource = gameObject.GetComponent<Light>();
        desiredIntensity = rand.Next(intensityMin, intensityMax);  
        SetFlickerDirection(lightSource.intensity);
    }

    void Update(){

        if(System.Math.Abs(lightSource.intensity - desiredIntensity) > flickerStep){
            lightSource.intensity += (flickerStep * flickerDirection);
        }
        else{
            desiredIntensity = rand.Next(intensityMin, intensityMax);  
            SetFlickerDirection(lightSource.intensity);
        }
        
    }

    private void SetFlickerDirection(float lightIntensity){

        if(lightIntensity > desiredIntensity){
            flickerDirection = -1;
        }
        else if(lightIntensity < desiredIntensity){
            flickerDirection = 1;
        }
    }
}
