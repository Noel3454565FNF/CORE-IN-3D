using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using Unity.VisualScripting;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.ShaderKeywordFilter;

public class ForcedEventTriggerUI : Editor
{
    [MenuItem("Tools/Events/testing")]
    static void ULTRAKILL()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("IS PLAYING");
        }
        else
        {
            Debug.LogError("ISN'T PLAYING");
        }
    }

    [MenuItem("Tools/Events/aaah")]
    static void mf()
    {

    }






    //CORE EVENTS
    [MenuItem("Tools/Events/Core/Shutdown/Shutdown")]
    static void Sdown()
    {
        if (EditorApplication.isPlaying)
        {
            Shutdown.instance.ShutdownCaller();
        }
    }
    [MenuItem("Tools/Events/Core/Shutdown/Shutdown Failure")]
    static void ShutdownFailure()
    {
        if (EditorApplication.isPlaying)
        {

        }
    }
    [MenuItem("Tools/Events/Core/Shutdown/Shutdown Success")]
    static void ShutdownSuccess()
    {
        if (EditorApplication.isPlaying)
        {

        }
    }
    [MenuItem("Tools/Events/Core/Stall/InstantStall")]
    static void InstantStall()
    {
        if (EditorApplication.isPlaying)
        {
            Stall.instance.InstantStall();
        }
    }
    [MenuItem("Tools/Events/Core/StartupV2")]
    static void StartupV2F()
    {
        if (EditorApplication.isPlaying)
        {
            StartupV2.instance.StartupV2CALLER();
        }
    }
    [MenuItem("Tools/Events/Core/EMP")]
    static void EMPwave()
    {
        EMP.instance.NewEMP();
    }


    //REACTOR GRID EVENT
    [MenuItem("Tools/Events/ReactorGrid/Power outage")]
    static void b()
    {
        if(EditorApplication.isPlaying)
        {
            ReactorGrid.instance.GridOutageCaller();
            //ReactorGrid.instance.GridOutagefunc();
            Debug.LogError("OUTAGE TRIGGERED");
        }
    }

    //STABS EVENT
    [MenuItem("Tools/Events/ReactorSys/Stabs/Enter Overheat")]
    static void c()
    {
        COREManager.instance.TestOverheat();
    }

    //MCFS EVENT
    [MenuItem("Tools/Events/MCFS/Kys")]
    static void d()
    {
        MCFS.instance.ShieldKYS();
        Debug.LogError("BYE BYE MCFS!");
    }

    //CHAOTIC EVENT
    [MenuItem("Tools/Events/Chaotic/SDV1")]
    static void SDV1()
    {
        SelfDestructV1.instance.SDV1caller();
    }
    [MenuItem("Tools/Events/Chaotic/OverloadV1")]
    static void OverloadV1()
    {

    }

    //SAFEGUARDS SYSTEMS
    [MenuItem("Tools/Events/Core/PowerPurge/PowerPurgeCaller")]
    static void PPC()
    {
        CPS.cps.POWERPURGECALLER();
    }

    //REACTORSYS
    [MenuItem("Tools/Events/ReactorSys/Bootup")]
    static void RSB()
    {
        ReactorSysManager.instance.ReactorSysBootupCALLER();
    }
    [MenuItem("Tools/Events/ReactorSys/Crash")]
    static void RSC()
    {
        ReactorSysManager.instance.EventManagerCaller(ReactorSysManager.EventEnum.CRASH);
    }

    //DEV EVENT
    [MenuItem("Tools/Events/DEV/FFSD")]
    static void FFSD()
    {

    }

    //PARTICLES THINGS
    [MenuItem("Tools/Stuff/Particles/Particles Shockwave")]
    static void ParticlesShockwave()
    {
        Stall.instance.ParticleShockwaveCaller();
    }


    //LIMBO
    [MenuItem("Tools/Limbo/SDV1")]
    static void sdv1Limbo()
    {
        Death.instance.TeleportToLimbo(Death.DeathReason.sd);
    }
    //private void Awake()
    //{
    //    Debug.LogWarning("i can see you.");
    //}

}
