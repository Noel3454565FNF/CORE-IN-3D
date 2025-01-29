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
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using System.ComponentModel;
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
    public int coreStability = 100;

    public bool CanDecreaseCoreStability = true;
    public bool CanIncreaseCoreStability = true;

    public bool CanUpdateTemp = false;

    [Header("Temperature and Efficiency Vars")]
    public float MaxHeatUnitEfficiency = 20;
    public float MaxCoolingUnitEfficiency = 25;
    public float MaxShieldCoolingEfficiency = 50;
    public float MasCoreInstabilityEfficiency = 20;

    public float CoreStabilityDecreasingSpeed = 0.2f;
    public float CoreStabilityIncreasingSpeed = 0.1f;

    [Header("Stabs Connection")]
    public STABSLasers Stab1; // Cooling Unit
    public STABSLasers Stab2; // Cooling Unit
    public STABSLasers Stab3; // AUX
    public STABSLasers Stab4; // AUX
    public STABSLasers Stab5; // Power Unit
    public STABSLasers Stab6; // Power Unit
    public STABSLasers Stab7; // Power Unit
    public STABSLasers Stab8; // Power Unit

    [HideInInspector] public List<STABSLasers> Stablist;

    [Header("Debug Vars")]
    public float debugTempChange = 0; // For debugging CoreTempChange
    public static COREManager instance;

    [Header("Screen")]
    public TMPro.TextMeshProUGUI TempText;
    public TMPro.TextMeshProUGUI StateText;

    public UnityEngine.UI.Image CoreDiag;
    public RawImage CoreWS;
    public TMPro.TextMeshProUGUI CoreUS;

    public LineClone ReactorSysLogsScreen;
    
    public Color LineAttentionColor;
    public Color LineWarnColor;
    public Color LineUnknownColor;
    public Color LineOVERRIDEColor;


    [Header("Core State")]
    //global
    public bool CoreInEvent = false;
    public bool CoreAllowGridEvent = true;

    //Pre Melt state
    public bool Overheating = false;
    public bool CritOverheating = false;
    public bool Premeltdown = false;
    public bool ShieldDetonationImminent = false;
    
    //Pre Freeze state
    public bool PowerLoss = false;
    public bool ReactionLoss = false;

    //Custom Pre State
    public bool ReactorHandleLoss = false;

    //Catastrophic State
    public bool Freezedown = false;
    public bool FreezedownHISTORY = false;
    public bool Meltdown = false;
    public bool Overload = false;
    public bool ControlLoss = false;

    [Header("Screen Connection")]
    public GlobalScreenManager MCFSscreen;
    public GlobalScreenManager MainCoreScreen;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        Stablist.Add(Stab1);
        Stablist.Add(Stab2);
        Stablist.Add(Stab3);
        Stablist.Add(Stab4);
        UpdateCoreTemperature();
    }

    void Update()
    {
        if (CoreStatus != "OFFLINE")
        {
            UpdateCoreTemperature();
        }
        StateText.text = CoreStatus.ToLower();
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
        // Reset CoreTempChange and apply base heating
        CoreTempChange = CAH;

        // Core instability effect
        CoreTempChange -= MasCoreInstabilityEfficiency * (coreStability / 100f);

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

        // Shield effect
        CoreTempChange -= MaxShieldCoolingEfficiency * ((int)MCFS.instance.ShieldIntegrity / 100f);

        // Debug: Log CoreTempChange before damping
        debugTempChange = CoreTempChange;

        // Apply a damping factor to reduce sensitivity
        CoreTempChange *= 0.8f; // Damping factor to smooth abrupt changes

        // Clamp CoreTempChange to prevent extreme values
        CoreTempChange = Mathf.Clamp(CoreTempChange, -100f, 100f); // Adjust range as needed

        // Debug: Log CoreTempChange after damping and clamping

        // Calculate adjustment speed
        float adjustmentSpeed = Mathf.Abs(CoreTempChange) * changeSpeed * changeSpeedCoreInfluence;

        // Accumulate time for smooth updates
        timeAccumulator += Time.deltaTime * adjustmentSpeed;

        // Debug: Log time accumulator value

        // Apply temperature changes based on the time accumulator
        while (timeAccumulator >= 1f && CanUpdateTemp)
        {
            if (Mathf.Abs(CoreTempChange) > 0.1f) // Apply only significant changes
            {
                // Convert the floating-point result to an integer
                CoreTemp += (int)Mathf.Sign(CoreTempChange) * Mathf.Min(1, (int)Mathf.Abs(CoreTempChange));
            }
            else
            {
                UnityEngine.Debug.Log("Core is stable.");
            }

            // Debug: Log updated CoreTemp value

            TempText.text = $"{CoreTemp}C°"; // Update temperature text
            timeAccumulator -= 1f;
            COREConstantStateChecker();
        }
    }

    public void COREConstantStateChecker()
    {
        if(RegenHandler.instance.AppRunning && CoreStatus != "OFFLINE" && CoreInEvent == false)
        {
            if (CoreTemp > 7600 && Overheating == false && CritOverheating == false)
            {
                Overheating = true;
                //P1 PREMELT.
                FAS.GFAS.WriteAnAnnouncement("ReactorSys", "!Warning Core overheating, nuclear meltodwn IMMINENT!", 9); ReactorSysLogsScreen.EntryPoint("Core overheating! please engage cooling systems.", Color.yellow);
                StartCoroutine(CoreStabilityDecrease());
                TempText.color = Color.yellow;
                //LightsManager.GLM.LevelNeg3LightsFlick(3, Negate3roomsName.ALL);
            }
            if (coreStability == 0 && Overheating == true && CritOverheating == false)
            {
                CritOverheating = true;
                //P2 PREMELT.
                FAS.GFAS.WriteAnAnnouncement("ReactorSys", "Shield absortion capacity have reached his limit. PLEASE engage the Core Power Purge IMMEDIATLY.", 9); ReactorSysLogsScreen.EntryPoint("Core overheating! please engage cooling systems.", Color.yellow);
                MCFS.instance.CanShieldDegrad = true;
                StartCoroutine(MCFS.instance.ShieldDegradationFunc());
                MCFS.instance.integrityTxt.color = Color.yellow;
                //LightsManager.GLM.LevelNeg3LightsFlick(7, Negate3roomsName.ALL);
            }
            if (MCFS.instance.ShieldIntegrity <= 10 && Overheating && CritOverheating)
            {
                //P3 PREMELT.
            }


            if (CoreTemp < 7500 && Overheating == true &&  CritOverheating == false)
            {
                //BAI PREMELT.
                StopCoroutine(CoreStabilityIncrease());
                Overheating = false;
                TempText.color = Color.white;
            }



            if (CoreTemp < 500)
            {
                //PRE-FREEZE/STALL.
            }
        }
    }


    public IEnumerator CoreStabilityDecrease()
    {
        yield return new WaitForSeconds(CoreStabilityDecreasingSpeed);
        var t = coreStability - 1;
        if (t > -1)
        {
            coreStability = coreStability - 1;
            StartCoroutine(CoreStabilityDecrease());
        }
        else
        {
            StopCoroutine(CoreStabilityDecrease());
        }
    }
    public IEnumerator CoreStabilityIncrease()
    {
        yield return new WaitForSeconds(CoreStabilityIncreasingSpeed);
        var t = coreStability + 1;
        if (t < 101)
        {
            coreStability = coreStability + 1;
            StartCoroutine(CoreStabilityIncrease());
        }
        else
        {
            StopCoroutine(CoreStabilityIncrease());
        }
    }

    public void LeantweenTemp(float to, float time)
    {
        var frfr = CoreTemp;
        LeanTween.value(frfr, to, time)
            .setOnUpdate((float t) =>
            {
                CoreTemp = Mathf.FloorToInt(t);
                TempText.text = "" + CoreTemp + "C°";
            });
    }


    //CORE SCREEN FONCTIONS

    public void CoreToOnline()
    {
        CoreWS.gameObject.active = false;
        CoreUS.enabled = false;
        CoreDiag.gameObject.active = true;
    }

    public void CoreToUnknown()
    {
        CoreWS.gameObject.active = false;
        CoreUS.enabled = true;
        CoreDiag.gameObject.active = false;
        CoreUS.color = Color.white;
    }

    public void CoreToDanger()
    {
        CoreWS.gameObject.active = true;
        CoreUS.enabled = false;
        CoreDiag.gameObject.active = false;
    }

    public void CoreToUnknownDanger()
    {
        CoreWS.gameObject.active = false;
        CoreUS.enabled = true;
        CoreDiag.gameObject.active = false;
        CoreUS.color = new Color(160, 32, 240);
    }



}



public class DEVSONLY : MonoBehaviour
{
    [ReadOnly(true)] public string README = "don't take anything here seriously lol-";
    [ReadOnly(true)] public string ThoseWhoknow = ":skull:";
    public void AndSuddenlyIGotTheUrgeToSingErika()
    {
        ThoseWhoknow = "no seriously, erika is a good history song. the phonk version is peak :pray:";
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);
    }
}


