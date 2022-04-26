using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCount : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject num_0;
    [SerializeField]
    private GameObject num_1;
    [SerializeField]
    private GameObject num_2;
    [SerializeField]
    private GameObject num_3;
    [SerializeField]
    private GameObject num_4;
    [SerializeField]
    private GameObject num_5;
    [SerializeField]
    private GameObject handler;
    [SerializeField]
    private GameObject rodTop;
    [SerializeField]
    private GameObject bait;
    private List<GameObject> numbers=new List<GameObject>();//A list must new() here or later
    private float distance;
    private int currentNum;
    private int lastNum;
    private float waterLevel;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float max = 50f;
    private float percentage = 0f;

    
    void Start()
    {
        numbers.Add(num_0);
        numbers.Add(num_1);
        numbers.Add(num_2);
        numbers.Add(num_3);
        numbers.Add(num_4);
        numbers.Add(num_5);
        lastNum = 0;
        waterLevel = bait.transform.position.y;
        startPosition = bait.transform.position;
        endPosition = rodTop.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentNum = GetCurrentNumber();
        int change = currentNum - lastNum;
        //Debug.Log(percentage/max+"percentage");
        endPosition = rodTop.transform.position;
        if (change ==1 || change == -5)
        {
            //bait.transform.LookAt(handler.transform.position);
            if (percentage < max){
                percentage += 1f;
            }
            bait.transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, endPosition.x, percentage/max),
                waterLevel,
                Mathf.Lerp(startPosition.z, endPosition.z, percentage/max));
        }
        else if (change == -1 || change == 5)
        {
            if (percentage > 0f){
                percentage -= 1f;
            }
            bait.transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, endPosition.x, percentage/max),
                waterLevel,
                Mathf.Lerp(startPosition.z, endPosition.z, percentage/max));
        }
        lastNum = currentNum;

    }
    
    int GetCurrentNumber()
    {
        int minIndex = 0;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < numbers.Count; i++)
        {
            distance = Vector3.Distance(handler.transform.position,numbers[i].transform.position);
            if(distance < minDistance)
            {
              minDistance = distance;
              minIndex = i ;
            }
        }
        //Debug.Log("Closest node: " + minIndex + " - distance = " + minDistance);
        return minIndex;
    }

}
