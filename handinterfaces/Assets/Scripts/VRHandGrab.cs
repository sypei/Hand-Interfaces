using System.Linq;
using UnityEngine;
using System.Collections.Generic;


public enum HandToTrack
{
    Left,
    Right
}

public class VRHandGrab : MonoBehaviour
{   
    public StateFlag flag;
    [SerializeField]
    private HandToTrack handToTrack = HandToTrack.Left;

    [SerializeField]
    public float minFingerPinchDownStrength = 0.5f;

    [SerializeField]
    private GameObject grabbableObject;
    private GameObject grandparent;
    
    [SerializeField]
    private bool IsBimanual = true;

    [SerializeField]
    private bool IsTouchMode = false;

    [SerializeField]
    private bool IsJoystick = false;

    [SerializeField]
    private bool isFishingRod = false;

    [SerializeField]
    private GameObject handToTrackMovement;
    [SerializeField]
    private GameObject theOtherhandToTrackMovement;

    [SerializeField]
    private float distanceThreshold = 0.05f; //max distance to activate grabbing

    [SerializeField]
    private GameObject editorhandToTrackMovement;

    [SerializeField]
    private bool allowEditorControls = true;

    public bool IsPinchingReleased = false;
    public bool isFingerPinching;
    public float FingerPinchStrength;

#region Oculus Types

    private OVRHand ovrHand;

    private OVRSkeleton ovrSkeleton;
    private OVRSkeleton theOtherovrSkeleton;

    private OVRBone boneToTrack;
    private OVRBone theOtherboneToTrack;
#endregion

    void Awake() 
    {
        ovrHand = handToTrackMovement.GetComponent<OVRHand>();
        ovrSkeleton = handToTrackMovement.GetComponent<OVRSkeleton>();
        theOtherovrSkeleton = theOtherhandToTrackMovement.GetComponent<OVRSkeleton>();
        
        //grabbableObject = GameObject.Find("JoystickTop");
        //handToTrackMovement = GameObject.Find("OVRHandPrefabLeft");
        //theOtherhandToTrackMovement = GameObject.Find("OVRHandPrefabRight");

        #if UNITY_EDITOR
        
        // if we allow editor controls use the editor object to track movement because oculus
        // blocks the movement of LeftControllerAnchor and RightControllerAnchor
        if(allowEditorControls)
        {
            handToTrackMovement = editorhandToTrackMovement != null ? editorhandToTrackMovement : handToTrackMovement;
        }

        #endif
        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
        
        theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
        //VRLogInfo.Instance.LogInfo("boneToTrack is null: " + (boneToTrack == null));
    }

    void Update()
    {
        if (boneToTrack == null)
        {
            boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
            if (boneToTrack != null)
                handToTrackMovement = boneToTrack.Transform.gameObject;
        }

        if (theOtherboneToTrack == null)
        {
            theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
            if (boneToTrack != null)
                theOtherhandToTrackMovement = theOtherboneToTrack.Transform.gameObject;
        }
        InteractionEnabler(grabbableObject);
    }

    private float DistanceCalculator(GameObject grabbableObject)
    {
        float distance = 0;
        
        distance = Vector3.Distance(grabbableObject.transform.position,handToTrackMovement.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }

    private void InteractionEnabler(GameObject grabbableObject)
    {
        isFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        FingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

        // finger pinch down

        float h2o_distance = DistanceCalculator(grabbableObject);
        float h2h_distance = DistanceCalculator(theOtherhandToTrackMovement);
        bool ActualTouchMode;
        grandparent = grabbableObject.transform.parent.gameObject.transform.parent.gameObject;
        
        if (flag.IsVirtualGrasp == true){
            ActualTouchMode = false;
        } else {
            ActualTouchMode = IsTouchMode;
        }
        if (!IsBimanual) //user only emulate and operate with only one hand
        {
            if (isFishingRod){
                // rotate the object
                //grabbableObject.transform.LookAt(handToTrackMovement.transform.position);   
                grandparent.transform.LookAt(theOtherhandToTrackMovement.transform.position);
                //rotation restriction
                grandparent.transform.localEulerAngles = new Vector3(0,grandparent.transform.localEulerAngles.y,0);
            }
            else if (IsJoystick)
            {
                // rotate the parent of parent of the object (e.g. joystick base & joystick top)
                //grabbableObject.transform.parent.gameObject.transform.parent.gameObject.transform.LookAt(handToTrackMovement.transform.position);
                grandparent.transform.LookAt(theOtherhandToTrackMovement.transform.position);
            }
        }
        else if(IsBimanual && !ActualTouchMode)//IsBimanual = true && IsTouchMode == false
        {
            //if (isFingerPinching && FingerPinchStrength >= minFingerPinchDownStrength && h2o_distance < distanceThreshold )
            if (FingerPinchStrength >= minFingerPinchDownStrength && h2o_distance < distanceThreshold )
            {
                
                if (isFishingRod){
                    // rotate the object
                    //grabbableObject.transform.LookAt(handToTrackMovement.transform.position);   
                    grandparent.transform.LookAt(handToTrackMovement.transform.position);
                    //rotation restriction
                    grandparent.transform.localEulerAngles = new Vector3(0,grandparent.transform.localEulerAngles.y,0);
                }
                else if (IsJoystick)
                {
                    // rotate the parent of parent of the object (e.g. joystick base & joystick top)
                    //grabbableObject.transform.parent.gameObject.transform.parent.gameObject.transform.LookAt(handToTrackMovement.transform.position);
                    grandparent.transform.LookAt(handToTrackMovement.transform.position);
                }
                else
                {
                    // move/grab the object
                    grabbableObject.transform.position = Vector3.Lerp(grabbableObject.transform.position, handToTrackMovement.transform.position, 1f);
                }
                
                IsPinchingReleased = true;
                return;
            }
            
            // finger pinch up
            if (IsPinchingReleased)
            {
                IsPinchingReleased = false;
            }

        } else if(IsBimanual && ActualTouchMode)//IsBimanual = true && IsTouchMode == true
        {
            //if (isFingerPinching && FingerPinchStrength >= minFingerPinchDownStrength && h2h_distance < distanceThreshold )
            if (FingerPinchStrength >= minFingerPinchDownStrength && h2h_distance < distanceThreshold )
            {
                //Debug.Log("joystick HI detected");
                if (isFishingRod){
                    // rotate the object
                    //grabbableObject.transform.LookAt(handToTrackMovement.transform.position);   
                    grandparent.transform.LookAt(theOtherhandToTrackMovement.transform.position);
                    //rotation restriction
                    grandparent.transform.localEulerAngles = new Vector3(0,grandparent.transform.localEulerAngles.y,0);
                }
                else if (IsJoystick)
                {
                    // rotate the parent of parent of the object (e.g. joystick base & joystick top)
                    //grabbableObject.transform.parent.gameObject.transform.parent.gameObject.transform.LookAt(handToTrackMovement.transform.position);
                    grandparent.transform.LookAt(theOtherhandToTrackMovement.transform.position);
                }
                
                IsPinchingReleased = true;
                return;
            }
            // finger pinch up
            if (IsPinchingReleased)
            {
                IsPinchingReleased = false;
            }
        }


    }
}