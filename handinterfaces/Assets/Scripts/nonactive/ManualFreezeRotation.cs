using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualFreezeRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject handler;
    [SerializeField]
    private bool freezeX;
    [SerializeField]
    private bool freezeY;
    [SerializeField]
    private bool freezeZ;
    [SerializeField]
    private float mx = 0.0f;   
    private float x; 
    [SerializeField]
    private float my = 0.0f;
    private float y; 
    [SerializeField]
    private float mz = 0.0f;
    private float z; 
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(!freezeZ){
            x = handler.transform.localEulerAngles.x;
        } else x=mx;
        if(!freezeY){
            y = handler.transform.localEulerAngles.y;
        } else y=my;
        if(!freezeY){
            y = handler.transform.localEulerAngles.y;
        } else z=mz;
        handler.transform.localEulerAngles = new Vector3(x,y,z);
    }
}
