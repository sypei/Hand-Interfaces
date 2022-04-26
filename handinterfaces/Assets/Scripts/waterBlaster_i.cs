using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBlaster_i : MonoBehaviour
{
    private float timeRemaining = 3;
    private bool waterOn = true;
    private ParticleSystem water;
    // Start is called before the first frame update
    void Start()
    {
        
        water = GetComponentsInChildren<ParticleSystem>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else if (waterOn == true){
            water.Stop();
            waterOn = false;
            timeRemaining = 3;
        }
        else if (waterOn == false){
            water.Play();
            waterOn = true;
            timeRemaining = 3;
        }
            
    }
}
