using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COREManager : MonoBehaviour
{


    [Header("Important Vars")]
    public string CoreStatus = "OFFLINE";
    public string CoreEvent = "NONE";
    public int CoreTemp = 0;
    public int CorePressure = 0;
    public float CoreTempChange = 1; // Rate of change for core temperature
    public float changeSpeed = 0f; // Base speed of temperature change
    public float changeSpeedCoreInfluence = 0f;

    private float timeAccumulator = 0; // Tracks time for smoother updates

    void Update()
    {
        if (CoreStatus != "OFFLINE")
        {
            ChangeCoreTemp();
        }
    }

    private void ChangeCoreTemp()
    {
        changeSpeed = changeSpeed + changeSpeedCoreInfluence;
        // Calculate the adjustment speed based on CoreTempChange
        float adjustmentSpeed = Mathf.Abs(CoreTempChange) * changeSpeed;

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
