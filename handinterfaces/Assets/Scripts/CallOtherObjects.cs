using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// [System.Serializable]
//struc = class without function
public class CallOtherObjects : MonoBehaviour
{
    public TextMeshPro GestureLogger;
    private GameObject lefthand;
    //Other Hand Interface
    private GameObject waterBlaster;
    private GameObject ladle;
    private GameObject fork;
    private GameObject cameraHD;
    private GameObject wrench;
    private GameObject magnet;
    private GameObject book;
    private GameObject pen;
    private GameObject stapler;
    private GameObject mug;
    private GameObject flipPhone;
    private GameObject flower;
    private GameObject heart;
    private GameObject lever;
    private GameObject colorPalette;
    private GameObject probe_P;
    // private GameObject probe_N;
    private GameObject rake;
    private GameObject bird;
    private GameObject sandClock;
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
        waterBlaster = GameObject.Find("waterBlaster");
        ladle = GameObject.Find("ladle");
        fork = GameObject.Find("fork");     
        wrench = GameObject.Find("wrench");
        magnet = GameObject.Find("magnet");
        book = GameObject.Find("book");
        pen = GameObject.Find("pen");
        stapler = GameObject.Find("stapler");
        mug = GameObject.Find("mug");
        flipPhone = GameObject.Find("flipPhone");
        flower = GameObject.Find("flower");
        cameraHD = GameObject.Find("cameraHD");
        heart = GameObject.Find("heart");
        lever = GameObject.Find("lever");
        colorPalette = GameObject.Find("colorPalette");
        probe_P = GameObject.Find("probe_P");
        // probe_N = GameObject.Find("probe_N");
        rake = GameObject.Find("rake");
        bird = GameObject.Find("bird");
        sandClock = GameObject.Find("sandClock");
        gesturedict = new Dictionary<string, GameObject>()
        {
            {"Water Blaster", waterBlaster},
            {"Ladle", ladle},
            {"Fork", fork},
            {"Wrench", wrench},
            {"Magnet", magnet},
            // {"Book", book},
            {"Pen", pen},
            {"Stapler", stapler},
            {"Mug", mug},
            {"Flip Phone", flipPhone},
            {"Flower", flower},
            {"Camera", cameraHD},
            {"Heart", heart},
            {"Lever", lever},
            {"Color Palette", colorPalette},
            {"Multimeter Probes", probe_P},
            {"Grass Scooper", rake},
            {"Bird", bird},
            {"Sand Clock", sandClock}

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
        GestureLogger.text="Current Interface:"+currentInterface;
        HandInterfaceRendering();
        RelatedAssetsRendering();
    }

    public void Recognize()
    {
        if(Input.GetKeyDown(KeyCode.F1)){
            currentInterface="Water Blaster";
        } else if(Input.GetKeyDown(KeyCode.F2)){
            currentInterface="Ladle";
        } else if(Input.GetKeyDown(KeyCode.F3)){
            currentInterface="Fork";
        } else if(Input.GetKeyDown(KeyCode.F4)){
            currentInterface="Camera";
        } else if(Input.GetKeyDown(KeyCode.F5)){
            currentInterface="Wrench";
        } else if(Input.GetKeyDown(KeyCode.F6)){
            currentInterface="Magnet";
        } else if(Input.GetKeyDown(KeyCode.F7)){
            currentInterface="Book";
        } else if(Input.GetKeyDown(KeyCode.F8)){
            currentInterface="Pen";
        } else if(Input.GetKeyDown(KeyCode.F9)){
            currentInterface="Stapler";
        } else if(Input.GetKeyDown(KeyCode.F10)){
            currentInterface="Mug";
        } else if(Input.GetKeyDown(KeyCode.F11)){
            currentInterface="Flip Phone";
        } else if(Input.GetKeyDown(KeyCode.F12)){
            currentInterface="Flower";
        } else if(Input.GetKeyDown(KeyCode.Alpha1)){
            currentInterface="Heart";
        } else if(Input.GetKeyDown(KeyCode.Alpha2)){
            currentInterface="Lever";
        } else if(Input.GetKeyDown(KeyCode.Alpha3)){
            currentInterface="Color Palette";
        } else if(Input.GetKeyDown(KeyCode.Alpha4)){
            currentInterface="Multimeter Probes";
        } else if(Input.GetKeyDown(KeyCode.Alpha5)){
            currentInterface="Grass Scooper";
        } else if(Input.GetKeyDown(KeyCode.Alpha6)){
            currentInterface="Bird";
        } else if(Input.GetKeyDown(KeyCode.Alpha7)){
            currentInterface="Sand Clock";
        } else if(Input.GetKeyDown(KeyCode.Alpha8)){
            
        } else if(Input.GetKeyDown(KeyCode.Alpha9)){
            
        } else if(Input.GetKeyDown(KeyCode.Alpha0)){
            
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
            
        if(currentInterface=="Multimeter Probes"){
            FindByNameRendering("probe_N", true);
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
        // var trans = 0.5f;
        // var col = waterBlaster.GetComponent<Renderer>().material.GetColor("_Color");
        // col.a = trans;
        foreach(var ges in gesturedict)
            ChildrenRendering(ges.Value, false);
            //ges.Value.SetActive(false);
        //lefthand.GetComponent<Renderer>().enabled = true;
        if(caseSwitch!=null)
        {
            if (gesturedict.ContainsKey(caseSwitch)){
                if (caseSwitch != "Book"){
                    ChildrenRendering(gesturedict[caseSwitch], true);
                }
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
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())//The search includes parent as well.
            r.enabled = isEnabled;
        foreach (Canvas c in parent.GetComponentsInChildren<Canvas>())
            c.enabled = isEnabled;
    } 

}