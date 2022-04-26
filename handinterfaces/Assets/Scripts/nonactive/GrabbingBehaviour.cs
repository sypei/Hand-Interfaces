using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class GrabbingBehaviour : OVRGrabber
{
    public OVRHand m_hand;
    public float pinchThreshold = 0.7f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //m_hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = m_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;
        if(!m_grabbedObj && isPinching && m_grabCandidates.Count > 0)
        {
            GrabBegin();
        }
        else if(m_grabbedObj && ! isPinching)
        {
            GrabEnd();
        }
    
    }
}
