using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlled_Demo : MonoBehaviour
{
    public GameObject joystickTop;
    public GameObject joystickBase;
    public GameObject target;
    [SerializeField]
    private GestureDetection_Demo GD;
    [SerializeField]
    private VRHandGrab hg;
    private string currentInterface;
    // Start is called before the first frame update
    private Vector3 diff = new Vector3(0f,0f,0f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        diff = joystickTop.transform.position-joystickBase.transform.position;
        currentInterface = GD.Recognize().name;
        if(currentInterface == "Joystick"&&hg.FingerPinchStrength >= hg.minFingerPinchDownStrength)
            target.transform.Translate(diff.x*5*Time.deltaTime,0f,diff.z*5*Time.deltaTime);
    }
}
