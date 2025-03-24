using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool PurgeFailure = false;

    public int PurgeCount = 0;
    public int PurgeFailureChance = 0;
    public int PurgeEfficiency = 15000;



    private void Awake()
    {
        cps = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        CM = COREManager.instance;
        stab1 = CM.Stab1;
        stab2 = CM.Stab2;
        stab3 = CM.Stab3;
        stab4 = CM.Stab4;
        stab5 = CM.Stab5;
        stab6 = CM.Stab6;
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
        StartCoroutine(POWERPURGE());
    }

    IEnumerator POWERPURGE()
    {
        PurgeCount++;
        var hehe = true;
        var AmountOfLaserPurging = 0;

        foreach (STABSLasers stab in stabs)
        {

            if (stab.CanPurge())
            {
                stabsPURGING.Add(stab);
                AmountOfLaserPurging++;
            }

        }




        if (ForcePurgeFailure && hehe)
        {
            hehe = false;
            StartCoroutine(FAILURE());
        }

        if (ForcePurgeSucess && hehe)
        {
            hehe = false;
            StartCoroutine(PURGING());
        }

        var luck = Random.Range(0, 100);

        if (hehe)
        {
            var PFCtemp = PurgeFailureChance - AmountOfLaserPurging;
            if (PFCtemp < luck)
            {
                //SUCCESS
                PurgeFailure = false;
                StartCoroutine(PURGING());

            }
            else
            {
                //FAILURE
                PurgeFailure = true;
                StartCoroutine(PURGING());
            }
        }


        yield return new WaitForSeconds(1);
    }


    IEnumerator FAILURE()
    {
        CM.ReactorSysLogsScreen.EntryPoint("!!!ANOMALY DETECTED IN PURGESYS!!!", Color.red);

        foreach(var stab in stabsPURGING)
        {
            stab.PowerPurge1.startColor = Color.red;
            stab.PowerPurge2.startColor = Color.red;
        }

        var twow = CM.CoreTemp + Random.Range(2000, 15000);
        LeanTween.value(CM.CoreTemp, twow, 4)
            .setOnUpdate((float t) =>
            {
                CM.CoreTemp = Mathf.CeilToInt(t);
            });

        CM.ReactorSysLogsScreen.EntryPoint("ATTEMPTING TO ABORT PURGE SEQUENCE...", Color.yellow);

        foreach (var stab in stabsPURGING)
        {
            var structemp = stab.StructuralIntegrity - Random.Range(3, 25);
            var temptime = 4;


            LeanTween.value(stab.StructuralIntegrity, structemp, temptime)
                .setOnUpdate((float t) =>
                {
                    stab.StructuralIntegrity = Mathf.CeilToInt(t);
                });
        }


        yield return new WaitForSeconds(2);
        CM.ReactorSysLogsScreen.EntryPoint("!!!UNABLE TO ABORT PURGE SEQUENCE!!!", Color.red);
        CM.ReactorSysLogsScreen.EntryPoint("!!!UNKNOWN SYSTEMS E-E6E6e-E6EERROOR-", CM.LineUnknownColor);
        yield return new WaitForSeconds(2);
        var CanTurnReallyBad = Random.Range(0, 100);
        var chaoticnumber = Random.Range(0, 5);
        var othernumber = Random.Range(0, 49);
        var taken = false;

        if (taken = true && CanTurnReallyBad < chaoticnumber)
        {
            taken = true;
            //Chaotic meltdown...
            //safe locked cuz no idea what to do here-
        }
        else if (taken == true && CanTurnReallyBad < othernumber)
        {
            taken = true;
            //Self-destructV1
            FAS.GFAS.WriteAnAnnouncement("Mainframe", "CRITICAL REACTORSYS DAMAGE DETECTED! CONTIGENCY SYSTEMS ONLINE... SELF-DESTRUCT SEQUENCE ENGAGED!", 5);
        }
        else
        {
            //Instant Stall
            CM.ReactorSysLogsScreen.EntryPoint("REACTOR SYSTEMS UNABLE TO MAINTAIN REACTION!", Color.yellow);
            yield return new WaitForSeconds(1.3f);
            CM.ReactorSysLogsScreen.EntryPoint("CORE ST-ST-ST-ST-ST-ST-ALLLLLLLL IMMINENT!", Color.red);
            yield return new WaitForSeconds(4f);
            Stall.instance.InstantStall();
        }
    }

    IEnumerator PURGING()
    {
        yield return new WaitForSeconds(1);
        AudioPlayer.Play();
        CM.CanUpdateTemp = false;
        foreach (STABSLasers stab in stabs)
        {

            if (stab.CanPurge())
            {
                stab.StabAdminLock = true;
                //stab.StabStatus = STABSLasers.StabStatusEnum.ADMINLOCK.ToString();
                stab.PowerPurge1.Play();
                stab.PowerPurge2.Play();
            }

        }

        var TempCoreTemp = 0;

        if (PurgeFailure)
        {
            TempCoreTemp = CM.CoreTemp - 300;
        }
        else
        {
            TempCoreTemp = CM.CoreTemp - Random.Range(500, PurgeEfficiency);
        }

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

        yield return new WaitForSeconds(3f);

        if (PurgeFailure)
        {
            StartCoroutine(FAILURE());
        }
        else
        {

            yield return new WaitForSeconds(4f);

            if (CM.CoreTemp >= 4000)
            {
                CM.ReactorSysLogsScreen.EntryPoint("CORE STALL IMMINENT!", CM.LineWarnColor);
                foreach (STABSLasers stab in stabsPURGING)
                {
                    stab.StabAdminLock = false;
                    stab.PowerPurge1.Stop();
                    stab.PowerPurge2.Stop();
                }
                yield return new WaitForSeconds(4);
                Stall.instance.InstantStall();
                stabsPURGING = null;
                yield break;
            }
            else
            {
                CM.ReactorSysLogsScreen.EntryPoint("CORE PURGE SUCCESS!", Color.green);
                foreach (STABSLasers stab in stabsPURGING)
                {
                    stab.PowerPurge1.Stop();
                    stab.PowerPurge2.Stop();
                }

                if (PurgeCount <= 4 && PurgeCount > 5)
                {
                    yield return new WaitForSeconds(2);
                    CM.ReactorSysLogsScreen.EntryPoint("PURGE SYSTEM DAMAGED!", Color.yellow);
                    yield return new WaitForSeconds(1);
                    CM.ReactorSysLogsScreen.EntryPoint("CORE SHUTDOWN REQUIRED FOR MAINTENANCE OPERATION!", Color.yellow);
                    yield return new WaitForSeconds(1);
                    CM.ReactorSysLogsScreen.EntryPoint("PLEASE AVOID USING THIS CONTIGENCY SYSTEM!", Color.yellow);
                }
                if (PurgeCount <= 5)
                {
                    yield return new WaitForSeconds(2);
                    CM.ReactorSysLogsScreen.EntryPoint("!!!PURGE SYSTEM DAMAGED!!!", Color.red);
                    yield return new WaitForSeconds(1);
                    CM.ReactorSysLogsScreen.EntryPoint("!!!LASERS DAMAGE IMPORTANT!!!", Color.red);
                    yield return new WaitForSeconds(1);
                    CM.ReactorSysLogsScreen.EntryPoint("Attempting to repair systems...", Color.yellow);
                    var repairchance = Random.Range(40, 100);
                    var randomnumber = Random.Range(0, 100);

                    yield return new WaitForSeconds(3f);

                    if (randomnumber < repairchance)
                    {
                        //REPAIR SUCCESSFUL
                        CM.ReactorSysLogsScreen.EntryPoint("SYSTEMS REPAIR SUCCESSFUL!", Color.green);
                    }
                    else
                    {
                        //bro you are so cooked :skull:
                        //sdv1.instance.RouletteRusse(90);
                        stabsPURGING = null;
                        yield break;
                    }
                }

                CM.CanUpdateTemp = true;

                foreach (STABSLasers stab in stabsPURGING)
                {
                    stab.StabAdminLock = false;
                }
            }
            stabsPURGING = null;
            PurgeFailureChance = PurgeFailureChance + Random.Range(1, 5);
            if (PurgeEfficiency > 500)
            {
                var TempPurgeEfficiency = PurgeEfficiency - Random.Range(0, PurgeEfficiency);

                if (TempPurgeEfficiency < 500)
                {
                    PurgeEfficiency = 500;
                }
                else
                {
                    PurgeEfficiency = TempPurgeEfficiency;
                }
            }
        }
    }


}
