using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premelt : MonoBehaviour
{
    [Header("Component")]
    public static Premelt instance;
    public COREManager CM;
    public LineClone RSYSlogs;
    public MCFS mcfs;
    public CameraFollowAndControl ScreenShake;
    public ShockwaveHandler Shockwave;
    public ScreenFlash WhiteFlash;
    public AudioSource AS;
    public AudioClip PremeltOst;
    public FAS Announcement;
    public ReactorGrid RG;
    public CorePurgeSYS CPSYS;

    [Header("Vars")]
    public int TotalFailureChance = 5;





    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CM = COREManager.instance;
        mcfs = MCFS.instance;
        Shockwave = ShockwaveHandler.GSwH;
        WhiteFlash = ScreenFlash.GSF;
        Announcement = FAS.GFAS;
        RG = ReactorGrid.instance;
        CPSYS = CM.CPSYS;
        RSYSlogs = CM.ReactorSysLogsScreen;
    }

    public void Caller()
    {
        StartCoroutine(PremeltIsHere());
    }



    public IEnumerator PremeltIsHere()
    {
        CM.CoreInEvent = true;
        CM.Premelt = true;
        CM.CoreToDanger();
        mcfs.ShieldToWarning();
        FAS.GFAS.WriteAnAnnouncement("MCFS", "!SYSTEM COMPROMISED - RESTABILIZE IMMEDIATLY!", 5);
        RSYSlogs.EntryPoint("CORE OVERHEATING!", Color.yellow);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 0, Negate3roomsName.CORE_CONTROL_ROOM));
        yield return new WaitForSeconds(7);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1000, 100, Negate3roomsName.CORE_CONTROL_ROOM));
        RSYSlogs.EntryPoint("STAB 1 FAULT!", Color.red);
        RSYSlogs.EntryPoint("STAB 2 FAULT!", Color.red);
        RSYSlogs.EntryPoint("STAB 3 OVERHEATING!", Color.yellow);
        RSYSlogs.EntryPoint("STAB 4 OVERHEATING!", Color.yellow);
        RSYSlogs.EntryPoint("STAB 5 CONNECTION UNSTABLE!", Color.red);
        RSYSlogs.EntryPoint("STAB 6 OVERHEATING!", Color.yellow);
        FAS.GFAS.WriteAnAnnouncement("Administration", "TO ALL CORE OPERATOR, YOU ARE ORDERED TO RESTABILIZE IMMEDIATLY! USAGE OF CONTIGENCY SYSTEMS IS NOW AUTHORIZED.", 5);
        CM.CPSYS.CorePurgeAuth = true;
        yield return new WaitForSeconds(10f);
        RSYSlogs.EntryPoint("REACTOR GRID MALFUNCTION!", Color.red);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 0, Negate3roomsName.CORE_CONTROL_ROOM));
        yield return new WaitForSeconds(6f);
        RSYSlogs.EntryPoint("Checking systems...", Color.white);

        int lol = Random.Range(0, 100);
        StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1000, 0, Negate3roomsName.CORE_CONTROL_ROOM));
        yield return new WaitForSeconds(10f);
        if(lol <= TotalFailureChance)
        {
            StartCoroutine(ShieldDetonation());
        }
        else
        {
            RSYSlogs.EntryPoint("Systems ready for reactor salvation!", Color.green);
            yield return new WaitForSeconds(15f);
            if (CM.CoreTemp < 22000)
            {
                RSYSlogs.EntryPoint("Automatic reactor salvation now in progress...", CM.LineOVERRIDEColor);
                if (CPSYS.PurgeCaller() == true)
                {
                    //Call core purge
                }
                else
                {
                    yield return new WaitForSeconds(3f);
                    RSYSlogs.EntryPoint("CORE PURGE FAILURE!", Color.red);
                    yield return new WaitForSeconds(6f);
                    RSYSlogs.EntryPoint("SHIELD DETONATION IMMINENT!", Color.red);
                    yield return new WaitForSeconds(3f);
                    StartCoroutine(ShieldDetonation());
                }
                yield return null;
            }
        }
        yield return null;
    }



    public IEnumerator ShieldDetonation()
    {
        Debug.LogWarning("oh well... maybe you should not had done that-");
        RSYSlogs.EntryPoint("CONNECTION LOST WITH STAB 4!", CM.LineUnknownColor); CPSYS.CanPurge = false;
        CM.Stab4.StabKys();
        yield return new WaitForSeconds(4f);
        RSYSlogs.EntryPoint("PURGE SYSTEM FAILURE!", CM.LineUnknownColor);
        FAS.GFAS.WriteAnAnnouncement("Administration", "UNABLE TO RESTABILIZE! EVACUATION ORDER NOW IN EFFECT!", 5);
        yield return new WaitForSeconds(2.2f);
        mcfs.ShieldToUnknownThreat();
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        RSYSlogs.EntryPoint("MCFS SYSTEM FAILURE", CM.LineUnknownColor);
        yield return new WaitForSeconds(7);
        RSYSlogs.EntryPoint("SHIELD DETONATION IMMINENT!", Color.red);
        yield return new WaitForSeconds(2.5f);

        yield return new WaitForSeconds(3f);
        mcfs.ShieldKYS();
        CM.Stab1.StabKys();
        CM.Stab2.StabKys();
        CM.Stab3.StabKys();
        //Meltdown.Caller()
        yield return null;
    }
}
