using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
//struc = class without function
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}
public class GestureDetection : MonoBehaviour
{
    public StateFlag flag;
    public float threshold = 0.02f;// too small, will find nothing (too strict)
    private float graspthreshold = 0.04f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public List<Gesture> gesturesgrasp;
    private List<Gesture> gestureslist;
    public bool debugMode = true;
    public TextMeshPro ModeLogger;
    public bool writeModeLogger = true;
    public TextMeshPro GestureLogger;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    private bool thereAreBones = false;
    private GameObject lefthand;
    //Hand Interface
    private GameObject joystick;
    private GameObject sphere;
    private GameObject fishing;
    private GameObject inflator;
    private GameObject trumpet;
    private GameObject billiardcue;
    private GameObject spray;
    private GameObject myswitch;
    private GameObject thumbpiano;
    private GameObject scissors;
    //private GameObject trigger;
    private GameObject binoculars;
    // Virtual Grasp
    private GameObject joystickg;
    private GameObject sphereg;
    private GameObject fishingg;

    private GameObject inflatorg;
    private GameObject trumpetg;
    private GameObject billiardcueg;
    private GameObject sprayg;
    private GameObject myswitchg;
    private GameObject thumbpianog;
    private GameObject scissorsg;
    //private GameObject triggerg;
    private GameObject binocularsg;
    private Dictionary<string, GameObject> gesturedict;
    private Dictionary<string, GameObject> gesturedictgrasp;
    private Dictionary<string, GameObject> gesturedictlist;
    //This error: NullReferenceException: Object reference not set to an instance of an object
    // is because I did not drag the hand prefab to "skeleton" in inspector interface


    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();        
        //why it never prints this log???
        //Debug.Log("Start there are bones fingerBones.Count is "+fingerBones.Count);
        lefthand = GameObject.FindGameObjectsWithTag("lefthand")[0];
        //Hand Interface
        joystick = GameObject.FindGameObjectsWithTag("joystick")[0];
        sphere = GameObject.FindGameObjectsWithTag("sphere")[0];
        fishing = GameObject.FindGameObjectsWithTag("fishing")[0];     
        inflator = GameObject.FindGameObjectsWithTag("inflator")[0];
        trumpet = GameObject.FindGameObjectsWithTag("trumpet")[0];
        billiardcue = GameObject.FindGameObjectsWithTag("billiardcue")[0];
        spray = GameObject.FindGameObjectsWithTag("spray")[0];
        myswitch = GameObject.FindGameObjectsWithTag("switch")[0];
        thumbpiano = GameObject.FindGameObjectsWithTag("thumbpiano")[0];
        scissors = GameObject.FindGameObjectsWithTag("scissors")[0];
        //trigger = GameObject.FindGameObjectsWithTag("trigger")[0];
        binoculars = GameObject.FindGameObjectsWithTag("binoculars")[0];
        //Virtual Grasp
        joystickg = GameObject.FindGameObjectsWithTag("joystick")[1];
        sphereg = GameObject.FindGameObjectsWithTag("sphere")[1];
        fishingg = GameObject.FindGameObjectsWithTag("fishing")[1];
        inflatorg = GameObject.FindGameObjectsWithTag("inflator")[1];
        trumpetg = GameObject.FindGameObjectsWithTag("trumpet")[1];
        billiardcueg = GameObject.FindGameObjectsWithTag("billiardcue")[1];
        sprayg = GameObject.FindGameObjectsWithTag("spray")[1];
        myswitchg = GameObject.FindGameObjectsWithTag("switch")[1];
        thumbpianog = GameObject.FindGameObjectsWithTag("thumbpiano")[1];
        scissorsg = GameObject.FindGameObjectsWithTag("scissors")[1];
        //triggerg = GameObject.FindGameObjectsWithTag("trigger")[1];
        binocularsg = GameObject.FindGameObjectsWithTag("binoculars")[1];
        

        gesturedict = new Dictionary<string, GameObject>()
        {
            {"Joystick", joystick},
            {"Sphere", sphere},
            {"Fishing", fishing},
            {"Wand", billiardcue},
            {"Switch", myswitch},
            {"Inflator", inflator},
            {"Trumpet", trumpet},
            {"Spray", spray},
            {"ThumbPiano", thumbpiano},
            {"Scissors", scissors},
            //{"Trigger", trigger},
            {"Binoculars", binoculars}
        };
        gesturedictgrasp = new Dictionary<string, GameObject>()
        {
            {"Joystick", joystickg},
            {"Sphere", sphereg},
            {"Fishing", fishingg},
            {"Wand",billiardcueg},
            {"Switch",myswitchg},
            {"Inflator",inflatorg},
            {"Trumpet",trumpetg},
            {"Spray",sprayg},
            {"ThumbPiano",thumbpianog},
            {"Scissors", scissorsg},
            //{"Trigger", triggerg},
            {"Binoculars", binocularsg}
        };

        foreach(var ges in gesturedict)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        foreach(var ges in gesturedictgrasp)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        lefthand.GetComponent<Renderer>().enabled = true;
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
                Save(flag.IsVirtualGrasp);
            }

            Gesture currentGesture = Recognize();

            bool hasRecognized = !currentGesture.Equals(new Gesture());
            //check if gesture changes
            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                //new gesture!
                // why this never prints???
                //Debug.Log("Gesture detected: "+currentGesture.name);
                previousGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
            
            //Debug.Log("Current Gesture:"+currentGesture.name);
            //GestureLogger.SetText("Current Gesture:{0}",currentGesture.name);
            GestureLogger.text="Current Gesture:"+currentGesture.name;
            HandInterfaceRendering(currentGesture);
            if (writeModeLogger)
            {
                if (flag.IsVirtualGrasp){
                //ModeLogger.SetText("Virtual Grasp");
                ModeLogger.text="Retrieval - VirtualGrasp";
                } else {
                //ModeLogger.SetText("Hand Interface");
                ModeLogger.text="Retrieval - Hand Interface";
                }
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

    void Save(bool IsVirtualGrasp)
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
        if (IsVirtualGrasp){
            gesturesgrasp.Add(g);
        } else if (!IsVirtualGrasp){
            gestures.Add(g);
        }
        

    }

    public Gesture Recognize()
    {
        Gesture currentgesture = new Gesture();
        float currentMin = Mathf.Infinity;
        int testk = 0;
        
        if (flag.IsVirtualGrasp){
            gestureslist=gesturesgrasp;
        } else if (!flag.IsVirtualGrasp){
            gestureslist=gestures;
        }
        foreach (var gesture in gestureslist)
        {
            //Debug.Log("debug mode is "+debugMode);
            
            float sumDistance = 0;
            bool isDiscarded = false;
            float adaptivethreshold;
            if (gesture.name == "Joystick" && flag.IsVirtualGrasp==false){
                adaptivethreshold = 0.06f;
            }
            else if (gesture.name == "Fishing" && flag.IsVirtualGrasp==false){
                adaptivethreshold = 0.06f;
            }
            else if (flag.IsVirtualGrasp == false){
                adaptivethreshold = threshold;
            } 
            else {
                adaptivethreshold = graspthreshold;
            }
            
            //Debug.Log("Recog_fingerBones.Count is "+fingerBones.Count);
            for (int i = 0; i < fingerBones.Count; i++)
            {
                
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData,gesture.fingerDatas[i]);
                //Debug.Log("distance of finger "+i+" is " +distance+", threshold is "+threshold);
                if (distance>adaptivethreshold)
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

        if (flag.IsVirtualGrasp){
            gesturedictlist=gesturedictgrasp;
        } else if (!flag.IsVirtualGrasp){
            gesturedictlist=gesturedict;
        }

        foreach(var ges in gesturedictlist)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        //lefthand.GetComponent<Renderer>().enabled = true;
        if(caseSwitch!=null)
        {
            if (caseSwitch=="Binoculars"){
                return;//two hands trigger its rendering. One hand cannot.
            }
            if (gesturedictlist.ContainsKey(caseSwitch)){
                //gesturedictlist[caseSwitch].SetActive(true);
                ChildrenRendering(gesturedictlist[caseSwitch], true);
                //lefthand.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            //lefthand.GetComponent<Renderer>().enabled = true;
        }
        //Debug.Log("test if word is in the list" + gesturedictlist.ContainsKey("BilliardCue"));
        
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    }
 
}