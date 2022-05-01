using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class VirtualPiano_Demo : MonoBehaviour
{
    [SerializeField]
    private GameObject key_1;
    [SerializeField]
    private GameObject key_2;
    [SerializeField]
    private GameObject key_3;
    [SerializeField]
    private GameObject key_4;
    [SerializeField]
    private GestureDetection_Demo GD;
    private string currentInterface;

    void Start()
    {
        key_1.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
        key_2.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
        key_3.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
        key_4.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
        // key_5.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentInterface = GD.Recognize().name;
        
        if (currentInterface=="ThumbPiano"){
            key_1.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = true;
            key_2.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = true;
            key_3.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = true;
            key_4.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = true;
            // key_5.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = true;
        } else {
            key_1.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
            key_2.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
            key_3.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
            key_4.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
            // key_5.transform.Find("ButtonView").GetComponent<TrainButtonVisualController>().enabled = false;
        }   
        
    }
}
