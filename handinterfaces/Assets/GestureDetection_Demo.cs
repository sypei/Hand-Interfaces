using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GestureDetection_Demo : MonoBehaviour
{
    public float threshold = 0.02f;// too small, will find nothing (too strict)
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public Gesture currentGesture_stable;
    public bool debugMode = true;
    public TextMeshPro ModeLogger;
    public bool writeModeLogger = true;
    public TextMeshPro GestureLogger;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    private bool thereAreBones = false;
    private GameObject lefthand;
    //Hand Interface
    // private GameObject joystick;
    // private GameObject thumbpiano;
    private GameObject scissors;
    private float startTime = 0f;
    private float timer = 0f;
    public float holdTime = 0.2f;
   
    private Dictionary<string, GameObject> gesturedict;
    // private GameObject[] assets;
    private string currentInterface;

    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();    
        currentGesture_stable = new Gesture();    
        lefthand = GameObject.FindGameObjectsWithTag("lefthand")[0];
        //Hand Interface
        // joystick = GameObject.FindGameObjectsWithTag("joystick")[0];
        // thumbpiano = GameObject.FindGameObjectsWithTag("thumbpiano")[0];
        scissors = GameObject.FindGameObjectsWithTag("scissors")[0];
        // assets = GameObject.FindGameObjectsWithTag("assets");

        gesturedict = new Dictionary<string, GameObject>()
        {
            // {"Joystick", joystick},
            // {"ThumbPiano", thumbpiano},
            {"Scissors", scissors}
        };

        foreach(var ges in gesturedict)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        lefthand.GetComponent<Renderer>().enabled = true;
        // RelatedAssetsRendering();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!thereAreBones){
            FindBones();
        }

        if (thereAreBones)
        {
            //should not put it in save, 
            //or fingerBones will have nth when you don't press space
            fingerBones= new List<OVRBone>(skeleton.Bones);//added
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
                    Debug.Log("Time: "+timer);
                    if (timer > (startTime + holdTime))
                    {
                        currentGesture_stable = currentGesture;
                        // Debug.Log("get stable gesture: "+currentGesture_stable.name);
                    }
                }
                previousGesture = currentGesture;
            }

            // bool hasRecognized = !currentGesture.Equals(new Gesture());
            // //check if gesture changes
            // if (hasRecognized && !currentGesture.Equals(previousGesture))
            // {
            //     previousGesture = currentGesture;
            //     currentGesture.onRecognized.Invoke();
            // }

            currentInterface = currentGesture.name;
            
            GestureLogger.text="Current Gesture:"+currentGesture.name;
            HandInterfaceRendering(currentGesture);
            // RelatedAssetsRendering();
            if (writeModeLogger)
            {
                ModeLogger.text="Scissors Demo - Hand Interface";
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
        int testk = 0;
        
        foreach (var gesture in gestures)
        {
            //Debug.Log("debug mode is "+debugMode);
            
            float sumDistance = 0;
            bool isDiscarded = false;
            float adaptivethreshold;
            if (gesture.name == "Joystick"){
                adaptivethreshold = 0.06f;
            }
            else {
                adaptivethreshold = threshold;
            } 
            
            for (int i = 0; i < fingerBones.Count; i++)
            {
                
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData,gesture.fingerDatas[i]);
                if (distance > adaptivethreshold)
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

    void FindByNameRendering(string name, bool isEnabled)
    {
        GameObject Gobj = GameObject.Find(name);
        Gobj.GetComponent<Renderer>().enabled = isEnabled;
        ChildrenRendering(Gobj,isEnabled);
    }
    void RelatedAssetsRendering()
    {
        // foreach(var asset in assets)
        // {
        //     //disable themselves
        //     asset.GetComponent<Renderer>().enabled = false;
        //     //disable their children as well
        //     ChildrenRendering(asset, false);
        // }
        FindByLayerRendering(3,false);//cuttable layers

        // if(currentInterface=="Joystick"){
        //     FindByNameRendering("JoystickControlledCube", true);
        //     FindByNameRendering("JoystickResetButton", true);
        // } else 
        if(currentInterface=="Scissors"){
            FindByLayerRendering(3,true);
        }
    }
    void FindByLayerRendering(int layNum,bool isEnabled)
    {
        GameObject[] cuttables = FindGameObjectsInLayer(layNum);
        foreach (GameObject cuttable in cuttables)
        {
            cuttable.GetComponent<Renderer>().enabled = isEnabled;
            //ChildrenRendering(Gobj,isEnabled);
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

    void HandInterfaceRendering(Gesture currentgesture)
    {
        string caseSwitch = currentgesture.name;

        foreach(var ges in gesturedict)
            ChildrenRendering(ges.Value, false);
        if(caseSwitch!=null)
        {
            if (gesturedict.ContainsKey(caseSwitch)){
                ChildrenRendering(gesturedict[caseSwitch], true);
            }
        }
        
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    }
 
}