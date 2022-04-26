using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VirtualScissors : MonoBehaviour
{
    //define states
    [SerializeField]
    private StateFlag flag;
    [SerializeField]
    private GestureDetection GD;
    [SerializeField]
    private CallObjects CO;
    [SerializeField]
    private GameObject blade_1;
    [SerializeField]
    private GameObject blade_2;
    
    [SerializeField]
    private bool isPhantomVG = false;
    [SerializeField]
    private float HIdistanceThreshold = 0.05f;
    [SerializeField]
    private float VGdistanceThreshold = 0.05f;
    [SerializeField]
    private float BaselinedistanceThreshold = 0.05f;
    private float distanceThreshold;
    private bool lastState = true;
    private bool currentState = true;
    private string currentInterface;
    //define index3 and middle3 fingers
    [SerializeField]
    private GameObject finger_1;
    [SerializeField]
    private GameObject finger_2;
    [HideInInspector]
    public bool IsCutting = false;

#region Oculus Types

    private OVRHand ovrHand;

    private OVRSkeleton ovrSkeleton;
    private OVRSkeleton ovrSkeleton_2;

    private OVRBone boneToTrack;
    private OVRBone boneToTrack_2;
    private Vector3 closePlace;
#endregion
    
    // Start is called before the first frame update
    void Awake() 
    {
        ovrHand = finger_1.GetComponent<OVRHand>();
        ovrSkeleton = finger_1.GetComponent<OVRSkeleton>();
        ovrSkeleton_2 = finger_2.GetComponent<OVRSkeleton>();

        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();

        if (flag.IsVirtualGrasp){
            boneToTrack_2 = ovrSkeleton_2.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                    .SingleOrDefault();
        }
        if (flag.IsHandInterface){
            boneToTrack_2 = ovrSkeleton_2.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                    .SingleOrDefault();
        }
        
        
        
    }
    void Start()
    {
        if(flag.IsHandInterface)
            distanceThreshold = HIdistanceThreshold;
        if(flag.IsVirtualGrasp)
            distanceThreshold = VGdistanceThreshold;
        if(flag.IsBaselineInteraction)
            distanceThreshold = BaselinedistanceThreshold;
        closePlace = blade_1.transform.localEulerAngles;
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
                finger_1 = boneToTrack.Transform.gameObject;
        }

        if (boneToTrack_2 == null)
        {
            if (flag.IsHandInterface){
                boneToTrack_2 = ovrSkeleton_2.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3)
                    .SingleOrDefault();
                if (boneToTrack != null)
                    finger_2 = boneToTrack_2.Transform.gameObject;
            }
            if (flag.IsVirtualGrasp){
                boneToTrack_2 = ovrSkeleton_2.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                    .SingleOrDefault();
                if (boneToTrack != null)
                    finger_2 = boneToTrack_2.Transform.gameObject;
            }
        }
        if (flag.IsInteraction)
            ScissorsInteractionEnabler();
            ScissorsAnimation();        
    }

    private float DistanceCalculator(GameObject finger_2,GameObject finger_1)
    {
        float distance = 0;
        
        distance = Vector3.Distance(finger_2.transform.position,finger_1.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }

    (GameObject,float) GetClosestObject(GameObject[] cuttables)
    {
        GameObject cMin = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject cuttable in cuttables)
        {
            float dist = DistanceCalculator(cuttable, finger_1);
            if (dist < minDist)
            {
                cMin = cuttable;
                minDist = dist;
            }
        }
        return (cMin,minDist);
    }

    private void ScissorsInteractionEnabler()
    {
        float h2h_distance = DistanceCalculator(finger_2,finger_1); //hand to hand (hand interface)
        GameObject[] cuttables = FindGameObjectsInLayer(3);//lay3 is cuttable layer
        (GameObject cuttable, float h2o_distance) = GetClosestObject(cuttables);
        //Debug.Log(h2h_distance+"scissors distance");
        if (flag.IsGD){
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }
        if(flag.IsHandInterface&&!isPhantomVG)//hand interface - h2h
        {
            if (currentInterface=="Scissors")
            {
                currentState = (h2h_distance > distanceThreshold); //false if in the proximity
                IsCutting = currentState && !lastState;            
                lastState = currentState;                
                
            }
        } else if(flag.IsVirtualGrasp&&isPhantomVG&&!flag.IsBaselineInteraction){//Virtual Grasp
            if (currentInterface=="Scissors"||currentInterface=="Scissorsg")
            {
                currentState = (h2h_distance > distanceThreshold); //false if in the proximity
                IsCutting = currentState && !lastState;               
                lastState = currentState;
            }      
        } else if(flag.IsBaselineInteraction){
            if (currentInterface=="Scissors"||currentInterface=="Scissorsg")
            {
                currentState = (h2o_distance > distanceThreshold); //false if in the proximity
                IsCutting = currentState && !lastState;            
                lastState = currentState;                
                //Debug.Log(h2o_distance+"h2o_distance");
            }
        }
    }

    GameObject[] FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == 3)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    private void ScissorsAnimation()
    {
        if (currentState){//open
            blade_1.transform.localEulerAngles = new Vector3(closePlace.x,closePlace.y+30f,closePlace.z);
            blade_2.transform.localEulerAngles = new Vector3(closePlace.x,closePlace.y-30f,closePlace.z);
            //blade_1.transform.rotation = Quaternion.Euler(0,blade_1.transform.localEulerAngles.y+30f,0);
            //blade_2.transform.rotation = Quaternion.Euler(0,blade_2.transform.localEulerAngles.y-30f,0);
        } else {//close
            blade_1.transform.localEulerAngles = closePlace;
            blade_2.transform.localEulerAngles = closePlace;
        }
    }
}
