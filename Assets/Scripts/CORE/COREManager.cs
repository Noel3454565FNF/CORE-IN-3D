using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class COREManager : MonoBehaviour
{


    [Header("Important Vars")]
    public string CoreStatus = "OFFLINE";
    public string CoreEvent = "NONE";
    public int CoreTemp = 0;
    public int CAH = 0;
    public int CorePressure = 0;
    public float CoreTempChange = 1; // Rate of change for core temperature
    public float changeSpeed = 0f; // Base speed of temperature change
    public float changeSpeedCoreInfluence = 0f;
    private float timeAccumulator = 0; // Tracks time for smoother updates


    [Header("Stabs Connection")]
    //Shield Unit & Cooling Unit
    public STABSLasers Stab1;
    public STABSLasers Stab2;
    //Heating Unit & Power Extraction Unit
    public STABSLasers Stab3;
    public STABSLasers Stab4;
    //Aux Unit & Heating/Cooling/Shield Unit
    public STABSLasers Stab5;
    public STABSLasers Stab6;

    void Update()
    {
        if (CoreStatus != "OFFLINE")
        {
            ChangeCoreTemp();
        }
    }

    private void ChangeCoreTemp()
    {
        if (Stab1.StabCheckForCoreVal())
        {
            CoreTempChange = CoreTempChange - (3 * Stab1.Power / 10);
        }
        if (Stab2.StabCheckForCoreVal())
        {
            CoreTempChange = CoreTempChange - (3 * Stab2.Power / 10);
        }
        // Calculate the adjustment speed based on CoreTempChange
        float adjustmentSpeed = Mathf.Abs(CoreTempChange + CAH) * (changeSpeed * changeSpeedCoreInfluence);

        // Accumulate time to smoothly increment/decrement CoreTemp
        timeAccumulator += Time.deltaTime * adjustmentSpeed;

        // Apply changes only when the accumulator reaches 1 or higher
        while (timeAccumulator >= 1f)
        {
            // Adjust CoreTemp up or down based on the sign of CoreTempChange
            CoreTemp += (int)Mathf.Sign(CoreTempChange);
            timeAccumulator -= 1f;
        }
    }
}
