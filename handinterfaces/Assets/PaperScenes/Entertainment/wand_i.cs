using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wand_i : MonoBehaviour
{
    [SerializeField] private GameObject wand;
    [SerializeField] private GameObject position_box;
    public bool IsSpell = false;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 2) {
            IsSpell = true;
            count = 0;
        } else {
            IsSpell = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(wand.GetComponent<Renderer>().enabled == true)
        {
            count += 1;
        }
    }
}
