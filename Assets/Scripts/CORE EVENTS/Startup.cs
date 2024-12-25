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
        CoreStarup();
    }

    public async Task CoreStarup()
    {
        Stab1.CanGetDamaged = false; Stab1.CanHeat = false; Stab1.CanHeat = false;
        Stab2.CanGetDamaged = false; Stab2.CanHeat = false; Stab2.CanHeat = false;
        Stab3.CanGetDamaged = false; Stab3.CanHeat = false; Stab3.CanHeat = false;
        Stab4.CanGetDamaged = false; Stab4.CanHeat = false; Stab4.CanHeat = false;



        sk.WriteAnAnnouncement("ReactorSys", "Reactor Core ignition signal received. Ignition is imminent.", 3);
        await Task.Delay(5000);
        Stab1.StabAdminLock = true; Stab2.StabAdminLock = true; Stab3.StabAdminLock = true; Stab4.StabAdminLock = true;


        LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM);

        CM.CoreEvent = "STARTUP";
        print("hello there");
        audioSource.clip = StartupThingi;
        audioSource.Play();
        print("hello there");
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
        CoreShield.gameObject.transform.LeanScale(CoreShieldSize, 5f);
        Core.gameObject.transform.LeanScale(CoreSize, 5f);
        await Task.Delay(9000);

        LightsManager.GLM.LevelNeg3LightsControl(1, 1000, Negate3roomsName.CORE_CONTROL_ROOM);

        sk.WriteAnAnnouncement("ReactorSys", "Reactor Core ignition success. now switching to manual control.", 5);

        CM.CoreStatus = "ONLINE";
        CM.CoreEvent = "none";
        CM.CAH = 70;
        CM.changeSpeedCoreInfluence = 1;
        CM.CoreTempChange = 1;
        Stab1.StabRpmTweenDown(250, 35); Stab2.StabRpmTweenDown(250, 35); Stab3.StabRpmTweenDown(250, 35); Stab4.StabRpmTweenDown(250, 35);
        Stab1.CanGetDamaged = true; Stab1.CanHeat = true; Stab1.CanHeat = true; Stab1.StabAdminLock = false;
        Stab2.CanGetDamaged = true; Stab2.CanHeat = true; Stab2.CanHeat = true; Stab2.StabAdminLock = false;
        Stab3.CanGetDamaged = true; Stab3.CanHeat = true; Stab3.CanHeat = true; Stab3.StabAdminLock = false;
        Stab4.CanGetDamaged = true; Stab4.CanHeat = true; Stab4.CanHeat = true; Stab4.StabAdminLock = false;
        Stab1.Power = 50; Stab2.Power = 50; Stab3.Power = 25; Stab4.Power = 25;
        Stab1.CoolantInput = 20; Stab2.CoolantInput = 20; Stab3.CoolantInput = 20; Stab4.CoolantInput = 20;



    }

}
