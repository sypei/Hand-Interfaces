using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


// Show off all the Debug UI components.
public class MenuDebugUI : MonoBehaviour
{
    bool inMenu;
    private Text sliderText;
    private bool currentState = false;
    private bool lastState = false;

    //Menu
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
    private GameObject binoculars;
    [SerializeField]
    private GameObject dropAnimatioon;
    private GameObject menuObjects;
    private Dictionary<string, GameObject> gesturedict;
    private ToggleGroup toggleGroup;

	void Start ()
    {
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
        binoculars = GameObject.FindGameObjectsWithTag("binoculars")[0];

        menuObjects = GameObject.Find("MenuObjects");

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
            {"Binoculars", binoculars}
        };  

        DebugUIBuilder.instance.AddLabel("Interface Menu");
        DebugUIBuilder.instance.AddDivider();
        foreach(var ges in gesturedict)
        {
            ChildrenRendering(ges.Value, false);
            //DebugUIBuilder.instance.AddToggle(ges.Key, TogglePressed);
            DebugUIBuilder.instance.AddToggleInGroup(ges.Key, "group", delegate(Toggle t) { RadioPressed(ges.Key, "group", t); }) ;

        }
        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddButton("Drop the interface", LogButtonPressed);
        //DebugUIBuilder.instance.Show();
        inMenu = false;
        menuObjects.SetActive(false);
	}
    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    }
    public void TogglePressed(Toggle t)
    {
        // foreach(var ges in gesturedict)
        // {
        //     ChildrenRendering(ges.Value, false);
        // }
        // if (gesturedict.ContainsKey(radioLabel)){
        //     ChildrenRendering(gesturedict[radioLabel], true);
        // }
        Debug.Log("Toggle pressed. Is on? "+t.isOn);
    }
    public void RadioPressed(string radioLabel, string group, Toggle t)
    {
        foreach(var ges in gesturedict)
        {
            ChildrenRendering(ges.Value, false);
        }
        if (gesturedict.ContainsKey(radioLabel)){
            ChildrenRendering(gesturedict[radioLabel], true);
        }
        Debug.Log("Radio value changed: "+radioLabel+", from group "+group+". New value: "+t.isOn);
        toggleGroup = t.group;
    }

    public void SliderPressed(float f)
    {
        Debug.Log("Slider: " + f);
        sliderText.text = f.ToString();
    }

    void Update()
    {
        // DebugUIBuilder.instance.Show();
        //Debug.Log(OVRInput.Get(OVRInput.RawButton.RIndexTrigger)+"r button status");
        if(OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            currentState = true;
        else
            currentState = false;
        if(currentState && !lastState)
        {
            if (inMenu) {
                DebugUIBuilder.instance.Hide();
                menuObjects.SetActive(false);
            }
            else {
                DebugUIBuilder.instance.Show();
                menuObjects.SetActive(true);
            }
            inMenu = !inMenu;
        }
        lastState = currentState;

        if(inMenu && (toggleGroup.AnyTogglesOn() == false))
        {
            foreach(var ges in gesturedict)
            {
                ChildrenRendering(ges.Value, false);
            }
        }

    }

    void LogButtonPressed()
    {
        Debug.Log("Button pressed");
        if(toggleGroup.AnyTogglesOn())
        {
            toggleGroup.SetAllTogglesOff();
        }
        foreach(var ges in gesturedict)
        {
            ChildrenRendering(ges.Value, false);
        }
        dropAnimatioon.GetComponent<ParticleSystem>().Play();
    }
}
