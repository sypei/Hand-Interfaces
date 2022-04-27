using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_pointer : MonoBehaviour
{
    private float dist1 = Mathf.Infinity;
    private float dist2 = Mathf.Infinity;
    private float dist3 = Mathf.Infinity;
    private GameObject dummy;
    private GameObject light_1;
    private GameObject light_2;
    private GameObject light_3;
    [SerializeField]
    private GestureRetrieval_IoT GD;
    public int light_No=0;
    // Start is called before the first frame update
    void Start()
    {
        dummy = GameObject.Find("joystick");
        light_1 = GameObject.Find("light_1");
        light_2 = GameObject.Find("light_2");
        light_3 = GameObject.Find("light_3");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GD.currentGesture_stable.name == "joystick"){
            var a = DistanceCalculator(dummy, light_1);
            var b = DistanceCalculator(dummy, light_2);
            var c = DistanceCalculator(dummy, light_3);
            if (a <= b && a <= c)
                light_No = 1;
            else if (b <= a && b <= c)
                light_No = 2;
            else
                light_No = 3;
        }
    }


    private float DistanceCalculator(GameObject dummy, GameObject light)
    {
        float distance = 0;
        distance = Vector3.Distance(dummy.transform.position,light.transform.position);
        return distance;
    }
}
