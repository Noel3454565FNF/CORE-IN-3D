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
        PlayerController.me.OSTPLAYER(SDV1ost, 0.18f);
        COREManager.instance.CoreHideNormalDisplay(); 
        COREManager.instance.MiddleScreenDisplaySpecialReason("! UNKNWON REACTOR STATUS !", Color.red, "-> ReactorSys detected an imminent threat from the core, contigency systems online <-", Color.blue);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM)); PlayerController.me.OSTPLAYER(powerlost, 0.5f);
        StartCoroutine(Preparation());

        yield return new WaitForSeconds(5f);

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("UNABLE TO CONNECT WITH REACTOR SYSTEMS!", COREManager.instance.LineUnknownColor);

        yield return new WaitForSeconds(3f);

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("POWER CONNECTION LOST!", Color.red);

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

        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1, 3, COREManager.instance.LineUnknownColor, 1, Negate3roomsName.ALL));

        COREManager.instance.ReactorSysLogsScreen.EntryPoint("ASDS READY", COREManager.instance.LineUnknownColor);
        COREManager.instance.MiddleScreenDisplaySpecialReason("! SELF-DESTRUCT !", COREManager.instance.LineUnknownColor, "-> EVACUATION WINDOW EXPIRED - THANKS YOU FOR YOUR SERVICE <-", COREManager.instance.LineUnknownColor);

        yield return new WaitForSeconds(18f);

        foreach(STABSLasers stab in stabList)
        {
            stab.
        }

        yield return new WaitForSeconds(12f);

        StartCoroutine(ScreenFlash.GSF.DeathFlash());

        yield return new WaitForSeconds(10f);

        Death.instance.TeleportToLimbo(Death.DeathReason.sd);

        yield break;
    }
}
