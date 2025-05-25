using System;
using TMPro;
using UnityEngine;
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
using System.Data.Common;
using Cysharp.Threading.Tasks.Triggers;
public class COREManager : MonoBehaviour
{
    [Header("Important Vars")]

    private string llll = "don't mind me, im useless";
    public enum CoreStatusEnum { Offline, Online, Overload, Stall };
    public string CoreStatus = CoreStatusEnum.Offline.ToString(); 
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
    public bool CanShutdown = false; public bool ForceDisableShutdown = false;
    public bool CanStartup = true; public bool ForceDisableStartup = false;

    [Header("Temperature and Efficiency Vars")]
    public float MaxHeatUnitEfficiency = 20;
    public float MaxCoolingUnitEfficiency = 25;
    public float MaxShieldCoolingEfficiency = 50;
    public float MasCoreInstabilityEfficiency = 20;

    public float CoreStabilityDecreasingSpeed = 0.2f;
    public float CoreStabilityIncreasingSpeed = 0.1f;

    [Header("Stabs Connection")]
    public STABSLasers Stab1; // Stabilization Unit
    public STABSLasers Stab2; // Stabilization Unit
    public STABSLasers Stab3; // Power Unit
    public STABSLasers Stab4; // Power Unit
    public STABSLasers Stab5; // Combustion/Aux Unit
    public STABSLasers Stab6; // Combustion/Aux Unit

    [HideInInspector] public List<STABSLasers> Stablist;

    [Header("Debug Vars")]
    public float debugTempChange = 0; // For debugging CoreTempChange
    public static COREManager instance;
    public MeshRenderer COREMeshRenderer;
    public Renderer CORERenderer;

    [Header("Screen")]
    public TMPro.TextMeshProUGUI TempText;
    public TMPro.TextMeshProUGUI StateText;

    public UnityEngine.UI.Image CoreDiag;
    public RawImage CoreWS;
    public TMPro.TextMeshProUGUI CoreUS;
    public GameObject CoreTR;

    public LineClone ReactorSysLogsScreen;
    
    public Color LineAttentionColor;
    public Color LineWarnColor;
    public Color LineUnknownColor;
    public Color LineOVERRIDEColor;


    public TMPro.TextMeshProUGUI MiddleUpTextScreen;
    public Vector3 MiddleUpTextScreenDefaultPos;
    public Vector3 MiddleUpTextScreenMovedPos;

    public TMPro.TextMeshProUGUI MiddleSubTextScreen;
    public Vector3 MiddleSubTextScreenDefaultPos;
    public Vector3 MiddleSubTextScreenMovedPos;



    [Header("Core State")]
    //global
    public bool CoreInEvent = false;
    public bool CoreAllowGridEvent = true;

    public enum CoreEventEnum
    {
        Startup,
        Shutdown,
        ShutdownFailure,
        Meltdown,
        Freezedown,
        FreezedownHISTORY,
        ReactorFault,

    }

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
    public bool ReactorFault = false;

    //Catastrophic State
    public bool Freezedown = false;
    public bool FreezedownHISTORY = false;
    public bool Meltdown = false;
    public bool Overload = false;
    public bool ControlLoss = false;

    [Header("Screen Connection")]
    public GlobalScreenManager MCFSscreen;
    public GlobalScreenManager MainCoreScreen;

    [Header("Component")]
    public EventManager eventmanager;
    public CorePurgeSYS CPSYS;


    [Header("Core Purge Sys Component")]
    public ButtonHandler CPSYSButton;

    [Header("Events scripts")]
    public Startup startup;


    private void Awake()
    {
        instance = this;
    }

    public void TestOverheat()
    {
        Stab1.EnterOverheat();
    }
    private void Start()
    {
        MiddleSubTextScreenMovedPos = MiddleSubTextScreen.gameObject.GetComponent<RectTransform>().position;
        MiddleUpTextScreenMovedPos = MiddleUpTextScreen.gameObject.GetComponent<RectTransform>().position;

        MiddleSubTextScreenDefaultPos.y = MiddleSubTextScreenMovedPos.y + 1f;
        MiddleUpTextScreenDefaultPos.y = MiddleUpTextScreenMovedPos.y - 1f;
        startup = Startup.instance;
        Stablist.Add(Stab1);
        Stablist.Add(Stab2);
        Stablist.Add(Stab3);
        Stablist.Add(Stab4);
        Stablist.Add(Stab5);
        Stablist.Add(Stab6); 
        MiddleScreenHideSpecialReason();
        UpdateCoreTemperature();
    }

    void Update()
    {
        if (CoreStatus != CoreStatusEnum.Offline.ToString())
        {
            UpdateCoreTemperature();
        }
        StateText.text = CoreStatus.ToLower();
        TempText.text = CoreTemp.ToString();
    }

    public int STABSMoyenne()
    {
        int s = 0;
        int a = 0;
        foreach(STABSLasers stab in Stablist)
        {
            stab.StructuralIntegrity += a;
            s++;
        }
        return a/s;

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
        if (Stab5.StabCheckForCoreVal())
        {
            CoreTempChange += MaxHeatUnitEfficiency * (Stab5.Power / 100f);
        }
        if (Stab6.StabCheckForCoreVal())
        {
            CoreTempChange += MaxHeatUnitEfficiency * (Stab6.Power / 100f);
        }

        // Cooling lasers effect
        if (Stab1.StabCheckForCoreVal())
        {
            CoreTempChange -= MaxCoolingUnitEfficiency * (Stab1.Power / 100f);
        }
        if (Stab2.StabCheckForCoreVal())
        {
            CoreTempChange -= MaxCoolingUnitEfficiency * (Stab2.Power / 100f);
        }

        //Cooling unit effect


        //Power extraction effect


        // Shield effect
        CoreTempChange -= MaxShieldCoolingEfficiency * ((int)MCFS.instance.ShieldIntegrity / 100f);

        // Debug: Log CoreTempChange before damping
        debugTempChange = CoreTempChange;

        // Apply a damping factor to reduce sensitivity
        CoreTempChange *= 0.8f; // Damping factor to smooth abrupt changes

        // Clamp CoreTempChange to prevent extreme values
        //CoreTempChange = Mathf.Clamp(CoreTempChange, -100f, 100f); // Adjust range as needed

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

        }
    }



    public IEnumerator CoreStabilityDecrease()
    {
        yield return new WaitForSeconds(3);
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

    public void CoreHideNormalDisplay()
    {
        foreach (GlobalScreenManager glob in Startup.instance.gsm)
        {
            glob.MakeMemeGoAway();
        }
        CoreWS.gameObject.SetActive(false);
        CoreUS.gameObject.SetActive(false);
        CoreDiag.gameObject.SetActive(false);
        TempText.gameObject.SetActive(false);
        StateText.gameObject.SetActive(false);
    }

    public void CoreDisplayTimer(int minute, int seconds)
    {
        foreach(GlobalScreenManager glob in Startup.instance.gsm)
        {
            glob.MakeMemeGoAway();
        }
        CoreWS.gameObject.SetActive(false);
        CoreUS.gameObject.SetActive(false);
        CoreDiag.gameObject.SetActive(false);
        TempText.gameObject.SetActive(false);
        StateText.gameObject.SetActive(false);
        CoreTR.gameObject.SetActive(true);
        CoreTR.GetComponent<TimeTicking>().StartTimer(minute, seconds, 0000);
    }

    public void MiddleScreenDisplaySpecialReason(string reason, Color reasoncolor, string subtitle, Color subtitlecolor)
    {
        CoreHideNormalDisplay();
        if (CoreTR.GetComponent<TimeTicking>().IsRunning == false)
        {
            CoreTR.gameObject.SetActive(true);
            CoreTR.GetComponent<TimeTicking>().timetext.text = "N/A";
        }
        MiddleSubTextScreen.gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        MiddleUpTextScreen.gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        MiddleUpTextScreen.text = reason;
        LeanTween.value(gameObject, MiddleUpTextScreen.color, reasoncolor, 0.9f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnUpdate((Color c) =>
            {
                MiddleUpTextScreen.color = c;
            });

        
        MiddleSubTextScreen.text = subtitle;
        LeanTween.value(gameObject, MiddleSubTextScreen.color, subtitlecolor, 0.9f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnUpdate((Color c) =>
            {
                MiddleSubTextScreen.color = c;
            });
    }
    public void MiddleScreenHideSpecialReason()
    {
        MiddleSubTextScreen.gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
        MiddleSubTextScreen.text = "";
        MiddleUpTextScreen.gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
        MiddleUpTextScreen.text = "";
        //MiddleScreenDisplaySpecialReason("! SELF-DESTRUCT !", COREManager.instance.LineUnknownColor, "EVACUATE IMMEDIATLY - ALL EVACUATION ZONE EFFECTIVE", COREManager.instance.LineUnknownColor);
    }

    public void MiddleScreenMoveIn()
    {

    }
    public void MiddleScreenMoveOut()
    {
        MiddleSubTextScreen.gameObject.LeanMove(new Vector3(), 1);
    }


    public void ShutdownChecker()
    {
        if (CoreTemp > 6000 && CoreTemp < 8000 && Premeltdown == false && ControlLoss == false && CoreInEvent == false && ReactorGrid.instance.GridSTS == ReactorGrid.GridStatus.ONLINE.ToString() && ForceDisableShutdown == false)
        {
            CanShutdown = false;
            Shutdown.instance.ShutdownCaller();
        }
    }



    public IEnumerator CoreSizeChanger(Vector3 size, int time)
    {
        if (RegenHandler.instance.AppRunning)
        {
            Startup.instance.Core.transform.LeanScale(size, time);
        }
        yield break;
    }



    public void ResetToAllowStartup()
    {
        Startup.instance.StartupButton.canBePressed = true;
        Startup.instance.StartupButton.TriggerUsed = false;
        CanStartup = true;
        CanShutdown = false;
    }

}


public class EventManager : MonoBehaviour
{
    public List<IEnumerator> ListOfEvents;

    public IEnumerator temp;

    public IEnumerator StartupEvent;
    public IEnumerator ShutdownEvent;
    public IEnumerator PremeltEvent;
    public IEnumerator MeltdownEvent;
    public IEnumerator SDV1Event;
    public IEnumerator SDV2Event;
    public IEnumerator OverloadV1Event;
    public IEnumerator FFSD;

    public static EventManager instance;


    private void Awake()
    {
        instance = this;
    }


    public void Start()
    {
        //StartupEvent = Startup.instance.CoreStarup();
        ShutdownEvent = Shutdown.instance.ShutdownStart();

        ListOfEvents.Add(StartupEvent);
        ListOfEvents.Add(ShutdownEvent);
        ListOfEvents.Add(PremeltEvent);
        ListOfEvents.Add(MeltdownEvent);
        ListOfEvents.Add(SDV1Event);
        ListOfEvents.Add(SDV2Event);
        ListOfEvents.Add(OverloadV1Event);
        ListOfEvents.Add(FFSD);
    }

    public void EventKillSwitch(IEnumerator EventToNotKill)
    {
        foreach(IEnumerator e in ListOfEvents)
        {
            if (e != EventToNotKill)
            {
                StopCoroutine(e);
            }
            else
            {
                print("event sparred");
            }
        }
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


public class CorePurgeSYS:MonoBehaviour
{
    public static CorePurgeSYS instance;


    //vars
    public bool CanPurge = true;
    public bool CorePurgeAuth = false;
    public int SystemIntegrity = 100;
    public bool CorePurgeRequested = false;


    private void Awake()
    {
        instance = this;
    }



    public bool PurgeCaller()
    {
        if (CorePurgeAuth && CorePurgeRequested)
        {
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("Core purge inbound...", Color.yellow);
            StartCoroutine(Purge());
            return true;
        }
        if(CorePurgeRequested == false)
        {
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("ERROR: NO CORE PURGE REQUEST FOUND!", Color.red);
            return false;
        }
        return false;
    }


    public void PurgeRegister()
    {
        if (CorePurgeRequested == false && CorePurgeAuth == true)
        {
            CorePurgeRequested = true;
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("Core purge rquested!", Color.green);
        }
    }


    public IEnumerator Purge()
    {


        yield return null;
    }

}