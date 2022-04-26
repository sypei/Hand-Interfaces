using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class VirtualTrigger : MonoBehaviour
{   
    public StateFlag flag = new StateFlag();

    [SerializeField]
    private float minFingerPinchDownStrength = 0.5f;

    [SerializeField]
    private GameObject grabbableObject;
    private GameObject grandparent;

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

    private bool IsPinchingReleased = false;

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
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                .SingleOrDefault();
        
        theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                .SingleOrDefault();
        //VRLogInfo.Instance.LogInfo("boneToTrack is null: " + (boneToTrack == null));
    }

    void Start(){
        if (!flag.IsVirtualGrasp){
            
        }
    }
    void Update()
    {
        if (boneToTrack == null)
        {
            boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                .SingleOrDefault();
            if (boneToTrack != null)
                handToTrackMovement = boneToTrack.Transform.gameObject;
        }

        if (theOtherboneToTrack == null)
        {
            theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
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
        bool isFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Middle);

        float FingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);

        // finger pinch down

        //float h2o_distance = DistanceCalculator(grabbableObject);
        Vector3 closestPoint = grabbableObject.GetComponent<Collider>().ClosestPointOnBounds(handToTrackMovement.transform.position);
        float h2o_distance = Vector3.Distance(closestPoint, handToTrackMovement.transform.position);
        float h2h_distance = DistanceCalculator(theOtherhandToTrackMovement);
        grandparent = grabbableObject.transform.parent.gameObject.transform.parent.gameObject;
        
        if (flag.IsVirtualGrasp == true){
            if (isFingerPinching && FingerPinchStrength >= minFingerPinchDownStrength && h2o_distance < distanceThreshold )
            {
                grandparent.transform.LookAt(handToTrackMovement.transform.position);
                //rotation restriction
                grandparent.transform.localEulerAngles = new Vector3(grandparent.transform.localEulerAngles.x,0,0);
                //grandparent.transform.rotation = Quaternion.Euler(grandparent.transform.localEulerAngles.x,0,0);
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