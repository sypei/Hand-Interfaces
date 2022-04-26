using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFlag : MonoBehaviour
{
    public bool IsVirtualGrasp = false;//detection VG(3) + interaction VG(6)
    public bool IsHandInterface = false;//detection HI(2) + interaction HI(5)
    public bool IsGD = false; //detection VG(3) + detection HI(2)
    public bool IsBaselineDetection = false;//controller menu baseline scene(1)
    public bool IsInteraction = false;//interaction HI(5) + interaction VG(6) + demo(7)
    public bool IsBaselineInteraction = false;//interaction baseline scene(4)
    public bool IsDemo = false;//demo scene(7)
    void Update()
    {
        IsDemo = IsGD && IsInteraction;
    }
}


