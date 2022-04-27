using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Frequency_Count : MonoBehaviour
{
    public OVRSkeleton skeleton;
    private List<OVRBone> fingerBones;
    private List<OVRBone> previousBones;
    private float timer = 0f;
    private bool thereAreBones = false;
    private GameObject hand;
    private int count = 0;
    private float freq = 0f;
    //Hand Interface
    
    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousBones = new List<OVRBone>(skeleton.Bones);     
        hand = GameObject.FindGameObjectsWithTag("lefthand")[0];
        hand.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 20)
        {
            timer = 0f;
            count = 0;
        }

        timer += Time.deltaTime;


        fingerBones= new List<OVRBone>(skeleton.Bones);//added
        //should not put it in save, 
        //or fingerBones will have nth when you don't press space
        
        if (fingerBones != null){
            if (fingerBones != previousBones){
                count += 1;
                previousBones = fingerBones;
                // Debug.Log("Start to count for this gesture: "+fingerBones);
            } 
        }
        freq = count/timer;
        Debug.Log("hand tracking freq: "+freq);
        Debug.Log(string.Format("count{0}x time{1}",count,timer));

    }

    void FindBones()
    {
        //if (new List<OVRBone>(skeleton.Bones).Count > 0)
        if (skeleton.Bones.Count > 0)
        {
            //fingerBones= new List<OVRBone>(skeleton.Bones);//added
            thereAreBones = true;
        }
    }
}