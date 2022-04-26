using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class VirtualTrumpet : MonoBehaviour
{
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    [SerializeField]
    private StateFlag flag;
    [SerializeField]
    private CallObjects CO;
    [SerializeField]
    private GestureDetection GD;
    private string currentInterface;
    // Start is called before the first frame update
    void Start()
    {
        Button1.GetComponent<TrainButtonVisualController>().enabled = false;
        Button2.GetComponent<TrainButtonVisualController>().enabled = false;
        Button3.GetComponent<TrainButtonVisualController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag.IsGD){
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }
        if (flag.IsInteraction){
            if (currentInterface=="Trumpet"){
                // Debug.Log("will play sound");
                Button1.GetComponent<TrainButtonVisualController>().enabled = true;
                Button2.GetComponent<TrainButtonVisualController>().enabled = true;
                Button3.GetComponent<TrainButtonVisualController>().enabled = true;
            } else {
                Button1.GetComponent<TrainButtonVisualController>().enabled = false;
                Button2.GetComponent<TrainButtonVisualController>().enabled = false;
                Button3.GetComponent<TrainButtonVisualController>().enabled = false;
            }
        }
        
    }
}
