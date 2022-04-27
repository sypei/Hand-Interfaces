using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class probes_wire : MonoBehaviour
{
    
    // private GameObject wireA;
    // private GameObject wireB;
    // private GameObject wireC;
    // private GameObject wireD;
    private GameObject computerDesk;
    public string probe_wire;
    // private GameObject probes;
    // private GameObject probe_N;
    // Start is called before the first frame update
    void Start()
    {
        // wireA = GameObject.Find("wireA");
        // wireA = GameObject.Find("wireB");
        // wireA = GameObject.Find("wireC");
        // wireA = GameObject.Find("wireD");
        computerDesk = GameObject.Find("computerDesk");
        // probes = GameObject.Find("probes");//For consistency
        // probe_N = GameObject.Find("probe_N");
        Physics.IgnoreCollision(computerDesk.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void OnTriggerStay(Collider other)
    {
        // Debug.Log("collision detected!"+other.gameObject.name);
        switch (other.gameObject.name)
        {
            case "wireA":
                probe_wire = "A";
                break;

            case "wireB":
                probe_wire = "B";
                break;

            case "wireC":
                probe_wire = "C";
                break;

            case "wireD":
                probe_wire = "D";
                break;

            default:
                probe_wire = "";
                break;
        }     
    }

    // void OnTriggerExit(Collider other)
    // {
    //     // Destroy everything that leaves the trigger
    //     probe_wire = "";
    // }
}
