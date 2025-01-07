using System;
using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using System.Diagnostics;
using System.Reflection;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using UnityEditor.Timeline.Actions;
using Unity.VisualScripting;
public class COREManager : MonoBehaviour
{
    [Header("Important Vars")]
    public string CoreStatus = "OFFLINE";
    public string CoreState = "NONE";
    public string CoreEvent = "NONE";
    public int CoreTemp = 0; // Current temperature of the core
    public int CAH = 50; // Core additional heating
    public int CorePressure = 0; // Core pressure
    public float CoreTempChange = 1; // Rate of change for core temperature
    public float changeSpeed = 0f; // Base speed of temperature change
    public float changeSpeedCoreInfluence = 0f; // Core influence on change speed
    private float timeAccumulator = 0; // Tracks time for smoother updates

    [Header("Temperature and Efficiency Vars")]
    public float MaxHeatUnitEfficiency = 20;
    public float MaxCoolingUnitEfficiency = 25;
    public float MaxShieldCoolingEfficiency = 50;

    [Header("Stabs Connection")]
    public STABSLasers Stab1; // Cooling Unit
    public STABSLasers Stab2; // Cooling Unit
    public STABSLasers Stab3; // AUX
    public STABSLasers Stab4; // AUX
    public STABSLasers Stab5; // Heating Unit
    public STABSLasers Stab6; // Heating Unit
    public STABSLasers Stab7; // Heating Unit
    public STABSLasers Stab8; // Heating Unit

    [Header("Debug Vars")]
    public float debugTempChange = 0; // For debugging CoreTempChange

    [Header("Screen")]
    public TMPro.TextMeshProUGUI TempText;
    public TMPro.TextMeshProUGUI StateText;

    [Header("Core State")]
    //Pre Melt state
    public bool Overheating = false;
    public bool CritOverheating = false;
    
    //Pre Freeze state
    public bool PowerLoss = false;
    public bool ReactionLoss = false;

    //Custom Pre State
    public bool ReactorHandleLoss = false;

    //Catastrophic State
    public bool Freezedown = false;
    public bool Meltdown = false;



    void Update()
    {
        if (CoreStatus != "OFFLINE")
        {
            UpdateCoreTemperature();
        }        
    }


    //private void Start()
    //{
    //    test(UpdateCoreTemperature);
    //}

    //public void test()
    //{

    //}


    


    private void UpdateCoreTemperature()
    {
        
        // Reset CoreTempChange and apply additional heating
        CoreTempChange = CAH;

        // Heating unit effect
        if (Stab3.StabCheckForCoreVal())
        {
            CoreTempChange += MaxHeatUnitEfficiency * (Stab3.Power / 100f);
        }
        if (Stab4.StabCheckForCoreVal())
        {
            CoreTempChange += MaxHeatUnitEfficiency * (Stab4.Power / 100f);
        }

        // Cooling unit effect
        if (Stab1.StabCheckForCoreVal())
        {
            CoreTempChange -= MaxCoolingUnitEfficiency * (Stab1.Power / 100f);
        }
        if (Stab2.StabCheckForCoreVal())
        {
            CoreTempChange -= MaxCoolingUnitEfficiency * (Stab2.Power / 100f);
        }

        // Shield
        CoreTempChange -= MaxShieldCoolingEfficiency * (MCFS.instance.ShieldPower / 100f);

        // Calculate adjustment speed
        float adjustmentSpeed = Mathf.Abs(CoreTempChange) * changeSpeed * changeSpeedCoreInfluence;

        // Accumulate time for smooth updates
        timeAccumulator += Time.deltaTime * adjustmentSpeed;

        // Apply temperature changes based on the time accumulator
        while (timeAccumulator >= 1f)
        {
            if (CoreTempChange > 0.9f)
            {
                CoreTemp += 1;
            }
            else if (CoreTempChange < -1f)
            {
                CoreTemp -= 1;
            }
            else
            {
                CoreTemp += 0;
                print("core stable");
            }
            debugTempChange = CoreTempChange; //  For debugging purposes
            TempText.text = "" + CoreTemp + "C°";
            timeAccumulator -= 1f;
        }
    }
     
    async Task COREConstantStateChecker()
    {
        while(RegenHandler.instance.AppRunning && CoreStatus == "ONLINE" && CoreEvent == "NONE")
        {

            if (CoreTemp > 60000)
            {
                //PREMELT.
            }

            if (CoreTemp < 500)
            {
                //PRE-FREEZE/STALL.
            }

            Task.Delay(0250);
        }
    }


}



