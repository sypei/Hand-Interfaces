using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VirtualBalloon : MonoBehaviour
{
    private bool inflateMe = false;
    public StateFlag flag;
    public GameObject balloon;
    public CallObjects CO;
    public GestureDetection GD;
    public float fillSpeed=10f;
    private float deflateSpeed = 0.1f;
    private float maxSize = 3.3f;
    private Vector3 airAmountMin = new Vector3(0.1f,0.5f,0.4f);
    private Vector3 airAmountMax = new Vector3(2f,2f,2f);
    private bool currentState = true;
    private bool lastState = true;
    private float i2t_distance;
    private float h2o_distance;
    private string currentInterface;
    public GameObject inflator;
    private Vector3 beforeSqueeze;
    private Vector3 afterSqueeze;
    [SerializeField]
    float squeezeSpeed = 2f;

    [SerializeField]
    private GameObject handToTrackMovement;
    [SerializeField]
    private GameObject theOtherhandToTrackMovement;
    private bool IsProximity = false;
    // Start is called before the first frame update

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

        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
        
        theOtherboneToTrack = theOtherovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
        //VRLogInfo.Instance.LogInfo("boneToTrack is null: " + (boneToTrack == null));
        
    }
    void Start()
    {
        //disable balloon first
        //disable audio first
        beforeSqueeze = inflator.transform.localScale;
        if (flag.IsBaselineInteraction)//baseline
            afterSqueeze = new Vector3(inflator.transform.localScale.x,inflator.transform.localScale.y*0.2f,inflator.transform.localScale.z);
        else if (flag.IsVirtualGrasp)//VG
            afterSqueeze = new Vector3(inflator.transform.localScale.x,inflator.transform.localScale.y*0.2f,inflator.transform.localScale.z);
        else//HI
            afterSqueeze = new Vector3(inflator.transform.localScale.x,inflator.transform.localScale.y*0.4f,inflator.transform.localScale.z);
        //Debug.Log("before local scale"+inflator.transform.localScale);
    }

    // Update is called once per frame
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
        BalloonInteractionEnabler();        
    }

    private void BalloonInteractionEnabler()
    {
        i2t_distance = DistanceCalculator(handToTrackMovement,theOtherhandToTrackMovement);//index to thumb (right hand)
        h2o_distance = DistanceCalculator(handToTrackMovement,inflator);
        currentState = (i2t_distance > 0.05f);
        //Debug.Log("h2o distance"+h2o_distance);
        if (flag.IsBaselineInteraction)//baseline
        {
            IsProximity = (h2o_distance < 0.12f);
        }
        else if (flag.IsVirtualGrasp)//VG
        {
            IsProximity = (h2o_distance < 0.11f);
        }
        else //HI
        {
            IsProximity = (h2o_distance < 0.05f);
        }
        
        inflateMe = !lastState&&currentState&&IsProximity;
        //Debug.Log("isclosed?"+IsProximity);

        if (flag.IsGD){
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }

        if (currentInterface=="Inflator"){
        //if(true){
            if (inflateMe){
            //if(true){
                balloon.transform.localScale = Vector3.Lerp(balloon.transform.localScale, airAmountMax, fillSpeed*Time.deltaTime);
                //enable audio and play sound
                if(balloon.transform.localScale.magnitude>3.2){
                    //Debug.Log(balloon.transform.localScale.magnitude+"magnitude");
                    balloon.GetComponent<ParticleSystem>().Play();
                    balloon.transform.localScale = airAmountMin;
                    //play explode sound
                    //balloon.SetActive(false);
                }
            } else {
                if (balloon.transform.localScale.magnitude<2)
                    balloon.transform.localScale = Vector3.Lerp(balloon.transform.localScale, airAmountMin, deflateSpeed*Time.deltaTime);
                //disable audio source
            }
            // for inflator itself
            if (IsProximity)
            {
                if(i2t_distance < 0.035f){
                    inflator.transform.localScale = Vector3.Lerp(inflator.transform.localScale, afterSqueeze, squeezeSpeed*Time.deltaTime);
                } else {
                    inflator.transform.localScale = Vector3.Lerp(inflator.transform.localScale, beforeSqueeze, squeezeSpeed*Time.deltaTime);
                }
            }
            
        }
        lastState = currentState;
    }
    private float DistanceCalculator(GameObject clickableObject,GameObject handToTrackMovement)
    {
        float distance = 0;
        
        distance = Vector3.Distance(clickableObject.transform.position,handToTrackMovement.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }
}
