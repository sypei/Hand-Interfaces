using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mug_i : MonoBehaviour
{
    [SerializeField] private Transform mug;
    [SerializeField] private Transform mouth;
    private float threshold = 0.2f;
    [SerializeField] private AudioSource drink_sound;
    [SerializeField] private RawImage heart_3;
    private int stay = 0;
    private Color32 newColor = new Color32(255,255,225,255); 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        float dist = Vector3.Distance(mouth.position, mug.position);
        if (dist < threshold){
            if(mug.gameObject.GetComponent<Renderer>().enabled == true)
            {
                stay +=1;
                if (stay == 1){
                    drink_sound.Play(0);
                    heart_3.GetComponent<RawImage>().color = newColor;
                }
            } 
        }
        else{
            stay = 0;
        }

    }
}
