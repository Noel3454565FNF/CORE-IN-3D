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
        StallEffect1.Stop();
        StallEffect2.Stop();
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
        CM.CanUpdateTemp = false; CM.CanShutdown = false; CM.CanStartup = false; CM.CoreStatus = COREManager.CoreStatusEnum.Stall.ToString(); CM.StateText.color = Color.blue;
        CM.CoreInEvent = true; CM.CoreAllowGridEvent = false;
        stab1.Laser.SetActive(false); stab1.StabRpmTweenDown(0, 5); stab1.StabStatus = STABSLasers.StabStatusEnum.OFFLINE.ToString();
        stab2.Laser.SetActive(false); stab2.StabRpmTweenDown(0, 5); stab2.StabStatus = STABSLasers.StabStatusEnum.OFFLINE.ToString();
        stab3.Laser.SetActive(false); stab3.StabRpmTweenDown(0, 5); stab3.StabStatus = STABSLasers.StabStatusEnum.OFFLINE.ToString();
        stab4.Laser.SetActive(false); stab4.StabRpmTweenDown(0, 5); stab4.StabStatus = STABSLasers.StabStatusEnum.OFFLINE.ToString();
        CM.CoreSizeChanger(new Vector3(0, 0, 0), 4); MCFS.instance.ShieldKYS();
        StallEffect1.Play();
        yield return new WaitForSeconds(4);
        StallEffect1.Stop(true, ParticleSystemStopBehavior.StopEmitting);  StartCoroutine(ParticlesShockwave());
        CM.ReactorSysLogsScreen.EntryPoint("Unattended core stall detected", Color.white);
        yield return new WaitForSeconds(5);
        CM.ReactorSysLogsScreen.EntryPoint("Rebooting Reactor Component...", Color.white);
        yield return new WaitForSeconds(3);
        CM.ReactorSysLogsScreen.EntryPoint("Reboot suvccessful! reactor ready for ignition!", Color.green);
        CM.ResetToAllowStartup();
        yield break;
    }


    public void ParticleShockwaveCaller()
    {
        StartCoroutine(ParticlesShockwave());
    }
    IEnumerator ParticlesShockwave()
    {
        StallEffect2.Play();
        yield return new WaitForSeconds(4);
        StallEffect2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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
