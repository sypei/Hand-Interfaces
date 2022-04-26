using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Events;

public class ButtonListener : MonoBehaviour
{
    public UnityEvent proximityEvent;
    public UnityEvent contactEvent;
    public UnityEvent actionEvent;
    public UnityEvent defaultEvent;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ButtonController>().InteractableStateChanged.AddListener(InitiateEvent);

    }

    void InitiateEvent(InteractableStateArgs state)
    {
        Debug.Log("I am in InitiateEvent function");
        if(state.NewInteractableState == InteractableState.ProximityState)
        {
            Debug.Log("I am in proximity region");
            proximityEvent.Invoke();
        }
        else if (state.NewInteractableState == InteractableState.ContactState)
        {
            contactEvent.Invoke();
        }
        else if (state.NewInteractableState == InteractableState.ActionState)
        {
            actionEvent.Invoke();
        }
        else{
            Debug.Log("I am in default branch");
            defaultEvent.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("I am in update");
        //GetComponent<ButtonController>().InteractableStateChanged.AddListener(InitiateEvent);

    }
}
