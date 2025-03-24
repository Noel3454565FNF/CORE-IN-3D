using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stall : MonoBehaviour
{
    public static Stall instance;
    private COREManager CM;
    private LightsManager LM;


    private List<STABSLasers> stabs;
    private STABSLasers stab1;
    private STABSLasers stab2;
    private STABSLasers stab3;
    private STABSLasers stab4;
    private STABSLasers stab5;
    private STABSLasers stab6;

    public ParticleSystem StallEffect1;
    public ParticleSystem StallEffect2;


    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        stabs.Add(CM.Stab1);
        stabs.Add(CM.Stab2);
        stabs.Add(CM.Stab3);
        stabs.Add(CM.Stab4);
        //stabs.Add(CM.Stab5);
        //stabs.Add(CM.Stab6);
    }

    public void InstantStall()
    {
        StartCoroutine(InstaStallEvent());
    }
    IEnumerator InstaStallEvent()
    {
        CM.CanUpdateTemp = false; CM.CanShutdown = false; CM.CanStartup = false;
        CM.CoreInEvent = true; CM.CoreAllowGridEvent = false;
        foreach (STABSLasers stab in stabs)
        {
            stab.Laser.gameObject.SetActive(false);
            stab.StabRpmTweenDown(0, 5);
        }
        CM.CoreSizeChanger(new Vector3(0, 0, 0), 4);
        StallEffect1.Play();
        yield return new WaitForSeconds(4);
        StallEffect1.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StallEffect2.Play();
        CM.ReactorSysLogsScreen.EntryPoint("Unattended core stall detected", Color.white);
        yield return new WaitForSeconds(5);
        StallEffect2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        CM.ReactorSysLogsScreen.EntryPoint("Rebooting Reactor Component...", Color.white);
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("Reboot suvccessful! reactor ready for ignition!", Color.green);
        CM.CanStartup = true;
        yield break;
    }


    public void StallE()
    {
        StartCoroutine(StallEvent());
    }
    IEnumerator StallEvent()
    {

        yield break;
    }

}
