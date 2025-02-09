using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;

public class Shutdown : MonoBehaviour
{
public static Shutdown instance;

    //Component
    public COREManager CM;
    public MCFS mcfs;
    public CameraFollowAndControl ScreenShake;
    public ShockwaveHandler Shockwave;
    public ScreenFlash WhiteFlash;
    public AudioSource AS;
    public AudioClip ShutdownOst;
    public FAS Announcement;
    public ReactorGrid RG;

    //Vars things
    [InspectorName("How many Shutdown? ")] public int HowManyShutdown;
    public int FailureChance = 0;
    public int baseChance = 0;
    /// <summary>
    /// things may happend if set to true...
    /// </summary>
    public bool Chaotic = false;
    public bool ForcedChaotic = false;
    public int ChaoticChance = 1;

    /// <summary>
    /// set to true if used in meltdown
    /// </summary>
    public bool MeltShutdown = false;
    public bool MeltShutdownForced = false;
    public bool MeltShutdownForcedFail = false;
    public bool MeltShutdownForcedPass = false;


    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        CM = COREManager.instance;
        mcfs = MCFS.instance;
        Shockwave = ShockwaveHandler.GSwH;
        WhiteFlash = ScreenFlash.GSF;
        Announcement = FAS.GFAS;
        RG = ReactorGrid.instance;
    }

    public void ShutdownCaller()
    {
        StartCoroutine(ShutdownStart());
    }

    IEnumerator ShutdownStart()
    {
        CM.CoreInEvent = true;
        CM.CoreEvent = COREManager.CoreEventEnum.Shutdown.ToString();
        RG.CanGridUpdate = false;
        CM.CanUpdateTemp = false;
        CM.Stab1.StabAdminLock = true; CM.Stab1.CanHeat = false; CM.Stab1.CanGetDamaged = false;
        CM.Stab2.StabAdminLock = true; CM.Stab2.CanHeat = false; CM.Stab2.CanGetDamaged = false;
        CM.Stab3.StabAdminLock = true; CM.Stab3.CanHeat = false; CM.Stab3.CanGetDamaged = false;
        CM.Stab4.StabAdminLock = true; CM.Stab4.CanHeat = false; CM.Stab4.CanGetDamaged = false;
        yield return null;
        Announcement.WriteAnAnnouncement("Mainframe", "Core shutdown requested and inboud. Requester: Core Operation", 3);
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("Core Shutdown Inbound", Color.white);
        yield return new WaitForSeconds(1);


        if (MeltShutdown)
        {
            var chance = 0;

            if (MeltShutdownForced == false)
            {
                chance = UnityEngine.Random.Range(0, 100);
            }
            else
            {
                chance = -1;
            }

            if (baseChance < chance - FailureChance)
            {
                //SUCCESS
                baseChance += 10;
                StartCoroutine(ShutdownSuccess());
            }
            else
            {
                //FAILURE
            }
        }
        else
        {
            StartCoroutine(ShutdownSuccess());
        }
    }



    IEnumerator ShutdownSuccess()
    {
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("Shutdown in progress...", CM.LineAttentionColor);
        yield return new WaitForSeconds(7);
        CM.Stab3.StabRPMCHANGING(80, 5); CM.Stab4.StabRPMCHANGING(80, 3);
        CM.ReactorSysLogsScreen.EntryPoint("Shutting down Power unit...", Color.white);
        yield return new WaitForSeconds(5);
        CM.ReactorSysLogsScreen.EntryPoint("Core stall in progress...", Color.white);
        CM.Stab1.StabRPMCHANGING(CM.Stab1.RPM + 200, 3); CM.Stab2.StabRPMCHANGING(CM.Stab2.RPM + 200, 3);
        yield return new WaitForSeconds(10);
        CM.ReactorSysLogsScreen.EntryPoint("Core stall success!", Color.green);

        Startup.instance.CoreShield.transform.LeanScale(new Vector3(0, 0, 0), 10f); Startup.instance.Core.transform.LeanScale(new Vector3(0, 0, 0), 10f);

        CM.Stab1.StabRPMCHANGING(0, 7); CM.Stab2.StabRPMCHANGING(0, 7); sideshutdown(CM.Stab1, 7000); sideshutdown(CM.Stab2, 7000);
        CM.Stab3.StabRPMCHANGING(0, 5); CM.Stab4.StabRPMCHANGING(0, 3); sideshutdown(CM.Stab3, 3000); sideshutdown(CM.Stab4, 3000);
        yield return new WaitForSeconds(5);
        CM.ReactorSysLogsScreen.EntryPoint("Rebooting ReactorSys...", Color.white);
        yield return new WaitForSeconds(7);
        //CUSTOM EVENT CHANCE HERE!

        if (ForcedChaotic == false)
        {
            Chaotic = ChaoticSetter();
            if (Chaotic == true)
            {
                ChaoticCaller();
            }
            else
            {
                Debug.Log("Nothing will happend dw");
                CM.ReactorSysLogsScreen.EntryPoint("Reboot completed!", Color.green);
                CM.ReactorSysLogsScreen.EntryPoint("Startup is now available!", Color.green);
                CM.CanStartup = true;
                CM.CoreInEvent = false;
            }
        }
        else
        {
            Debug.LogWarning("FORCED CHAOS INBOUND.");
            //FORCED CHAOS INBOUND.
            ChaoticCaller();
        }
    }

    public bool ChaoticSetter()
    {
        int aaaa = UnityEngine.Random.Range(0, 100);

        if (ChaoticChance > aaaa)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task sideshutdown(STABSLasers stab, int timetowait)
    {
        await Task.Delay(timetowait);
        stab.Laser.SetActive(false);
    }



    //CHAOTIC
    public void ChaoticCaller()
    {
        StartCoroutine(ChaoticMain());
    }

    public IEnumerator ChaoticMain()
    {
        Debug.LogWarning("oh uuuh... smt went wrong-");
        CM.ReactorSysLogsScreen.EntryPoint("Rebooting ReactorySys...", CM.LineAttentionColor);
        yield return new WaitForSeconds(5);
        CM.ReactorSysLogsScreen.EntryPoint("UNABLE TO REBOOT.", CM.LineUnknownColor);
        CM.ReactorSysLogsScreen.EntryPoint("ERROR", CM.LineUnknownColor);
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("Contacting Administration...", CM.LineAttentionColor);
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("UNABLE TO CONTACT ADMININSTRATION!", CM.LineWarnColor);
        CM.ReactorSysLogsScreen.EntryPoint("SENDING SOS TO MAINFRAME...", CM.LineAttentionColor);
        yield return new WaitForSeconds(1);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM));
        CM.ReactorSysLogsScreen.EntryPoint("GRID FAULT DETECTED!", Color.red);
        FAS.GFAS.WriteAnAnnouncement("Mainframe", "ReactorSys fault detected! Attempting to terminate the core...", 3);
        CM.ReactorSysLogsScreen.EntryPoint("UNABLE TO GET RESPONCE FROM MAINFRAME!", Color.red);
        WhiteFlash.ScreenFlashF(Color.black, 1, 3, 0.1f, 0.1f);
        yield return new WaitForSeconds(3);
        //CALL UO;
        //oh wait...
        //Startcouroutine(Shutdown.instance.ChaoticMain()) is an UO recreation :skull:
        //i'll make smt origninal later i promise
        //but for now...
        //have fun!
        //and try to save the day!

    }


    //DEV FUNCTIONS
    public void ForceShutdownSuccess()
    {
        StartCoroutine(ShutdownSuccess());
    }
    public void ForceShutdownFailure()
    {
        //StartCoroutine();
    }
    public void ForceShutdownChaotic()
    {
        ForcedChaotic = true;
        StartCoroutine(ShutdownSuccess());
    }

}
