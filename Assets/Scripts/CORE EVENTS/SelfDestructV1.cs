using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SelfDestructV1 : MonoBehaviour
{

    [HideInInspector]public static SelfDestructV1 instance;

    [Header("Component")]
    public AudioClip SDV1ost, powerlost;
    public AudioSource AS;
    public STABSLasers stab1;
    public STABSLasers stab2;
    public STABSLasers stab3;
    public STABSLasers stab4;
    public STABSLasers stab5;
    public STABSLasers stab6;
    public List<STABSLasers> stabList;

    [Header("Values")]
    public bool Cancelable = false;
    public bool Canceled = false;




    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }


    public void SDV1caller()
    {
        StartCoroutine(SDV1());
        //FAS.GFAS.WriteAnAnnouncement("Administrator: Anta.C", "An unknown threat has been detected within the facility. Security AND integrity have been COMPROMISED! Self destruct protocol now in effect. All employee are tasked to EVACUATE TO THEIR DESIGNATED SAFEZONE IMMEDIATLY!", 10);
    }

    public void SDV1naturalCaller()
    {
        StartCoroutine(Preparation());
    }

    IEnumerator Preparation()
    {
        COREManager.instance.CanUpdateTemp = false; COREManager.instance.CoreInEvent = true; COREManager.instance.CoreAllowGridEvent = false;
        //if (COREManager.instance.CoreState != "OFFLINE")
        //{
        //    COREManager.instance.CoreSizeChanger(new Vector3(0, 0, 0), 6);
        //    COREManager.instance.CoreState = "OFFLINE";
        //}
        yield break;
    }

    IEnumerator SDV1()
    {
        if (Random.Range(0, 100) <= 5)
        {
            Cancelable = true;
        }
        AudioSource sss = PlayerController.me.OSTPLAYER(SDV1ost, 0.18f, "");
        COREManager.instance.CoreHideNormalDisplay(); COREManager.instance.CoreInEvent = true;
        COREManager.instance.MiddleScreenDisplaySpecialReason("! UNKNWON REACTOR STATUS !", Color.red, "-> ReactorSys detected an imminent threat from the core, contigency systems online <-", Color.blue);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1, Negate3roomsName.CORE_CONTROL_ROOM)); PlayerController.me.OSTPLAYER(powerlost, 0.5f);
        StartCoroutine(Preparation());

        yield return new WaitForSeconds(5f);

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("POWER CONNECTION LOST", Color.red);

        yield return new WaitForSeconds(3f);

        if (Cancelable)
        {
            ReactorSysManager.instance.EventManagerCaller(ReactorSysManager.EventEnum.COMPLETE_CRASH);
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("CONNECTION LOST WITH SERVERS!", COREManager.instance.LineUnknownColor);
            StartCoroutine(crashRecoveryLoop());
        }
        else
        {
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("POWER CONNECTION LOST!", COREManager.instance.LineUnknownColor);
        }

        yield return new WaitForSeconds(22f); //30 seconds past

        foreach(STABSLasers stab in stabList)
        {
            stab.StabRPMCHANGING(0, 5);
            stab.Laser.SetActive(false);

            if (stab.WS == STABSLasers.WhatStab.Stab1)
            {
                stab1.MoveStab(STABSLasers.StabPosition.Maintenance);
            }
            if (stab.WS == STABSLasers.WhatStab.Stab2)
            {

            }
        }

        

        COREManager.instance.MiddleScreenDisplaySpecialReason("! SELF-DESTRUCT !", COREManager.instance.LineUnknownColor, "-> SELF DESTRUCT IN EFFECT, PLEASE EVACUATE TO YOUR DESIGNATED SAFEZONE <-", Color.red);
        COREManager.instance.CoreDisplayTimer(1, 30);

        yield return new WaitForSeconds(60f); //90 seconds past

        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(500, 3, COREManager.instance.LineUnknownColor, 1, Negate3roomsName.ALL));

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("SYSTEMS READY FOR DETONATION", COREManager.instance.LineUnknownColor);
        COREManager.instance.MiddleScreenDisplaySpecialReason("! SELF-DESTRUCT !", COREManager.instance.LineUnknownColor, "-> EVACUATION WINDOW EXPIRED - THANKS YOU FOR YOUR SERVICE <-", COREManager.instance.LineUnknownColor);

        yield return new WaitForSeconds(18f);

        if (Canceled)
        {
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("Connection to servers restored!", Color.green);



            LeanTween.value(sss.volume, 0, 5f)
                .setOnUpdate((float t) =>
                {
                    sss.volume = t;
                })
                .setOnComplete(() =>
                {
                    amoud(sss);
                });

            yield return new WaitForSeconds(5f);


            COREManager.instance.MiddleScreenDisplaySpecialReason("ERROR", Color.red, "ERRORER-ROR-ERRORER-RORERRORE-RROR", Color.red);
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("Self-destruct sequence aborted!", COREManager.instance.LineOVERRIDEColor);

            yield break;
        }
        else
        {
            foreach (STABSLasers stab in stabList)
            {
                if (stab.WS != STABSLasers.WhatStab.Stab1 && stab.WS != STABSLasers.WhatStab.Stab2)
                {
                    PlayerController.me.OSTPLAYER(stab.StabLoudIgnit, 0.3f);
                    stab.StabRPMCHANGING(1000f, 10f);
                }
            }

            yield return new WaitForSeconds(12f);

            StartCoroutine(ScreenFlash.GSF.DeathFlash());

            yield return new WaitForSeconds(10f);

            Death.instance.TeleportToLimbo(Death.DeathReason.sd);

            yield break;
        }
    }



    private IEnumerator crashRecoveryLoop()
    {
        yield return new WaitForSeconds(0.5f);

        if (ReactorSysManager.instance.Status != ReactorSysManager.StatusEnum.CRASH)
        {
            Canceled = true;
        }
        else
        {
            StartCoroutine(crashRecoveryLoop());
        }

        yield break;
    }

    private void amoud(AudioSource sss)
    {
        sss.Stop();
        GameObject.Destroy(sss.gameObject);
    }
}
