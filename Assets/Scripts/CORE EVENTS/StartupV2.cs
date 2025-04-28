using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupV2 : MonoBehaviour
{
    [Header("Managers & Components")]
    public COREManager CM;


    [Header("Audio")]
    public AudioClip FuelSolutionInjection;
    public AudioClip LasersStartReaction;
    public AudioClip startupv2ost;

    [Header("Particles")]
    public ParticleSystem shieldIDLE;
    public ParticleSystem stab1PART;
    public ParticleSystem stab2PART;

    [Header("Value")]
    public int shieldIDLparticlestarget = 15000;


    private IEnumerator STARTUPV2COUR()
    {
        PlayerController.me.OSTPLAYER(startupv2ost, 0.5f);
        FAS.GFAS.WriteAnAnnouncement("FAS", "Reactor Ignition scheduled, RO are tasked to stop all ongoing maintenance operation immediately", 6);
        CM.ReactorSysLogsScreen.EntryPoint("core ignition signal received", Color.green);

        yield return new WaitForSeconds(11f);

        CM.ReactorSysLogsScreen.EntryPoint("moving stab 1 and 2...", Color.yellow);
        //don't forget to script that btw, take around 9 seconds to move

        yield return new WaitForSeconds(12f);

        CM.ReactorSysLogsScreen.EntryPoint("dispersing fuel...", Color.yellow);
        AudioSource astemp = PlayerController.me.OSTPLAYER(FuelSolutionInjection, 0.2f, "");

        //code fuel dispersion thing from stab 1 and 2
        //i'll do it now dawg dw :thumbsup:
        stab1PART.Play(); CM.Stab1.StabRPMCHANGING(150, 3);
        stab2PART.Play(); CM.Stab2.StabRPMCHANGING(150, 3);
        LeanTween.value(shieldIDLE.emissionRate, shieldIDLparticlestarget, 8)
            .setOnUpdate((float i) =>
            {
                shieldIDLE.emissionRate = i;
            });
        LeanTween.value(MCFS.instance.ShieldIntegrity, 100, 8)
            .setOnUpdate((float i) =>
            {
                MCFS.instance.ShieldIntegrity = Mathf.CeilToInt(i);
            });

        yield return new WaitForSeconds(8f);

        astemp.Stop();
        CM.ReactorSysLogsScreen.EntryPoint("core ready for ignition!", Color.green);

        yield return new WaitForSeconds(3f);

        CM.ReactorSysLogsScreen.EntryPoint("redirecting power to internal systems...", Color.yellow);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1, Negate3roomsName.ALL)); 

        yield return new WaitForSeconds(2f);

        PlayerController.me.OSTPLAYER(LasersStartReaction, 0.3f);
        CM.ReactorSysLogsScreen.EntryPoint("powering up lasers...", Color.yellow);

        yield return new WaitForSeconds(11f);

        shieldIDLE.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        yield return new WaitForSeconds(9f);

        //form reactor shield here

        yield break;
    }


}
