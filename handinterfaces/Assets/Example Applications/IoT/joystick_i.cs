using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class joystick_i : MonoBehaviour
{   
    private TCPClient TCPClient;
    [SerializeField]
    private GameObject joystick_top;//joystick top
    [SerializeField]
    private GameObject joystickbase;
    [SerializeField]
    private GestureRetrieval_IoT GD;
    
    [SerializeField]
    private GameObject handToTrackMovement;
    [SerializeField]
    private GameObject theOtherhandToTrackMovement;

    [SerializeField]
    private float distanceThreshold = 0.05f; //max distance to activate grabbing

    [SerializeField]
    public float minFingerPinchDownStrength = 0.5f;
    public bool IsPinchingReleased = false;
    public float FingerPinchStrength;
    private Vector3 basePos;
    private Vector3 topPos;
    public Vector3 diff = new Vector3(0f,0f,0f);
    public float pan = 90;
    private float panstep;
    public float tilt = 90;
    private float tiltstep;
    private float step = 0.5f;

#region Oculus Types

    private OVRHand ovrHand;

    private OVRSkeleton ovrSkeleton;
    private OVRSkeleton theOtherovrSkeleton;

    private OVRBone boneToTrack;
    private OVRBone theOtherboneToTrack;
    private GameObject trackpoint;
    private GameObject trackpoint2;
#endregion

    void Awake() 
    {
        ovrHand = handToTrackMovement.GetComponent<OVRHand>();
        ovrSkeleton = handToTrackMovement.GetComponent<OVRSkeleton>();
        theOtherovrSkeleton = theOtherhandToTrackMovement.GetComponent<OVRSkeleton>();
        
        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)//interacting hand
                .SingleOrDefault();
        
        theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)//emulating hand
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
                trackpoint = boneToTrack.Transform.gameObject;
        }

        if (theOtherboneToTrack == null)
        {
            theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
            if (boneToTrack != null)
                trackpoint2 = theOtherboneToTrack.Transform.gameObject;
        }
        if (GD.currentGesture_stable.name == "joystick")
        {
            InteractionEnabler();//HI interaction
        }
        
    }

    private float DistanceCalculator(GameObject trackpoint, GameObject trackpoint2)
    {
        float distance = 0;
        distance = Vector3.Distance(trackpoint.transform.position,trackpoint2.transform.position);
        return distance;
    }

    private void InteractionEnabler()
    {
        FingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

        // finger pinch down
        float h2h_distance = DistanceCalculator(trackpoint, trackpoint2);
        // joystickbase = joystick_top.transform.parent.gameObject.transform.parent.gameObject;
        
        if (FingerPinchStrength >= minFingerPinchDownStrength && h2h_distance < distanceThreshold )
        {
            joystickbase.transform.LookAt(trackpoint2.transform.position);
            JoystickCommandMaker();//create joystick commands
            IsPinchingReleased = true;
            return;
        }
        // finger pinch up
        if (IsPinchingReleased)
        {
            IsPinchingReleased = false;
        }
    }
    private void JoystickCommandMaker()
    {
        topPos = joystick_top.transform.position;
        basePos = joystickbase.transform.position;
        // Debug.Log("toppos"+topPos);
        diff = topPos - basePos;
        // Debug.Log("base"+basePos);
        // if (Mathf.Abs(diff.x) > 0.01 | Mathf.Abs(diff.z) > 0.01 )//The range is -0.02~0.02 when active, around 0.005 when non-active.
        // {
        //     // TCPClient.send(diff.x,diff.z);
        //     Debug.Log("x "+diff.x +" z" +diff.z);
        // }
        // Debug.Log("x "+diff.x +" y "+ diff.y+" z" +diff.z);
        if (diff.x > 0.01)
            panstep = step;
        else if (diff.x < -0.01)
            panstep = -step;
        else 
            panstep = 0;
        if (diff.z > 0.01)
            tiltstep = step;
        else if (diff.z < -0.01)
            tiltstep = -step;
        else 
            tiltstep = 0;
        if ((pan + panstep) >60 && (pan + panstep) < 120){
            pan += panstep;
        }
        if ((tilt + tiltstep) >60 && (tilt + tiltstep) < 120){
            tilt += tiltstep;
        }
    }
}