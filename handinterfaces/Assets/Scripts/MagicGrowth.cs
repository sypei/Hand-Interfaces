using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGrowth : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 initialSize;
    public GameObject wand;
    void Start()
    {
        initialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision col)
    {
        if(wand.GetComponent<Renderer>().enabled == true)
        {
            transform.localScale += new Vector3 (0.01f,0.01f,0.01f);
        }
    }
}
