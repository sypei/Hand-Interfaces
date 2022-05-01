using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FingerKeys_Demo : MonoBehaviour
{
    [SerializeField]
    private GestureDetection_Demo GD;
    public OVRSkeleton skeleton;
    public bool debugMode = true;
    private List<OVRBone> fingerBones;
    private bool thereAreBones = false;
    private GameObject lefthand;
    [SerializeField]
    private GameObject index_key;
    [SerializeField]
    private GameObject middle_key;
    [SerializeField]
    private GameObject ring_key;
    [SerializeField]
    private GameObject pinky_key;
    //This error: NullReferenceException: Object reference not set to an instance of an object
    // is because I did not drag the hand prefab to "skeleton" in inspector interface


    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        lefthand = GameObject.FindGameObjectsWithTag("lefthand")[0];
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!thereAreBones){
            FindBones();
        }

        if (thereAreBones)
        {
            fingerBones= new List<OVRBone>(skeleton.Bones);//added
            //should not put it in save, 
            //or fingerBones will have nth when you don't press space
            FingerKeysRendering();

        }
    }
    void FindBones()
    {
        //if (new List<OVRBone>(skeleton.Bones).Count > 0)
        if (skeleton.Bones.Count > 0)
        {
            //fingerBones= new List<OVRBone>(skeleton.Bones);//added
            thereAreBones = true;
        }
    }


    // If we use gameobject.GetComponent<Renderer>().enabled = false;
    // Once the gameobject is no longer active the code will no longer run in the update. (only run once even it is put in update())
    // Therefore we should use
    void FingerKeysRendering()
    {
        string currentInterface;
        
        currentInterface = GD.Recognize().name;
        
        
        bool isEnabled = currentInterface=="ThumbPiano";
        //determine if keys are visible
        ChildrenRendering(index_key, isEnabled);
        ChildrenRendering(middle_key, isEnabled);
        ChildrenRendering(ring_key, isEnabled);
        ChildrenRendering(pinky_key, isEnabled);
        //determine the position and direction of keys
        index_key.transform.position = fingerBones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position; //start point
        index_key.transform.LookAt(fingerBones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position); //point to end point
        middle_key.transform.position = fingerBones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.position; //start point
        middle_key.transform.LookAt(fingerBones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform.position); //point to end point
        ring_key.transform.position = fingerBones[(int)OVRSkeleton.BoneId.Hand_Ring1].Transform.position; //start point
        ring_key.transform.LookAt(fingerBones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform.position); //point to end point        
        pinky_key.transform.position = fingerBones[(int)OVRSkeleton.BoneId.Hand_Pinky1].Transform.position; //start point
        pinky_key.transform.LookAt(fingerBones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform.position); //point to end point
    }
    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    }
}