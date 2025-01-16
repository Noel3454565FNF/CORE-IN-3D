using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class StartupFailure : MonoBehaviour
{
    [Header("Stuff")]
    public string sk = "i hidded everything cuz fuck you im so silly >:3";
    private COREManager CM;
    private MCFS mcfs;
    private FAS fas;

    public static StartupFailure instance;


    [Header("STABS")]
    private STABSLasers Stab1;
    private STABSLasers Stab2;
    private STABSLasers Stab3;
    private STABSLasers Stab4;
    private STABSLasers Stab5;
    private STABSLasers Stab6;

    public List<STABSLasers> StabList;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CM = COREManager.instance;
        mcfs = MCFS.instance;
        fas = FAS.GFAS;
        Stab1 = CM.Stab1;
        Stab2 = CM.Stab2;
        Stab3 = CM.Stab3;
        Stab4 = CM.Stab4;
        Stab5 = CM.Stab5;
        Stab6 = CM.Stab6;
        StabList.Add(Stab1); StabList.Add(Stab2); StabList.Add(Stab3); StabList.Add(Stab4); StabList.Add(Stab5); StabList.Add(Stab6);
    }

    public void StartupFailureCaller()
    {
        Task.Run(StartupFailureFunc);
    }


    public async Task StartupFailureFunc()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}




//while (RegenHandler.instance.AppRunning == true)
//        {
//            CM.CoreEvent = "STARTUP";
//            CM.CoreInEvent = true;
//            Stab1.CanGetDamaged = false; Stab1.CanHeat = false; Stab1.CanKys = false;
//            Stab2.CanGetDamaged = false; Stab2.CanHeat = false; Stab2.CanKys = false;
//            Stab3.CanGetDamaged = false; Stab3.CanHeat = false; Stab3.CanKys = false;
//            Stab4.CanGetDamaged = false; Stab4.CanHeat = false; Stab4.CanKys = false;

//            string failsafecodeSTR = string.Empty;
//    float failsafecode = Random.Range(1000, 9999);
//    failsafecode = Mathf.FloorToInt(failsafecode);
//            if (failsafecode > 9999)
//            {
//                failsafecodeSTR = "[ERROR: UNABLE TO DEPLOY E-PROTOCOL ON SERVERS!]";
//            }
//            else
//{
//    failsafecodeSTR = "" + failsafecode;
//}
//Debug.LogError("REACTORSYS DISTRESS SIGNAL RECEIVED. TO ALL SUBLEVEL 3 EMPLOYEE, YOU HAVE TO PREVENT THE CORE IGNITION BEFORE A COMPLETE SYSTEM FAILURE. THE FAILSAFE CODE IS -> " + failsafecode + " <-");

//fas.WriteAnAnnouncement("ReactorSys", "Reactor Core ignition signal received. Ignition is imminent.", 3); CM.MainCoreScreen.MakeMemeGoAway(); CM.MCFSscreen.MakeMemeGoAway(); CM.ReactorSysLogsScreen.EntryPoint("Core ignition requested and approved!", Color.white);
//await Task.Delay(5000);
//Stab1.StabAdminLock = true; Stab2.StabAdminLock = true; Stab3.StabAdminLock = true; Stab4.StabAdminLock = true;


//StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM)); CM.ReactorSysLogsScreen.EntryPoint("Redirecting power to Reactor Grid", Color.white);
//await Task.Delay(3000);
//CM.ReactorSysLogsScreen.EntryPoint("Verifying ReactorSys...", Color.yellow);
//Stab1.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString(); Stab2.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString(); Stab3.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString(); Stab4.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString();
//CM.Stab1.StabRPMCHANGING(200, 10); CM.Stab2.StabRPMCHANGING(200, 10); CM.Stab3.StabRPMCHANGING(1200, 10); CM.Stab4.StabRPMCHANGING(1200, 10); CM.ReactorSysLogsScreen.EntryPoint("STABS MALFUNCTION DETECTED!", CM.LineUnknownColor);
//await Task.Delay(2500);
//int counting = 0;
//StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1, 1000, Negate3roomsName.CORE_CONTROL_ROOM)); CM.ReactorSysLogsScreen.EntryPoint("UNKNOWN REACTOR GRID ERROR!", CM.LineUnknownColor);
//while (StabList.Count < counting)
//{
//    float from = (float)StabList[counting].Power;
//    float time = Random.Range(5, 15);
//    float to = Random.Range(0, 1000);
//    LeanTween.value(from, to, time)
//        .setOnUpdate((float t) =>
//        {
//            StabList[counting].Power = Mathf.FloorToInt(t);
//        });
//}
//await Task.Delay(6000);
//CM.ReactorSysLogsScreen.EntryPoint("ReactorSys Anomaly detected!", Color.red);
//await Task.Delay(0500);
//CM.ReactorSysLogsScreen.EntryPoint("Unable to connect to MainframeSys.", Color.yellow);
//fas.WriteAnAnnouncement("Mainframe", "DANGER. ReactorSys abnormal behaviour detected! attempting to abort Startup Protocol...", 6);
//StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1, 0100, Negate3roomsName.CORE_CONTROL_ROOM));
//await Task.Delay(1500);
//CM.ReactorSysLogsScreen.EntryPoint("Forcing Autonomous Startup Protocol...", CM.LineUnknownColor);
//print("hello there"); CM.ReactorSysLogsScreen.EntryPoint("Powering up stabs...", Color.white);
//break;
//        }

