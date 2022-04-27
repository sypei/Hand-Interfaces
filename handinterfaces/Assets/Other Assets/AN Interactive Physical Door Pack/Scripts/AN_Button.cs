using UnityEngine;

public class AN_Button : MonoBehaviour
{
    [Tooltip("True for rotation like valve (used for ramp/elevator only)")]
    public bool isValve = false;
    [Tooltip("SelfRotation speed of valve")]
    public float ValveSpeed = 10f;
    [Tooltip("If it isn't valve, it can be lever or button (animated)")]
    public bool isLever = false;
    [Tooltip("If it is false door can't be used")]
    public bool Locked = false;
    [Tooltip("The door for remote control")]
    public AN_DoorScript DoorObject;
    [Space]
    [Tooltip("Any object for ramp/elevator baheviour")]
    public Transform RampObject;
    [Tooltip("Door can be opened")]
    public bool CanOpen = true;
    [Tooltip("Door can be closed")]
    public bool CanClose = true;
    [Tooltip("Current status of the door")]
    public bool isOpened = false;
    [Space]
    [Tooltip("True for rotation by X local rotation by valve")]
    public bool xRotation = true;
    [Tooltip("True for vertical movenment by valve (if xRotation is false)")]
    public bool yPosition = false;
    public float max = 90f, min = 0f, speed = 5f;
    bool valveBool = true;
    float current, startYPosition;
    Quaternion startQuat, rampQuat;

    Animator anim;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        startYPosition = RampObject.position.y;
        startQuat = transform.rotation;
        rampQuat = RampObject.rotation;
    }

    void Update()
    {
        if (!Locked)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isValve && DoorObject != null && DoorObject.Remote && NearView()) // 1.lever and 2.button
            {
                DoorObject.Action(); // void in door script to open/close
                if (isLever) // animations
                {
                    if (DoorObject.isOpened) anim.SetBool("LeverUp", true);
                    else anim.SetBool("LeverUp", false);
                }
                else anim.SetTrigger("ButtonPress");
            }
            else if (isValve && RampObject != null) // 3.valve
            {
                // changing value in script
                if (Input.GetKey(KeyCode.E) && NearView())
                {
                    if (valveBool)
                    {
                        if (!isOpened && CanOpen && current < max) current += speed * Time.deltaTime;
                        if (isOpened && CanClose && current > min) current -= speed * Time.deltaTime;

                        if (current >= max)
                        {
                            isOpened = true;
                            valveBool = false;
                        }
                        else if (current <= min)
                        {
                            isOpened = false;
                            valveBool = false;
                        }
                    }

                }
                else
                {
                    if (!isOpened && current > min) current -= speed * Time.deltaTime;
                    if (isOpened && current < max) current += speed * Time.deltaTime;
                    valveBool = true;
                }

                // using value on object
                transform.rotation = startQuat * Quaternion.Euler(0f, 0f, current * ValveSpeed);
                if (xRotation) RampObject.rotation = rampQuat * Quaternion.Euler(current, 0f, 0f); // I have a doubt in working correctly
                else if (yPosition) RampObject.position = new Vector3(RampObject.position.x, startYPosition + current, RampObject.position.z);
            }
        }
    }

    bool NearView() // it is true if you near interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        if (angleView < 45f && distance < 2f) return true;
        else return false;
    }
}
