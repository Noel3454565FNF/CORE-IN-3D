using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class Startup : MonoBehaviour
{
    [Header("Stuff")]
    public COREManager CM;
    private Utility utility;
    public AudioSource audioSource;
    public ShockwaveHandler l;
    public FAS sk;
    public bool InProgress = false;

    [Header("Plr")]
    public CameraFollowAndControl plrCAM;

    [Header("Stabs")]
    public STABSLasers Stab1;
    public STABSLasers Stab2;
    public STABSLasers Stab3;
    public STABSLasers Stab4;

    [Header("Core")]
    public MeshRenderer Core;
    public MeshRenderer CoreShield;

    public Vector3 CoreSize;
    public Vector3 CoreShieldSize;

    [Header("Lights")]
    public Light light;

    [Header("Audios")]
    public AudioClip StartupThingi;
    public AudioClip lightsSound;

    [Header("Debug")]
    public bool forcedFailure;
    public bool forcedSuccess;
    public RegenHandler RH;

    public void Start()
    {
        utility = new Utility();
        Test();
    }

    public async Task Test()
    {
        //await Task.Delay(3000);
        //var l = ShockwaveHandler.GSwH;
    }

    public void CoreStartupCaller()
    {
        print("startup invoked");
        if (InProgress == false)
        {
            bool choosenOne = false;
            InProgress = true;
            if (forcedFailure == true && choosenOne == false)
            {
                choosenOne = true;
                StartupFailure.instance.StartupFailureFunc();
            }
            else if (forcedSuccess == true && choosenOne == false)
            {
                choosenOne = true;
                CoreStarup();
            }
            else if (choosenOne == false)
            {
                    choosenOne = true;
                    CoreStarup();
            }
        }
    }

    public async Task CoreStarup()
    {
        print("hey baby");
        print(RegenHandler.instance.AppRunning);
        while (RH.AppRunning == true)
        {
            COREManager.instance.CoreInEvent = true;
            Stab1.CanGetDamaged = false; Stab1.CanHeat = false;
            Stab2.CanGetDamaged = false; Stab2.CanHeat = false;
            Stab3.CanGetDamaged = false; Stab3.CanHeat = false;
            Stab4.CanGetDamaged = false; Stab4.CanHeat = false;



            sk.WriteAnAnnouncement("ReactorSys", "Reactor Core ignition signal received. Ignition is imminent.", 3); CM.MainCoreScreen.MakeMemeGoAway(); CM.MCFSscreen.MakeMemeGoAway(); CM.ReactorSysLogsScreen.EntryPoint("Core ignition requested and approved!", Color.white);
            await Task.Delay(5000);
            Stab1.StabAdminLock = true; Stab2.StabAdminLock = true; Stab3.StabAdminLock = true; Stab4.StabAdminLock = true;


            StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM)); CM.ReactorSysLogsScreen.EntryPoint("Redirecting power to Reactor Grid", Color.white);

            CM.CoreEvent = "STARTUP";
            print("hello there");
            audioSource.clip = StartupThingi;
            audioSource.Play();
            print("hello there"); CM.ReactorSysLogsScreen.EntryPoint("Powering up stabs...", Color.white);
            Task.Run(Stab1.StabStart);
            Task.Run(Stab2.StabStart);
            Task.Run(Stab3.StabStart);
            Task.Run(Stab4.StabStart);
            print("hello there");
            await Task.Delay(21000);

            ScreenFlash.GSF.ScreenFlashF(new Color(255f, 255f, 255f, 0.7f), 0.3f, 1);

            Stab1.Laser.gameObject.SetActive(true); Stab2.Laser.gameObject.SetActive(true); Stab3.Laser.gameObject.SetActive(true); Stab4.Laser.gameObject.SetActive(true); 

            plrCAM.TriggerScreenShake(5f, 4f);

            ShockwaveHandler.GSwH.ShockWaveBuilder(l.DefaultGameObject, l.DefaultMaterial, new Vector3(500, 500, 500), 2f, Core.gameObject.transform.position);
            CoreShield.gameObject.transform.LeanScale(CoreShieldSize, 5f); MCFS.instance.ShieldCreation(100, 5); COREManager.instance.ReactorSysLogsScreen.EntryPoint("Core shield formation...", Color.white);
            Core.gameObject.transform.LeanScale(CoreSize, 5f); CM.LeantweenTemp(7500f, 5f); CM.ReactorSysLogsScreen.EntryPoint("Core formation...", Color.white);
            await Task.Delay(9000); CM.ReactorSysLogsScreen.EntryPoint("Core ignition successful!", Color.green);

            StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1, 1000, Negate3roomsName.CORE_CONTROL_ROOM));
            CM.COREConstantStateChecker();
            sk.WriteAnAnnouncement("ReactorSys", "Reactor Core ignition success. now switching to manual control.", 5); CM.ReactorSysLogsScreen.EntryPoint("Core manual control unlocked", Color.green);

            CM.CoreStatus = "ONLINE"; CM.CoreToOnline(); MCFS.instance.ShieldToOnline(); MCFS.instance.ShieldStatus = MCFS.ShieldStatusEnum.Online.ToString(); 
            MCFS.instance.ShieldStatus = MCFS.ShieldStatusEnum.Online.ToString();
            CM.CoreEvent = "none";
            COREManager.instance.CoreInEvent = false;
            CM.CAH = 20;
            CM.changeSpeedCoreInfluence = 1;
            CM.CoreTempChange = 1;
            Stab1.StabRPMCHANGING(250, 7f); Stab2.StabRPMCHANGING(250, 7f); Stab3.StabRPMCHANGING(250, 7f); Stab4.StabRPMCHANGING(250, 7f);
            Stab1.CanGetDamaged = true; Stab1.CanHeat = true; Stab1.CanHeat = true; Stab1.StabAdminLock = false;
            Stab2.CanGetDamaged = true; Stab2.CanHeat = true; Stab2.CanHeat = true; Stab2.StabAdminLock = false;
            Stab3.CanGetDamaged = true; Stab3.CanHeat = true; Stab3.CanHeat = true; Stab3.StabAdminLock = false;
            Stab4.CanGetDamaged = true; Stab4.CanHeat = true; Stab4.CanHeat = true; Stab4.StabAdminLock = false;
            Stab1.Power = 50; Stab2.Power = 50; Stab3.Power = 25; Stab4.Power = 25;
            Stab1.CoolantInput = 22; Stab2.CoolantInput = 22; Stab3.CoolantInput = 22; Stab4.CoolantInput = 22;
            CM.CanUpdateTemp = true;
            CM.CoreAllowGridEvent = true;
            break;
        }
    }
}
