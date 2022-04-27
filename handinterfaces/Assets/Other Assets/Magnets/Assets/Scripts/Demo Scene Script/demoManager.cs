using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoManager : MonoBehaviour
{
    public List<GameObject> magnets;
    public KeyCode changeMagnetKey = KeyCode.C;
    int currentMagnet;

    void Start()
    {
        changeMagnet(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(changeMagnetKey))
        {
            currentMagnet++;
            if (currentMagnet > magnets.Count - 1) currentMagnet = 0;
            changeMagnet(currentMagnet);
        }
    }

    public void changeMagnet(int newMagnet)
    {
        for (int i = 0; i < magnets.Count; i++)
        {
            magnets[i].SetActive(false);
        }

        magnets[newMagnet].SetActive(true);
    }
}