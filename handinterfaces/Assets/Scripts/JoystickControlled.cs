using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlled : MonoBehaviour
{
    public GameObject joystickTop;
    public GameObject joystickBase;
    public GameObject target;
    [SerializeField]
    private VRHandGrab hg;
    public CallObjects CO;
    // Start is called before the first frame update
    private Vector3 diff = new Vector3(0f,0f,0f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        diff = joystickTop.transform.position-joystickBase.transform.position;
        // Debug.Log("Base Position"+joystickBase.transform.position);
        // Debug.Log("Top Position"+joystickTop.transform.position);
        // Debug.Log("Position Difference"+diff);
        if(CO.currentInterface == "Joystick"&&hg.FingerPinchStrength >= hg.minFingerPinchDownStrength)
            target.transform.Translate(diff.x*5*Time.deltaTime,0f,diff.z*5*Time.deltaTime);
    }
}
