using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
//struc = class without function
public class CallObjects : MonoBehaviour
{
    public TextMeshPro GestureLogger;
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
    private GameObject trigger;
    private GameObject binoculars;
    // Virtual Grasp
    // private GameObject joystickg;
    // private GameObject sphereg;
    // private GameObject fishingg;

    // private GameObject inflatorg;
    // private GameObject trumpetg;
    // private GameObject billiardcueg;
    // private GameObject sprayg;
    // private GameObject myswitchg;
    // private GameObject thumbpianog;
    // private GameObject scissorsg;
    // private GameObject triggerg;
    // private GameObject binocularsg;
    private Dictionary<string, GameObject> gesturedict;
    [HideInInspector]
    public string currentInterface;
    private GameObject[] assets;
    //This error: NullReferenceException: Object reference not set to an instance of an object
    // is because I did not drag the hand prefab to "skeleton" in inspector interface


    // Start is called before the first frame update
    void Start()
    {
        lefthand = GameObject.FindGameObjectsWithTag("lefthand")[0];
        assets = GameObject.FindGameObjectsWithTag("assets");
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
        trigger = GameObject.FindGameObjectsWithTag("trigger")[0];
        binoculars = GameObject.FindGameObjectsWithTag("binoculars")[0];
        

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
            {"Empty", trigger},
            {"Binoculars", binoculars}
        };

        foreach(var ges in gesturedict)
            //ges.Value.SetActive(false);
            ChildrenRendering(ges.Value, false);
        lefthand.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Recognize();
        //currentInterface = "Joystick";
        GestureLogger.text="Current Interface: "+currentInterface;
        HandInterfaceRendering();
        RelatedAssetsRendering();
    }

    public void Recognize()
    {
        if(Input.GetKeyDown(KeyCode.F1)){
            currentInterface="Joystick";
        } else if(Input.GetKeyDown(KeyCode.F2)){
            currentInterface="Fishing";
        } else if(Input.GetKeyDown(KeyCode.F3)){
            currentInterface="Sphere";
        } else if(Input.GetKeyDown(KeyCode.F4)){
            currentInterface="Inflator";
        } else if(Input.GetKeyDown(KeyCode.F5)){
            currentInterface="Trumpet";
        } else if(Input.GetKeyDown(KeyCode.F6)){
            currentInterface="Wand";
        } else if(Input.GetKeyDown(KeyCode.F7)){
            currentInterface="Switch";
        } else if(Input.GetKeyDown(KeyCode.F8)){
            currentInterface="Spray";
        } else if(Input.GetKeyDown(KeyCode.F9)){
            currentInterface="ThumbPiano";
        } else if(Input.GetKeyDown(KeyCode.F10)){
            currentInterface="Scissors";
        } else if(Input.GetKeyDown(KeyCode.F11)){
            currentInterface="Empty";
        } else if(Input.GetKeyDown(KeyCode.F12)){
            currentInterface="Binoculars";
        }
    }


    // If we use gameobject.GetComponent<Renderer>().enabled = false;
    // Once the gameobject is no longer active the code will no longer run in the update. (only run once even it is put in update())
    // Therefore we should use
    void FindByNameRendering(string name, bool isEnabled)
    {
        GameObject Gobj = GameObject.Find(name);
        Gobj.GetComponent<Renderer>().enabled = isEnabled;
        ChildrenRendering(Gobj,isEnabled);
    }
    void RelatedAssetsRendering()
    {
        foreach(var asset in assets)
        {
            //disable themselves
            asset.GetComponent<Renderer>().enabled = false;
            //disable their children as well
            ChildrenRendering(asset, false);
        }
        FindByLayerRendering(3,false);//cuttable layers
            
        if(currentInterface=="Joystick"){
            FindByNameRendering("JoystickControlledCube", true);
            FindByNameRendering("JoystickResetButton", true);
        } else if(currentInterface=="Fishing"){
            FindByNameRendering("WaterSurface", true);
            FindByNameRendering("Bait", true);
        } else if(Input.GetKeyDown(KeyCode.F3)){
            //map's rendering, is defined in sphere's own script
        } else if(currentInterface=="Inflator"){
            FindByNameRendering("Balloon", true);
        } else if(Input.GetKeyDown(KeyCode.F5)){

        } else if(currentInterface=="Wand"){
            FindByNameRendering("BilliardBall",true);
            FindByNameRendering("BilliardBall2",true);
            FindByNameRendering("BilliardTable",true);
        } else if(Input.GetKeyDown(KeyCode.F7)){

        } else if(Input.GetKeyDown(KeyCode.F8)){
            //Erase all previous drawings when pressed again
            //Destroy??
        } else if(Input.GetKeyDown(KeyCode.F9)){

        } else if(currentInterface=="Scissors"){
            //FindByNameRendering("CuttableBall", true);
            FindByLayerRendering(3,true);

        // } else if(currentInterface=="Trigger"){
        //     //disable the controlled object (not done yet)
        } else if(currentInterface=="Binoculars"){
            //binocularview's rendering, is defined in bino's own script
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
    void HandInterfaceRendering()
    {
        string caseSwitch = currentInterface;

        foreach(var ges in gesturedict)
            ChildrenRendering(ges.Value, false);
            //ges.Value.SetActive(false);
        //lefthand.GetComponent<Renderer>().enabled = true;
        if(caseSwitch!=null)
        {
            if (gesturedict.ContainsKey(caseSwitch)){
                ChildrenRendering(gesturedict[caseSwitch], true);
                //gesturedict[caseSwitch].SetActive(true);
                //lefthand.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            //lefthand.GetComponent<Renderer>().enabled = true;
        }
        //Debug.Log("test if word is in the list" + gesturedictlist.ContainsKey("Wand"));
        
    }

    
    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    } 

}