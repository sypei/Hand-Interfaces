using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GestureRetrieval_IoT : MonoBehaviour
{
    public float threshold = 0.02f;// too small, will find nothing (too strict)
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    public bool writeLogger = true;
    public TextMeshPro ModeLogger;
    public TextMeshPro GestureLogger;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    public Gesture currentGesture_stable;
    private float startTime = 0f;
    private float timer = 0f;
    public float holdTime = 0.2f;
    private bool thereAreBones = false;
    private GameObject hand;
    //Hand Interface
    private GameObject joystick;
    private GameObject colorpalette;
    private GameObject lightswitch;
    private Dictionary<string, GameObject> gesturedict;
    private List<float> ss_joystick =  new List<float>{1,1,1,1,0.2f,//0-5
                                                    1,1,1,1,1,//6-10
                                                    1,1,1,1,1,//11-15
                                                    1,1,1,0.1f,1,//16-20
                                                    1,1,1,1};//21-24
    private List<float> ss_colorpalette =  new List<float>{1,1,1,1,1,//0-5
                                                    1,1,1,1,1,//6-10
                                                    1,1,1,1,1,//11-15
                                                    1,1,1,2,2,//16-20
                                                    2,2,2,1};//21-24
    private List<float> ss_lightswitch =  new List<float>{1,1,1,1,1,//0-5
                                                    1,2,2,1,1,//6-10
                                                    1,1,1,1,1,//11-15
                                                    1,1,1,1,1,//16-20
                                                    1,1,1,1};//21-24
    private Dictionary<string, List<float>> sensitivitydict;
    //This error: NullReferenceException: Object reference not set to an instance of an object
    // is because I did not drag the hand prefab to "skeleton" in inspector interface


    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
        currentGesture_stable = new Gesture();
        hand = GameObject.FindGameObjectsWithTag("lefthand")[0];
        //Hand Interface
        
        joystick = GameObject.Find("joystick");
        colorpalette = GameObject.Find("colorpalette");
        lightswitch = GameObject.Find("lightswitch");

        sensitivitydict = new Dictionary<string, List<float>>()
        {
            {"joystick", ss_joystick},
            {"colorpalette", ss_colorpalette},
            {"lightswitch", ss_lightswitch}
        };

        gesturedict = new Dictionary<string, GameObject>()
        {
            {"joystick", joystick},
            {"colorpalette", colorpalette},
            {"lightswitch", lightswitch}
        };
        foreach(var ges in gesturedict)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        hand.GetComponent<Renderer>().enabled = true;
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
             if(debugMode && Input.GetKeyDown(KeyCode.Space))
            {
                Save();
            }

            Gesture currentGesture = Recognize();

            if (currentGesture.name != null){
                if (currentGesture.name != previousGesture.name){
                    startTime = Time.time;
                    timer = startTime;
                    // Debug.Log("Start to count for this gesture: "+currentGesture.name);
                } else if (currentGesture.name == previousGesture.name){
                    timer += Time.deltaTime;
                    // Debug.Log("Time: "+timer);
                    if (timer > (startTime + holdTime))
                    {
                        currentGesture_stable = currentGesture;
                        // Debug.Log("get stable gesture: "+currentGesture_stable.name);
                    }
                }
                previousGesture = currentGesture;
            }

            if (Input.GetKeyDown("s")){
                currentGesture_stable.name = "";
            }
            
            //Debug.Log("Current Gesture:"+currentGesture.name);
            //GestureLogger.SetText("Current Gesture:{0}",currentGesture.name);
            
            HandInterfaceRendering(currentGesture_stable);
            if (writeLogger)
            {
                ModeLogger.text="Hand Interface - IoT";
                GestureLogger.text="Current Gesture: "+currentGesture_stable.name;
            }
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

    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            //finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerDatas = data;
        gestures.Add(g);
    }

    public Gesture Recognize()
    {
        Gesture currentgesture = new Gesture();
        float currentMin = Mathf.Infinity;
        
        foreach (var gesture in gestures)
        {
            string assumption = gesture.name;
            // Debug.Log("name is "+gesture.name);
            float sumDistance = 0;
            bool isDiscarded = false;
            // List<float> ss = sensitivitydict[assumption];
            
            for (int i = 0; i < fingerBones.Count; i++)
            {
                // Debug.Log("scanning finger bone ID : "+i);
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData,gesture.fingerDatas[i]);
                //Debug.Log("distance of finger "+i+" is " +distance+", threshold is "+threshold);
                // distance = distance*ss[i];
                if (distance>threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentgesture = gesture;
            }
        }
        //Debug.Log("Current Gesture:"+currentgesture.name);
        return currentgesture;
    }


    // If we use gameobject.GetComponent<Renderer>().enabled = false;
    // Once the gameobject is no longer active the code will no longer run in the update. (only run once even it is put in update())
    // Therefore we should use
    void HandInterfaceRendering(Gesture currentgesture)
    {
        string caseSwitch = currentgesture.name;

        foreach(var ges in gesturedict)
            ChildrenRendering(ges.Value, false);
        //hand.GetComponent<Renderer>().enabled = true;
        if(caseSwitch!=null)
        {
            if (gesturedict.ContainsKey(caseSwitch)){
                ChildrenRendering(gesturedict[caseSwitch], true);
            }
        }
        //Debug.Log("test if word is in the list" + gesturedictlist.ContainsKey("BilliardCue"));        
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    }
 
}