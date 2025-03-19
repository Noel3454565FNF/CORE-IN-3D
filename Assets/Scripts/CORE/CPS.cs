using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPS : MonoBehaviour
{

    [Header("Component")]
    private COREManager CM;
    private STABSLasers stab1;
    private STABSLasers stab2;
    private STABSLasers stab3;
    private STABSLasers stab4;
    private STABSLasers stab5;
    private STABSLasers stab6;
    private List<STABSLasers> stabs = new List<STABSLasers>();
    private List<STABSLasers> stabsPURGING = new List<STABSLasers>();
    public AudioClip PowerPurgeAudio;
    public AudioSource AudioPlayer;
    public static CPS cps;


    [Header("Value")]
    public bool AllowPurge = false;
    public bool ForcePurgeSucess = false;
    public bool ForcePurgeFailure = false;

    public int PurgeCount = 0;
    public int PurgeFailureChance = 0;
    public int PurgeEfficiency = 10000;



    private void Awake()
    {
        cps = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        CM = COREManager.instance;
        CM.Stab1 = stab1;
        CM.Stab2 = stab2;
        CM.Stab3 = stab3;
        CM.Stab4 = stab4;
        CM.Stab5 = stab5;
        CM.Stab6 = stab6;
        stabs.Add(stab1);
        stabs.Add(stab2);
        stabs.Add(stab3);
        stabs.Add(stab4);
        //stabs.Add(stab5);
        //stabs.Add(stab6);
        AudioPlayer.clip = PowerPurgeAudio;
    }

    
    public void POWERPURGECALLER()
    {

    }

    IEnumerator POWERPURGE()
    {
        var hehe = true;
        
        if (ForcePurgeFailure && hehe)
        {
            hehe = false;
        }

        if (ForcePurgeSucess && hehe)
        {
            hehe = false;
        }

        var luck = Random.Range(0, 100);

        if (hehe)
        {
            if (PurgeFailureChance < luck)
            {
                //SUCCESS
                StartCoroutine(SUCCESS());
            }
            else
            {
                //FAILURE
                StartCoroutine(FAILURE());
            }
        }


        yield return new WaitForSeconds(1);
    }


    IEnumerator SUCCESS()
    {
        yield return new WaitForSeconds(1);
        AudioPlayer.Play();
        CM.CanUpdateTemp = false;
        foreach (STABSLasers stab in stabs)
        {

            if (stab.CanPurge())
            {
                stabsPURGING.Add(stab);
                stab.PowerPurge1.Play();
                stab.PowerPurge2.Play();
            }

        }

    }

    IEnumerator FAILURE()
    {
        yield return new WaitForSeconds(1);
        AudioPlayer.Play();
        CM.CanUpdateTemp = false;
        foreach (STABSLasers stab in stabs)
        {

            if (stab.CanPurge())
            {
                stabsPURGING.Add(stab);
                stab.StabAdminLock = true;
                //stab.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString();
                stab.PowerPurge1.Play();
                stab.PowerPurge2.Play();
            }

        }

        var TempCoreTemp = CM.CoreTemp - PurgeEfficiency;

        LeanTween.value(CM.CoreTemp, TempCoreTemp, 7)
            .setOnUpdate((float t) => {

                CM.CoreTemp = Mathf.CeilToInt(t);

            });

        foreach(var stab in stabsPURGING)
        {
            var structemp = stab.StructuralIntegrity - Random.Range(0, 15);
            var temptime = Random.Range(4, 7);


            LeanTween.value(stab.StructuralIntegrity, structemp, temptime)
                .setOnUpdate((float t) =>
                {
                    stab.StructuralIntegrity = Mathf.CeilToInt(t);
                });
        }

        yield return new WaitForSeconds(7);
        CM.CanUpdateTemp = true;

        foreach(STABSLasers stab in stabsPURGING)
        {
            stab.PowerPurge1.Stop();
            stab.PowerPurge2.Stop();
        }
    }


}
