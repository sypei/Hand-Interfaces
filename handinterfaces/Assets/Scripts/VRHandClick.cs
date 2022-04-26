using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using DilmerGames;//to enable VRDraw
public enum ClickWhich
{
    Sphere,//0
    Spray,//1
    Switch//2
}

public class VRHandClick : MonoBehaviour
{   
    [SerializeField]
    private StateFlag flag;
    [SerializeField]
    private GestureDetection GD;
    [SerializeField]
    private CallObjects CO;
    [SerializeField]
    private bool isPhantomG = false;
    [SerializeField]
    private HandToTrack handToTrack = HandToTrack.Right;
    [SerializeField]
    private ClickWhich clickWhich=ClickWhich.Sphere;
    [SerializeField]
    private GameObject clickableObject;
    //private GameObject grandparent;
    [SerializeField]
    private VRHandDraw VD;
    [SerializeField]
    private GameObject sphereTriggeredMap;
    private bool isMapActive = false;
    [SerializeField]
    private GameObject switchTriggeredLight;
    private bool isLightOn = true;
    
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
    [SerializeField]
    private Material defaultLineMaterial;
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
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                .SingleOrDefault();
        //VRLogInfo.Instance.LogInfo("boneToTrack is null: " + (boneToTrack == null));
        VD.AddNewLineRenderer(clickableObject);
        
    }

    void Start(){
        //sphere
        MapRendering(sphereTriggeredMap, false);
        //switch
        switchTriggeredLight.GetComponent<Light>().enabled = true;
        //spray
        
    }

    void MapRendering(GameObject parent, bool isEnabled){
        parent.SetActive(isEnabled);
        //parent.GetComponent<Renderer>().enabled = isEnabled;
        //parent.GetComponentsInChildren<Canvas>().enabled = isEnabled;
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
        //Debug.Log("distance is " + distance);
        
        return distance;
    }

    private void InteractionEnabler(GameObject clickableObject)
    {
        float h2o_distance = DistanceCalculator(clickableObject,handToTrackMovement); // right hand to object (switch/sphere's virtualgrasp)
        float h2o_distance2 = DistanceCalculator(clickableObject,theOtherhandToTrackMovement); // left hand to object (spray's virtualgrasp)
        float h2h_distance = DistanceCalculator(theOtherhandToTrackMovement,handToTrackMovement); //hand to hand (hand interface)
        //grandparent = clickableObject.transform.parent.gameObject.transform.parent.gameObject;
        if (flag.IsGD){
            currentInterface = GD.Recognize().name;
        } else {
            currentInterface = CO.currentInterface;
        }
        
        if (flag.IsBaselineInteraction && clickWhich==ClickWhich.Spray && currentInterface=="Spray")//baseline interaction's spray is more like hand interface
        {
            if (h2o_distance < distanceThreshold){
                //spray smoke rendering
                //spray sound
                //Debug.Log("touch is detected");
                VD.UpdateLine(clickableObject);
                //enable VR draw rendering, with spray tip rather than pinched fingertip
            }
            else {
                VD.AddNewLineRenderer(clickableObject);
                //Debug.Log("touch is not detected");
                //disable VR draw rendering
            }
        }
        else if(!flag.IsVirtualGrasp&&!isPhantomG)//hand interface - h2h
        {
            if (clickWhich==ClickWhich.Sphere && currentInterface=="Sphere")
            {
                //isMapActive = sphereTriggeredMap.activeInHierarchy;
                currentState = (h2h_distance > distanceThreshold); //false if in the proximity
                if (currentState && !lastState){
                    MapRendering(sphereTriggeredMap,!isMapActive);
                    isMapActive = !isMapActive;
                }                
                lastState = currentState;                
                
            }
            if (clickWhich==ClickWhich.Spray && currentInterface=="Spray")
            {
                //Debug.Log("spray"+currentInterface);
                if (h2h_distance < distanceThreshold){
                    //spray smoke rendering
                    //spray sound
                    //Debug.Log("touch is detected");
                    VD.UpdateLine(clickableObject);
                    //enable VR draw rendering, with spray tip rather than pinched fingertip
                }
                else {
                    VD.AddNewLineRenderer(clickableObject);
                    //Debug.Log("touch is not detected");
                    //disable VR draw rendering
                }
            }
            if (clickWhich==ClickWhich.Switch&&currentInterface=="Switch")//H2H
            {
                Vector3 switchOn = new Vector3(0, 0, 15f);
                Vector3 switchOff = new Vector3(0, 0, 345f);//don't use -15, use positive values
                currentState = (h2h_distance > distanceThreshold); //false if in the proximity
                Debug.Log("islighton:"+isLightOn);
                if (currentState && !lastState){
                    switchTriggeredLight.GetComponent<Light>().enabled = !isLightOn;
                    isLightOn=!isLightOn;
                    if (!isLightOn){
                        //Debug.Log("Angles"+clickableObject.transform.localEulerAngles);
                        clickableObject.transform.localEulerAngles = switchOff;
                        //switch sound
                        //button change rendering
                    } else {
                        clickableObject.transform.localRotation = Quaternion.Euler(switchOn);
                    }
                }
                lastState = currentState;
            }
        } else if(flag.IsVirtualGrasp&&isPhantomG){//Virtual Grasp
            if (clickWhich==ClickWhich.Sphere && currentInterface=="Sphere")
            {
                //isMapActive = sphereTriggeredMap.activeInHierarchy;
                currentState = (h2o_distance > 0.1f); //false if in the proximity
                if (currentState && !lastState){
                    MapRendering(sphereTriggeredMap,!isMapActive);
                    isMapActive = !isMapActive;
                }                
                lastState = currentState;                
                
            }
            if (clickWhich==ClickWhich.Spray&&currentInterface=="Spray")
            {
                //Debug.Log(h2o_distance2);
                if (h2o_distance2 < 0.03f){
                    //spray smoke rendering
                    //spray sound
                    //Debug.Log("click is detected:"+h2o_distance2);
                    VD.UpdateLine(clickableObject);
                    //enable VR draw rendering, with spray tip rather than pinched fingertip
                }
                else {
                    VD.AddNewLineRenderer(clickableObject);
                    //Debug.Log("click is not detected"+h2o_distance2);
                    //disable VR draw rendering
                }
            }
            if (clickWhich==ClickWhich.Switch&&currentInterface=="Switch")
            {
                Vector3 switchOn = new Vector3(0, 0, 15f);
                Vector3 switchOff = new Vector3(0, 0, 345f);
                currentState = (h2o_distance > distanceThreshold); //false if in the proximity
                Debug.Log("islighton:"+isLightOn);
                if (currentState && !lastState){
                    switchTriggeredLight.GetComponent<Light>().enabled = !isLightOn;
                    isLightOn=!isLightOn;
                    if (!isLightOn){
                        Debug.Log("Angles"+clickableObject.transform.localEulerAngles);
                        clickableObject.transform.localEulerAngles = switchOff;
                        //switch sound
                        //button change rendering
                    } else {
                        clickableObject.transform.localRotation = Quaternion.Euler(switchOn);
                    }
                }
                lastState = currentState;
            }
         
        }


    }
}