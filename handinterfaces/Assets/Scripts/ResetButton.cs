using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    private Vector3 initialPosition;
    void Start()
    {
        initialPosition = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(target.GetComponent<Renderer>().enabled == true){
            target.transform.position = initialPosition;
        }
    }
}
