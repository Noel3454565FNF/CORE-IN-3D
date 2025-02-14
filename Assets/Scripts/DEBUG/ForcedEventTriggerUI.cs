using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using Unity.VisualScripting;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;

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
    public void ShutdownSuccess()
    {
        if (EditorApplication.isPlaying)
        {

        }
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

    }
    [MenuItem("Tools/Events/Chaotic/OverloadV1")]
    static void OverloadV1()
    {

    }

    //DEV EVENT
    [MenuItem("Tools/Events/DEV/FFSD")]
    static void FFSD()
    {

    }
    //private void Awake()
    //{
    //    Debug.LogWarning("i can see you.");
    //}

}
