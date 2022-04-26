using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class probes_i : MonoBehaviour
{
    public probes_wire probe_N_wire;//left
    public probes_wire probe_P_wire;//right
    private string N_wire;
    private string P_wire;
    private float voltage_reading;
    public TextMeshPro VoltageLogger;
    private Dictionary<string, float> multimeterDict;
    // Start is called before the first frame update
    void Start()
    {
        multimeterDict = new Dictionary<string, float>{
            {"AB", -6},
            {"AC", -3},
            {"AD", -6},
            {"BA", 6},
            {"BC", 3},
            {"BD", 0},
            {"CA", 3},
            {"CB", -3},
            {"CD", -3},
            {"DA", 6},
            {"DB", 0},
            {"DC", 3},
        };
    }

    // Update is called once per frame
    void Update()
    {
        N_wire = probe_N_wire.probe_wire;
        P_wire = probe_P_wire.probe_wire;
        Debug.Log("Nwire "+N_wire+" Pwire "+P_wire);
        string combination = P_wire+N_wire;
        if (combination != ""){
            if (multimeterDict.ContainsKey(combination)){
                voltage_reading = multimeterDict[combination];
                VoltageLogger.text="Voltage: "+voltage_reading+" V";
            }
        } else {
            VoltageLogger.text="Voltage: null";
        }
        
    }
}
