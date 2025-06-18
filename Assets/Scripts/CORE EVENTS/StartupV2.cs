using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupV2 : MonoBehaviour
{
    [Header("Managers & Components")]
    public COREManager CM;
    public static StartupV2 instance;
    public GameObject COREgm;
    //public Animation ShieldanimPlayer;
    public Animator ShieldanimPlayer;


    [Header("Audio")]
    public AudioClip FuelSolutionInjection;
    public AudioClip LasersStartReaction;
    public AudioClip startupv2ost;

    [Header("Particles")]
    public ParticleSystem shieldIDLE;
    public ParticleSystem stab1PART;
    public ParticleSystem stab2PART;

    public ParticleSystem ConstantSteam;
    public ParticleSystem boomPart;


    [Header("Value")]
    public int shieldIDLparticlestarget = 15000;

    [Header("anim files")]
    public AnimationClip shieldspawn;




    public void Awake()
    {
        instance = this;
        CM = COREManager.instance;
        //COREgm = CM.COREMeshRenderer.gameObject;
    }

    public void StartupV2CALLER()
    {
        StartCoroutine(STARTUPV2COUR());
    }
    private IEnumerator STARTUPV2COUR()
    {
       Startup.instance.makememegoaway();
       AudioSource osting = PlayerController.me.OSTPLAYER(startupv2ost, 0.7f, "");

        yield return new WaitForSeconds(1f);

        FAS.GFAS.WriteAnAnnouncement("FAS", "Reactor Ignition scheduled, RO are tasked to stop all ongoing maintenance operation immediately", 10);
        CM.ReactorSysLogsScreen.EntryPoint("core ignition signal received", Color.green);

        yield return new WaitForSeconds(11f);

        CM.ReactorSysLogsScreen.EntryPoint("moving stab 1 and 2...", Color.yellow);
        //don't forget to script that btw, take around 9 seconds to move

        yield return new WaitForSeconds(12f);

        CM.ReactorSysLogsScreen.EntryPoint("dispersing fuel...", Color.yellow);
        AudioSource astemp = PlayerController.me.OSTPLAYER(FuelSolutionInjection, 0.2f, "");
        astemp.loop = true;

        //code fuel dispersion thing from stab 1 and 2
        //i'll do it now dawg dw :thumbsup:
        stab1PART.gameObject.SetActive(true); stab1PART.Play(); CM.Stab1.StabRPMCHANGING(150, 3);
        stab2PART.gameObject.SetActive(true); stab2PART.Play(); CM.Stab2.StabRPMCHANGING(150, 3);
        shieldIDLE.gameObject.SetActive(true);
        LeanTween.value(MCFS.instance.ShieldIntegrity, 100, 8)
            .setOnUpdate((float i) =>
            {
                MCFS.instance.ShieldIntegrity = Mathf.CeilToInt(i);
            });

        yield return new WaitForSeconds(1f);

        LeanTween.value(shieldIDLE.emissionRate, shieldIDLparticlestarget, 7)
            .setOnUpdate((float i) =>
            {
                shieldIDLE.emissionRate = i;
            });


        yield return new WaitForSeconds(7f);

        astemp.Stop();
        GameObject.Destroy(astemp.gameObject);
        stab1PART.Stop(); stab2PART.Stop();
        CM.ReactorSysLogsScreen.EntryPoint("core ready for ignition!", Color.green);

        yield return new WaitForSeconds(3f);

        CM.ReactorSysLogsScreen.EntryPoint("redirecting power to internal systems...", Color.yellow);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1, Negate3roomsName.ALL)); 

        yield return new WaitForSeconds(2f);

        PlayerController.me.OSTPLAYER(LasersStartReaction, 0.3f);
        CM.ReactorSysLogsScreen.EntryPoint("powering up lasers...", Color.yellow);
        CM.Stab1.StabRPMCHANGING(900, 4); CM.Stab1.RegisterRotorBlink(Color.cyan, 0.7f, 1f);
        CM.Stab2.StabRPMCHANGING(900, 4); CM.Stab2.RegisterRotorBlink(Color.cyan, 0.7f, 1f);
        CM.Stab3.StabRPMCHANGING(750, 7); CM.Stab3.RegisterRotorBlink(Color.yellow, 1f, 0.9f);
        CM.Stab4.StabRPMCHANGING(750, 7); CM.Stab4.RegisterRotorBlink(Color.yellow, 1f, 0.9f);
        CM.Stab5.StabRPMCHANGING(750, 7); CM.Stab5.RegisterRotorBlink(Color.yellow, 1f, 0.9f);
        CM.Stab6.StabRPMCHANGING(750, 7); CM.Stab6.RegisterRotorBlink(Color.yellow, 1f, 0.9f);



        yield return new WaitForSeconds(7.5f);

        CM.ReactorSysLogsScreen.EntryPoint("firing...", Color.yellow);

        CM.LeantweenTemp(12000, 6.5f);

        CM.Stab1.Laser.SetActive(true); CM.Stab2.Laser.SetActive(true); CM.Stab3.Laser.SetActive(true); CM.Stab4.Laser.SetActive(true); CM.Stab5.Laser.SetActive(true); CM.Stab6.Laser.SetActive(true);
        shieldIDLE.startSpeed = -3;
        ConstantSteam.gameObject.SetActive(true); ConstantSteam.Play();
        boomPart.gameObject.SetActive(true); boomPart.Play();

        
        CM.COREMeshRenderer.gameObject.transform.LeanScale(new Vector3(8, 8, 8), 9f)
            .setOnUpdate((Vector3 v3) =>
            {
                CM.COREMeshRenderer.gameObject.transform.localScale = v3;
            });


        yield return new WaitForSeconds(5f);

        shieldIDLE.Stop(false, ParticleSystemStopBehavior.StopEmitting); boomPart.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        //make shield here
        CM.ReactorSysLogsScreen.EntryPoint("deploying main shielding...", Color.yellow);


        yield return new WaitForSeconds(4f);
        CM.ReactorSysLogsScreen.EntryPoint("no shield error detected", Color.white);
        CM.Stab1.StabRPMCHANGING(225f, 4f); 
        CM.Stab2.StabRPMCHANGING(225, 4);
        CM.Stab3.StabRPMCHANGING(200, 4); CM.Stab3.blinkMANAGER.KILL(); CM.Stab3.Rotor.GetComponent<MeshRenderer>().material.color = Color.white;
        CM.Stab4.StabRPMCHANGING(200, 4); CM.Stab4.blinkMANAGER.KILL(); CM.Stab4.Rotor.GetComponent<MeshRenderer>().material.color = Color.white;
        CM.Stab5.StabRPMCHANGING(200, 4); CM.Stab5.blinkMANAGER.KILL(); CM.Stab5.Rotor.GetComponent<MeshRenderer>().material.color = Color.white;
        CM.Stab6.StabRPMCHANGING(200, 4); CM.Stab6.blinkMANAGER.KILL(); CM.Stab6.Rotor.GetComponent<MeshRenderer>().material.color = Color.white;

        yield return new WaitForSeconds(3f);

        LeanTween.value(osting.volume, 0f, 5f)
            .setEaseInOutCirc()
            .setOnUpdate((float f) =>
            {
                osting.volume = f;
            })
            .setOnComplete(Action =>
            {
                print("its over.");
                osting.Stop();
                GameObject.Destroy(osting.gameObject);
            });

        CM.ReactorSysLogsScreen.EntryPoint("CORE IGNITION WAS SUCCESSFUL!", Color.green);

        yield return new WaitForSeconds(1f);

        CM.ReactorSysLogsScreen.EntryPoint("MANUAL CONTROL AUTHORIZED!", Color.green);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(500, 1, Negate3roomsName.ALL));
        CM.Stab1.StabRPMCHANGING(225, 4);
        CM.Stab2.StabRPMCHANGING(225, 4);
        CM.Stab3.StabRPMCHANGING(200, 4);
        CM.Stab4.StabRPMCHANGING(200, 4);
        CM.Stab5.StabRPMCHANGING(200, 4);
        CM.Stab6.StabRPMCHANGING(200, 4);

        Startup.instance.corestartstats();

        yield break;
    }


}
