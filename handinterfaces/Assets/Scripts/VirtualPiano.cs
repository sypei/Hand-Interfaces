using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class VirtualPiano : MonoBehaviour
{
    [SerializeField]
    private GameObject key_1;
    [SerializeField]
    private GameObject key_2;
    [SerializeField]
    private GameObject key_3;
    [SerializeField]
    private GameObject key_4;
    // [SerializeField]
    // private GameObject key_5;
    [SerializeField]
    private StateFlag flag;
    [SerializeField]
    private CallObjects CO;
    [SerializeField]
    private GestureDetection GD;
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
        if (flag.IsGD){
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }
        
        //Debug.Log("piano Interface"+currentInterface);
        if (flag.IsInteraction){
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
}
