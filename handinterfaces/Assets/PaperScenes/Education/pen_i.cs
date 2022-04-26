using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames;
using System.Linq;

public class pen_i : MonoBehaviour
{
    [SerializeField]
    private GestureRetrieval_Education GD;
    [SerializeField]
    private VRHandDraw VD;
    [SerializeField]
    private GameObject handToTrackMovement;
    // [SerializeField]
    // private GameObject theOtherhandToTrackMovement;
    private GameObject trackpoint;
    private GameObject trackpoint2;
    private GameObject trackpoint3;
    [SerializeField]
    private GameObject pentip;
    private string currentInterface;
    [SerializeField]
    private float distanceThreshold = 0.02f; 

#region Oculus Types

    private OVRHand ovrHand;

    private OVRSkeleton ovrSkeleton;
    private OVRSkeleton theOtherovrSkeleton;

    private OVRBone boneToTrack;
    private OVRBone boneToTrack2;
    private OVRBone boneToTrack3;
#endregion
    void Awake() 
    {
        ovrHand = handToTrackMovement.GetComponent<OVRHand>();
        ovrSkeleton = handToTrackMovement.GetComponent<OVRSkeleton>();
        // theOtherovrSkeleton = theOtherhandToTrackMovement.GetComponent<OVRSkeleton>();
        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                .SingleOrDefault();
        
        boneToTrack2 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();

        boneToTrack3 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
                .SingleOrDefault();
    }


    // Start is called before the first frame update
    void Start()
    {
        // pentip = GameObject.Find("pentip");
        VD.AddNewLineRenderer(pentip);
    }

    // Update is called once per frame
    void Update()
    {
        if (boneToTrack == null)
        {
            boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                .SingleOrDefault();
            if (boneToTrack != null)
                trackpoint = boneToTrack.Transform.gameObject;
        }
        if (boneToTrack2 == null)
        {
            boneToTrack2 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
            if (boneToTrack2 != null)
                trackpoint2 = boneToTrack2.Transform.gameObject;
        }
        if (boneToTrack3 == null)
        {
            boneToTrack3 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
                .SingleOrDefault();
            if (boneToTrack3 != null)
                trackpoint3 = boneToTrack3.Transform.gameObject;
        }
        PenInteractionEnabler();
    }

    private float DistanceCalculator(GameObject pentip,GameObject trackpoint)
    {
        float distance = 0;
        
        distance = Vector3.Distance(pentip.transform.position,trackpoint.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }

    private void PenInteractionEnabler(){
        currentInterface = GD.currentGesture_stable.name;
        float h2o_distance = DistanceCalculator(pentip,trackpoint);
        if (currentInterface=="pen"){
            if (h2o_distance < distanceThreshold){
                VD.UpdateLine(pentip);
            }
            else {
                VD.AddNewLineRenderer(pentip);
            }
        }
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    } 
}
