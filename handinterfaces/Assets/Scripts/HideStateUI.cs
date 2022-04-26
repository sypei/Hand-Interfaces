using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideStateUI : MonoBehaviour
{
    private GameObject menuObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        menuObjects = GameObject.Find("MenuObjects");
    }

    // Update is called once per frame
    void Update()
    {
        if (menuObjects.activeInHierarchy==true)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = true;
        }
    }
}
