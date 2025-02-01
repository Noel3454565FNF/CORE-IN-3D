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

    //private void Awake()
    //{
    //    Debug.LogWarning("i can see you.");
    //}

}
