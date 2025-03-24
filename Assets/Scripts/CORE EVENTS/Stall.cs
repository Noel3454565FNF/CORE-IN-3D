using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stall : MonoBehaviour
{
    public static Stall instance;
    public COREManager CM;
    private LightsManager LM;


    public List<STABSLasers> stabs;
    public STABSLasers stab1;
    public STABSLasers stab2;
    public STABSLasers stab3;
    public STABSLasers stab4;
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
        //stabs.Add(CM.Stab5);
        //stabs.Add(CM.Stab6);
    }

    IEnumerator init()
    {
        yield return new WaitForSeconds(1);
        stabs.Add(stab1);
        stabs.Add(stab2);
        stabs.Add(stab3);
        stabs.Add(stab4);
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
        CM.ReactorSysLogsScreen.EntryPoint("Unattended core stall detected", Color.white);
        yield return new WaitForSeconds(5);
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
