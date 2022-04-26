using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VirtualBinoculars : MonoBehaviour
{
    //public Camera m_Camera;
    public StateFlag flag;
    public GestureDetection GD;
    public CallObjects CO;
    [SerializeField]
    private GameObject BinocularsView;
    [SerializeField]
    private GameObject binocularsPhantom;
    [SerializeField]
    private GameObject handToTrackMovement;
    [SerializeField]
    private GameObject theOtherhandToTrackMovement;
    [SerializeField]
    private GameObject eye;
    [SerializeField]
    private float distanceThreshold = 0.1f; 
    [SerializeField]
    private float ViewDistanceThreshold = 0.4f; 
    [SerializeField]
    private GameObject editorhandToTrackMovement;

    [SerializeField]
    private bool allowEditorControls = true;
    float m_FieldOfView=90f;
    float m_FieldOfView_zoom=30f;
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
        
        #if UNITY_EDITOR
        
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
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
    }
    void Start()
    {
        //m_Camera.fieldOfView = m_FieldOfView;
        BinocularsView.SetActive(false);
    }

    // Update is called once per frame
    
    void Update()
    {
        if (boneToTrack == null)
        {
            boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
            if (boneToTrack != null){
                handToTrackMovement = boneToTrack.Transform.gameObject;
            }
        }
        if (theOtherboneToTrack == null)
        {
            theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
            if (boneToTrack != null)
                theOtherhandToTrackMovement = theOtherboneToTrack.Transform.gameObject;
        }
        BinocularsInteractionEnabler();
    }

    void BinocularsInteractionEnabler()
    {
        if (flag.IsGD){//demo means there is gesture recognition
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }
        float h2h_distance = DistanceCalculator(theOtherhandToTrackMovement,handToTrackMovement);//hand to object
        float o2e_distance = DistanceCalculator(eye,binocularsPhantom);//hand to eye
        //Debug.Log("eye distance"+h2e_distance);
        
        //enable rendering
        if(flag.IsGD){//the HI, VG detection scenes + demo scene 
            //Debug.Log("h2h distance"+h2h_distance);
            if(h2h_distance < distanceThreshold && (currentInterface=="Binocularsg"||currentInterface=="Binoculars")){
                ChildrenRendering(binocularsPhantom, true);

            } else {
                ChildrenRendering(binocularsPhantom,false);
            }
        } else {//the 3 interaction scenes
            if (currentInterface=="Binoculars"){
                ChildrenRendering(binocularsPhantom, true);

            } else {
                ChildrenRendering(binocularsPhantom,false);
            }
        }

        //enable zoom-in effect
        if (flag.IsInteraction){
            if (o2e_distance < ViewDistanceThreshold && currentInterface=="Binoculars"){
            //if (true){    
                //m_Camera.fieldOfView = m_FieldOfView_zoom;
                BinocularsView.SetActive(true);

            } else {
                //m_Camera.fieldOfView = m_FieldOfView;
                BinocularsView.SetActive(false);
            }
        }
        
        
    }

    private float DistanceCalculator(GameObject target, GameObject handToTrackMovement)
    {
        float distance = 0;
        
        distance = Vector3.Distance(target.transform.position,handToTrackMovement.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }
    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    } 

}
