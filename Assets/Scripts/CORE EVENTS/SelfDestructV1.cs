using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructV1 : MonoBehaviour
{

    [HideInInspector]public static SelfDestructV1 instance;

    [Header("Component")]
    public STABSLasers stab1, stab2, stab3, stab4, stab5, stab6;
    public AudioClip SDV1ost;
    public AudioSource AS;




    private void Awake()
    {
        instance = this;
    }


    public void SDV1caller()
    {
        StartCoroutine(SDV1());
    }

    public void SDV1naturalCaller()
    {
        StartCoroutine(Preparation());
    }

    IEnumerator Preparation()
    {
        if (COREManager.instance.CoreState != "OFFLINE")
        {
            COREManager.instance.CoreSizeChanger(new Vector3(0, 0, 0), 0);
        }
        yield break;
    }

    IEnumerator SDV1()
    {
        AS.clip = SDV1ost;
        AS.Play();
        COREManager.instance.CoreDisplayTimer(1, 30);
        FAS.GFAS.WriteAnAnnouncement("Administrator: Patrick.A", "An unknown threat has been reported within the facility. Security AND integrity have been COMPROMISED! Self destruct protocol now in effect. All employee are tasked to EVACUATE TO THEIR SAFEZONE IMMEDIATLY!", 10);
        COREManager.instance.ReactorSysLogsScreen.EntryPoint("!Self-destruct signal received!", COREManager.instance.LineUnknownColor);

        yield return new WaitForSeconds(5);

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("!Core detonation has been scheduled!", COREManager.instance.LineUnknownColor);
        COREManager.instance.ReactorSysLogsScreen.EntryPoint("!Evacuate immediatly!", COREManager.instance.LineUnknownColor);

        yield return new WaitForSeconds(75);



        yield return new WaitForSeconds(10);

        //death.

        yield break;
    }
}
