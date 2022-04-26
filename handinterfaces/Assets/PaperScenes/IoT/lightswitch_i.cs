using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class lightswitch_i : MonoBehaviour
{   

    [SerializeField]
    private GestureRetrieval_IoT GD;
    
    [SerializeField]
    private GameObject clickableObject;
    //private GameObject grandparent;
    public bool isLightOn;
    
    [SerializeField]
    private GameObject handToTrackMovement;
    [SerializeField]
    private GameObject theOtherhandToTrackMovement;

    [SerializeField]
    private float distanceThreshold = 0.05f; //max distance to activate grabbing
    private bool lastState = true;
    private bool currentState = true;
    private string currentInterface;

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
        
        //clickableObject = GameObject.Find("JoystickTop");
        //handToTrackMovement = GameObject.Find("OVRHandPrefabLeft");
        //theOtherhandToTrackMovement = GameObject.Find("OVRHandPrefabRight");

        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
        
        theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
        
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
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
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
            if (boneToTrack != null)
                theOtherhandToTrackMovement = theOtherboneToTrack.Transform.gameObject;
        }
        InteractionEnabler(clickableObject);
    }

    private float DistanceCalculator(GameObject clickableObject,GameObject handToTrackMovement)
    {
        float distance = 0;     
        distance = Vector3.Distance(clickableObject.transform.position,handToTrackMovement.transform.position);     
        return distance;
    }

    private void InteractionEnabler(GameObject clickableObject)
    {
        float h2h_distance = DistanceCalculator(theOtherhandToTrackMovement,handToTrackMovement); //hand to hand (hand interface)
        //grandparent = clickableObject.transform.parent.gameObject.transform.parent.gameObject;

        currentInterface = GD.currentGesture_stable.name;

        if (currentInterface=="lightswitch")
        {
            Vector3 OnSwitch = new Vector3(0, 0, 15f);
            Vector3 OffSwitch = new Vector3(0, 0, 345f);//don't use -15, use positive values
            currentState = (h2h_distance > distanceThreshold); //false if in the proximity
            Debug.Log("islighton:"+isLightOn);
            if (currentState && !lastState){
                isLightOn=!isLightOn;
                if (isLightOn){
                    //Debug.Log("Angles"+clickableObject.transform.localEulerAngles);
                    clickableObject.transform.localEulerAngles = OffSwitch;
                } else {
                    clickableObject.transform.localRotation = Quaternion.Euler(OnSwitch);
                }
            }
            lastState = currentState;
        }
        
    }
}