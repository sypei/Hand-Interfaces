using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private GameObject startPoint;//the fishing rod top
    [SerializeField]
    private GameObject endPoint;//the bait
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawTheLine();
    }

    void DrawTheLine()
    {
        //start point
        line.SetPosition(0, startPoint.transform.position);
        //end point
        line.SetPosition(1, endPoint.transform.position);
    }
}
