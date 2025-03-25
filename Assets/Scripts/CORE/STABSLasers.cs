using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UIElements;

public class STABSLasers : MonoBehaviour
{
    [Header("Component")]
    public GameObject[] GMstabsParts;
    public MeshRenderer[] MRstabsParts;
    [HideInInspector] public GameObject Rotor;
    public COREManager Cm;
    public ParticleSystem PSStab;
    public GameObject Laser;
    public AudioSource ASSTAB;
    public GameObject BuildUp;


    [Header("Particles things")]
    public ParticleSystem PowerPurge1;
    public ParticleSystem PowerPurge2;

    public enum StabStatusEnum
    {
        ONLINE,
        OFFLINE,
        ERROR,
        DESTROYED,
        OVERLOAD,
        OVERCLOCK,
        ADMINLOCK
    };
    
    [HideInInspector] public int LTColorCurTween;


    [Header("Vars")]
    /// <summary>
    /// The real one btw, use StabStatusEnum;
    /// </summary>
    public string StabStatus = StabStatusEnum.OFFLINE.ToString();
    public string PendingEvent = "none";
    public float StabTemp = 26;
    public int RPM = 0;
    public int Power = 0;
    public int CoolantInput = 0;
    public int StructuralIntegrity = 100;
    public bool CanGetDamaged = true;
    public bool CanHeat = true; public bool canCool = true;
    public bool CanKys = true;
    public bool CurrChange = false;
    public string CurrPowerChangeDir = "none";
    public string CurrCoolingChangeDir = "none";
    public bool CanRotate = false;
    public bool CanUsePower = true;
    public bool CanStart = true;


    public enum WhatStab
    {
        Stab1,
        Stab2,
        Stab3,
        Stab4,
        Stab5,
        Stab6,
    }
    /// <summary>
    /// Stab name
    /// </summary>
    public WhatStab WS = new WhatStab();

    [Header("Warning")]
    public bool LowTempWarning = false;
    public bool HighTempWarning = false;
    public bool StabOverHeat = false;
    public bool LowRPM = false;
    public bool HighRPM = false;
    public bool RotorOverLoad = false;
    public bool LowFuel = false;
    public bool LowIntegrity = false;
    public bool EBrakeActive = false;
    public bool EOverloadActive = false;
    public bool StabTrip = false;
    public bool EMode = false;
    public bool StabDamaged = false;
    public bool StabERROR = false;
    public bool StabAdminLock = false;


    [Header("Color")]
    public Color OverHeatCol = Color.red;
    public Color NormalCol = Color.white;
    public Color FreezeCol = Color.blue;


    [HideInInspector] public IEnumerator stabtempcheckvar;
    [HideInInspector] public IEnumerator stabintcheckvar;

    [Header("Optional Vars")]
    public Vector3 rotationAxis = Vector3.up;
    public int STABStatupEventID;
    private Utility WW;
    [HideInInspector] public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Color WelcomeCurrColorOverheat;

    public int oldRPM = 0;
    public int oldLASERPOWER = 0;


    [Header("Audio")]
    public AudioClip StabDie;
    public AudioClip StabTogetherIgnit;
    public AudioClip StabSoloIgnit;

    [Header("Screen things")]
    public UnityEngine.UI.Image stabrotorimg;
    public UnityEngine.UI.Image stablaserimg;
    public UnityEngine.UI.Image stabsstructuralpillarimg;

    public TextMeshProUGUI temperature;
    public TextMeshProUGUI rpm;
    public TextMeshProUGUI cooling;
    public TextMeshProUGUI stabInput;
    public TextMeshProUGUI strucINT;
    public TextMeshProUGUI globalStatus;



    void Start()
    {
        PowerPurge1.Stop();
        PowerPurge2.Stop();
        Cm = COREManager.instance;
        ASSTAB = gameObject.AddComponent<AudioSource>();
        ASSTAB.playOnAwake = false;
        ASSTAB.volume = 1;

        WW = new Utility();
        StartCoroutine(STABTEMPCHECK());
        StartCoroutine(STABINTCHECK());
        var lol = 0;
        foreach (GameObject gm in GMstabsParts)
        {
            MRstabsParts[lol] = gm.GetComponent<MeshRenderer>();
            lol++;
            if (gm.name == "Rotor")
            {
                Rotor = gm;
            }
        }
    }

    void Update()
    {
        temperature.text = Mathf.FloorToInt(StabTemp) + "C°";
        rpm.text = RPM.ToString();
        stabInput.text = Power.ToString() + "%";
        cooling.text = CoolantInput.ToString() + "%";
        strucINT.text = StructuralIntegrity.ToString() + "%";
    }


    IEnumerator STABTEMPCHECK()
    {
        float TempTar = 0;
        float TempTar2 = 0;
        float TempTar3 = 0;
        float TempTarA = 0;
        if (StabStatus == "ONLINE")
        {
            TempTar = RPM / 40f;
            if (canCool)
            {
                TempTar2 = -CoolantInput / 2.5f;
            }
            TempTar3 = Power / 10f;
            TempTarA = TempTar + TempTar3;
            TempTarA += TempTar2;
            if (CanHeat) { StabTemp += TempTarA;}
        }

        if (StabTemp < 0 && canCool)
        {
            canCool = false;
            StartCoroutine(CanCoolCooldownThing());
        }

        if (StabTemp >= 300 && HighTempWarning == false)
        {
            HighTempWarning = true;
            temperature.color = Color.yellow;
        }
        if (StabTemp < 300 && HighTempWarning)
        {
            HighTempWarning = false;
            temperature.color = Color.white;
        }

        if (StabTemp >= 400 && StabOverHeat == false)
        {
            StabOverHeat = true;
            StartCoroutine(EnterOverheat());
            temperature.color = Color.red;
        }
        if (StabTemp < 400 && StabOverHeat)
        {
            StabOverHeat = false;
            StartCoroutine(ExitOverheat());
            temperature.color = Color.yellow;
        }

        if (RPM >= 370 && HighRPM == false)
        {
            HighRPM = true;
            rpm.color = Color.yellow;
        }
        if (RPM < 370 && HighRPM)
        {
            HighRPM = false;
            rpm.color = Color.white;
        }

        if (RPM >= 420 && RotorOverLoad == false)
        {
            RotorOverLoad = true;
            rpm.color = Color.red;
        }
        if (RPM < 420 && RotorOverLoad)
        {
            RotorOverLoad = false;
            rpm.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);
        if (globalStatus.ToString().ToLower() != StabStatus.ToString().ToLower())
        {
            globalStatus.text = StabStatus.ToString().ToUpper();
        }
        StartCoroutine(STABTEMPCHECK());
    }


    public IEnumerator CanCoolCooldownThing()
    {
        yield return null;
        canCool = false;
        StructuralIntegrity = UnityEngine.Random.Range(1, 3);
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        canCool = true;
    }


    public void ROF()
    {
        // Cache the Rotor MeshRenderer and Material
        var rotorRenderer = Rotor.gameObject.GetComponent<MeshRenderer>();
        if (rotorRenderer == null)
        {
            Debug.LogError("Rotor does not have a MeshRenderer component.");
            return;
        }

        var rotorMaterial = rotorRenderer.material;
        if (StabOverHeat)
        {
            // Play particle system for overheating
            if (PSStab != null)
            {
                PSStab.Play();
            }
            else
            {
                Debug.LogWarning("PSStab is not assigned.");
            }

            // Animate emission to a strong red glow over 10 seconds with an easeOutQuad curve
            LTColorCurTween = LeanTween.value(Rotor, 0f, 1f, 10f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnUpdate((float t) =>
                {
                    WelcomeCurrColorOverheat = Color.Lerp(rotorMaterial.color, Color.red, t);
                    rotorMaterial.SetColor("_EmissionColor", WelcomeCurrColorOverheat * 2f); // Adjust intensity
                    rotorMaterial.SetColor("_Color", WelcomeCurrColorOverheat);
                })
                .setOnComplete(() =>
                {
                    Debug.Log($"Emission color after overheat: {rotorMaterial.GetColor("_EmissionColor")}");
                }).id;

            Debug.Log("Overheat color animation started.");
        }
        else
        {
            // Stop particle system for overheating
            if (PSStab != null)
            {
                PSStab.Stop();
            }

            // Animate emission back to neutral over 10 seconds with an easeInOutQuad curve
            LTColorCurTween = LeanTween.value(Rotor, 1f, -1f, 10f)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnUpdate((float t) =>
                {
                    // Transition emission to black and color to white
                    Color emissionColor = Color.Lerp(rotorMaterial.color, Color.black, t);
                    WelcomeCurrColorOverheat = Color.Lerp(rotorMaterial.color, Color.white, t);
                    rotorMaterial.SetColor("_EmissionColor", emissionColor);
                    rotorMaterial.SetColor("_Color", WelcomeCurrColorOverheat);
                })
                .setOnComplete(() =>
                {
                    Debug.Log($"Emission color after cooling: {rotorMaterial.GetColor("_EmissionColor")}");
                }).id;

            Debug.Log("Cooling color animation started.");
        }
    }

    public IEnumerator EnterOverheat()
    {
        StopCoroutine(ExitOverheat());
        var rotorRenderer = Rotor.gameObject.GetComponent<MeshRenderer>();
        var rotorMaterial = rotorRenderer.material;
        PSStab.Play();
        LTColorCurTween = LeanTween.value(Rotor, -1f, 1f, 3f)
    .setEase(LeanTweenType.easeOutQuad)
    .setOnUpdate((float t) =>
    {
        WelcomeCurrColorOverheat = Color.Lerp(rotorMaterial.color, Color.red, t);
        rotorMaterial.SetColor("_EmissionColor", WelcomeCurrColorOverheat * 2f); // Adjust intensity
        rotorMaterial.SetColor("_Color", WelcomeCurrColorOverheat);
    })
    .setOnComplete(() =>
    {
        Debug.Log($"Emission color after overheat: {rotorMaterial.GetColor("_EmissionColor")}");
    }).id;

        Debug.Log("Overheat color animation started.");

        yield return null;

    }


    public IEnumerator ExitOverheat()
    {
        StopCoroutine(EnterOverheat());
        var rotorRenderer = Rotor.gameObject.GetComponent<MeshRenderer>();
        var rotorMaterial = rotorRenderer.material;
        PSStab.Play();
        LTColorCurTween = LeanTween.value(Rotor, 1f, -1f, 3f)
    .setEase(LeanTweenType.easeOutQuad)
    .setOnUpdate((float t) =>
    {
        WelcomeCurrColorOverheat = Color.Lerp(rotorMaterial.color, Color.white, t);
        rotorMaterial.SetColor("_EmissionColor", WelcomeCurrColorOverheat); // Adjust intensity
        rotorMaterial.SetColor("_Color", WelcomeCurrColorOverheat);
    })
    .setOnComplete(() =>
    {
        Debug.Log($"Emission color after overheat: {rotorMaterial.GetColor("_EmissionColor")}");
    }).id;

        Debug.Log("Overheat color animation started.");

        yield return null;

    }

    IEnumerator STABINTCHECK()
    {
        float integrityLoss = 0;
        if (HighTempWarning && StabOverHeat == false) { integrityLoss += 0.3f; }
        if (HighTempWarning && StabOverHeat) { integrityLoss += 0.7f; }
        if (StabTemp > 700) { integrityLoss += 1.3f; }
        if (StabTemp > 900) { integrityLoss += 1.7f; }

        if (HighRPM && RotorOverLoad == false) { integrityLoss += 0.2f; }
        if (HighRPM && RotorOverLoad) { integrityLoss += 0.5f; }

        if (StabStatus == "ONLINE" && CanGetDamaged)
        {
            StructuralIntegrity -= (int)Math.Round(integrityLoss);
            if (StructuralIntegrity < 15f)
            {
                STABESHUTDOWN();
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(STABINTCHECK());
    }
    private void FixedUpdate()
    {
        if (CanRotate)
        {
            float rotationSpeed = RPM * Time.deltaTime;
            Rotor.transform.Rotate(rotationAxis * rotationSpeed);
        }
        var usedPWR = 5;
        if (CanUsePower)
        {
            usedPWR += 250 / (100 * RPM + 1);
        }

    }



    public bool StabCheckForCoreVal()
    {
        if (StabStatus != "DESTROYED" && StabStatus != "OFFLINE")
        {
            return true;
        }
        return false;
    }


    public void StabChangePowerCaller(PassiveArgs lol)
    {
        var type = lol.arg1;
        var d = lol.arg2;
        if (type == "power")
        {
            if (d == "up")
            {
                Task.Run(StabChangePowerUp);
            }
            if (d == "down")
            {
                Task.Run(StabChangePowerDown);
            }
        }
        else if (type == "rpm")
        {

        }
        else if (type == "cooling")
        {
            if (d == "up")
            {
                Task.Run(StabChangeCoolingUp);
            }
            if (d == "down")
            {
                Task.Run(StabChangeCoolingDown);
            }
        }
        else
        {
            Debug.LogError("INVALID ARGUMENT!");
        }
    }

    public async Task StabChangePowerUp()
    {
        if (StabAdminLock == false)
        {
            CurrChange = !CurrChange;
            Debug.LogWarning("bleh");
            CurrPowerChangeDir = "Up";
            while (CurrChange && CurrPowerChangeDir == "Up" && Power < 100)
            {
                Debug.LogWarning("bleh");
                await Task.Delay(500);
                Power += 1;
            }
        }
    }

    public async Task StabChangePowerDown()
    {
        if (StabAdminLock == false)
        {
            CurrChange = !CurrChange;
            Debug.LogWarning("bleh");
            CurrPowerChangeDir = "Down";
            while (CurrChange && CurrPowerChangeDir == "Down" && Power > 10)
            {
                await Task.Delay(500);
                Power -= 1;
                Debug.LogWarning("bleh");
            }
        }
    }






    public async Task StabChangeCoolingUp()
    {
        if (StabAdminLock == false)
        {
            CurrChange = !CurrChange;
            Debug.LogWarning("bleh");
            CurrCoolingChangeDir = "Up";
            while (CurrChange && CurrCoolingChangeDir == "Up" && CoolantInput < 100)
            {
                Debug.LogWarning("bleh");
                await Task.Delay(500);
                CoolantInput += 1;
            }
        }
    }

    public async Task StabChangeCoolingDown()
    {
        if (StabAdminLock == false)
        {
            CurrChange = !CurrChange;
            Debug.LogWarning("bleh");
            CurrCoolingChangeDir = "Down";
            while (CurrChange && CurrCoolingChangeDir == "Down" && CoolantInput > 0)
            {
                await Task.Delay(500);
                CoolantInput -= 1;
                Debug.LogWarning("bleh");
            }
        }
    }


    private void OnApplicationQuit()
    {
        CurrChange = false;
        CurrPowerChangeDir = "none";
        CurrCoolingChangeDir = "none";
    }

    //FUNC FOR EVENTS
    public void STABSTART()
    {
        //Attempting Stab startup...
        if (StructuralIntegrity == 100)
        {

        }
        if (StructuralIntegrity > 100)
        {

        }
    }

    public async void STABESHUTDOWN()
    {
        CanGetDamaged = false;
        PendingEvent = "SHUTDOWN";
        //Attempting Stab shutdown...
        if (StructuralIntegrity == 100)
        {
            await StabRpmTweenDown(0, 100);
            StabStatus = "OFFLINE";
            PendingEvent = "none";
            Laser.SetActive(false);
        }
        if (StructuralIntegrity < 100)
        {
            if (WW.ChanceMath(50f) == true)
            {
                print("kys");
                StabKys();
            }
            else
            {
                print("lucky enough");
                await StabRpmTweenDown(0, 100);
                StabStatus = "OFFLINE";
                PendingEvent = "none";
                Laser.SetActive(false);
            }
        }
    }


    public void StabKys()
    {
        StabStatus = "ERROR";
        Power = 0;
        CanHeat = false;
        canCool = false;
        CanKys = false;
        CanStart = false;
        StabRPMCHANGING(30, 10);
        PSStab.startColor = Color.grey;
        PSStab.Play();
        ASSTAB.clip = StabDie;
        ASSTAB.Play();
        Laser.gameObject.SetActive(false);
    }

    public async Task StabStart()
    {
        CanRotate = true;
        if (CanStart)
        {
            PendingEvent = "STARTUP";
            StabStatus = "ONLINE";
            await StabRpmTweenUp(500, 40);
            PendingEvent = "none";
        }
    }

    public void StabOutage()
    {
        StabAdminLock = true;
        oldRPM = RPM;
        oldLASERPOWER = Power;
        StabRPMCHANGING(60, 1.2f);
        Power = 0;
        Laser.SetActive(false);
    }

    public void StabExitOutage()
    {
        StabRPMCHANGING(oldRPM, 0.9f);
        Power  = oldLASERPOWER;
        Laser.SetActive(true);
    }

    public IEnumerator InstantShutdown()
    {
        yield return null;
        StabRPMCHANGING(0, RPM);
        Laser.SetActive(false);
        CanHeat = false;
        CanGetDamaged = false;
        LeanTween.value(Power, 0, 6)
            .setOnUpdate((float t) =>
            {
                Power = (int)Mathf.Lerp(Power, 0, t);
            });

        yield return new WaitForSeconds(8);
        StabStatus = StabStatusEnum.OFFLINE.ToString();
    }

    public async Task StabRpmTweenUp(int to, int TimeinMS)
    {

        for (int i = to; RPM < i;)
        {
            RPM++;
            await Task.Delay(TimeinMS);
        }
    }

    public void StabRPMCHANGING(float to, float time)
    {
        var aze = RPM;
        LeanTween.value(aze, to, time)
            .setOnUpdate((float t) =>
            {
                RPM = Mathf.CeilToInt(t);
            });
    }
    public async Task StabRpmTweenDown(int to, int TimeinMS)
    {
        for (int i = to; RPM > i;)
        {
            RPM--;
            await Task.Delay(TimeinMS);
        }

    }



    public IEnumerator StabBuildUp(float timetoreach, Color BColor, Vector3 StartSize, Vector3 GoalSize)
    {
        BColor.a = 0;
        BuildUp.GetComponent<MeshRenderer>().material.SetColor("_Color", BColor);
        Color lol = new Color();
        lol.r = BColor.r;
        lol.g = BColor.g;
        lol.b = BColor.b;
        lol.a = 1;

        BuildUp.transform.position = StartSize;

        LeanTween.value(BuildUp.GetComponent<MeshRenderer>().material.GetColor("_Color").a, lol.a, timetoreach)
            .setOnUpdate((float t) =>
            { 
                Color n = new Color(BColor.r, BColor.g, BColor.b, t);
                BuildUp.GetComponent<MeshRenderer>().material.SetColor("_Color", n);
            });

        BuildUp.LeanScale(GoalSize, timetoreach);

        yield return new WaitForSeconds(timetoreach);

        Vector3 LastGoalSize = new Vector3(GoalSize.x + 3, GoalSize.y + 3, GoalSize.z + 3);

        LeanTween.value(BuildUp.GetComponent<MeshRenderer>().material.GetColor("_Color").a, 0, 0.4f)
            .setOnUpdate((float t) =>
            {
                Color n = new Color(BColor.r, BColor.g, BColor.b, t);
                BuildUp.GetComponent<MeshRenderer>().material.SetColor("_Color", n);
            });

        BuildUp.LeanScale(LastGoalSize, 0.4f);


        yield return null;
    }




    public bool CanPurge()
    {
        if (StabStatus == StabStatusEnum.OFFLINE.ToString() || StabStatus == StabStatusEnum.ERROR.ToString() || StabStatus == StabStatusEnum.DESTROYED.ToString() || StructuralIntegrity < 15)
        {
            Cm.ReactorSysLogsScreen.EntryPoint("! " + WS.ToString() + " FAILED TO PURGE!", Color.red);
            return false;
        }
        else
        {
            //Cm.ReactorSysLogsScreen.EntryPoint("! " + WS.ToString() + " PURGE SEQUENCE AUTHORIZED!", Color.green);
            Debug.LogError("! " + WS.ToString() + " PURGE SEQUENCE AUTHORIZED!");
            return true;
        }
    }



}
    public class Utility : MonoBehaviour
    {


    public static Utility Instance;
    private void Awake()
    {
        Instance = this;
    }




    public async Task WaitedWalter(int Timetowait)
        {
            Task.Delay(Timetowait);

        }

        //public Task TweenValue(int startV, int EndV, int awaittime)
        //{
        //    while (startV < EndV)
        //    {
        //    startV++;
        //    Task.Delay(awaittime);
        //    }


        //return 
        //}

        public bool ChanceMath(float pourcent)
        {
            var tempV = UnityEngine.Random.Range(0, 100);

            if (pourcent >= tempV)
            {
                Debug.Log(pourcent + "% | " + tempV + " float");
                return false;
            }
            if (pourcent <= tempV)
            {
                Debug.Log(pourcent + "% | " + tempV + " float");
                return true;
            }
            return false;
        }

        //public void TweenLightsIntensity(Light light, float to, float DimFactor)
        //{
        //    if (to > light.intensity)
        //    {
        //        for (float f = to;  f > light.intensity; 0f)
        //        {

        //        }
        //    }
        //    if (to < light.intensity)
        //    {

        //    }
        //}

    }



[SerializeField]
public class argStabVarchange
{
    public string type;
    public string d;
}