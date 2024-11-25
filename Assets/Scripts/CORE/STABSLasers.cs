using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STABSLasers : MonoBehaviour
{
    [Header("Component")]
    public GameObject[] GMstabsParts;
    public MeshRenderer[] MRstabsParts;
    [HideInInspector] public GameObject Rotor;
    public COREManager Cm;


    [Header("Vars")]
    public string StabStatus = "OFFLINE";
    public string PendingEvent = "none";
    public float StabTemp = 26;
    public float RPM = 0f;
    public float Power = 0;
    public float CoolantInput = 0;
    public float StructuralIntegrity = 100;


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


    [HideInInspector]public IEnumerator stabtempcheckvar;
    [HideInInspector]public IEnumerator stabintcheckvar;

    [Header("Optional Vars")]
    public Vector3 rotationAxis = Vector3.up;


    void Start()
    {
        StartCoroutine(STABTEMPCHECK());
        StartCoroutine(STABINTCHECK());
        var lol = 0;
        foreach(GameObject gm in GMstabsParts)
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
            TempTar2 = -CoolantInput / 2.5f;
            TempTar3 = Power / 10f;
            TempTarA = TempTar + TempTar3;
            TempTarA += TempTar2;
            StabTemp += TempTarA;
            print(TempTarA);
        }

        if (StabTemp >= 300 && HighTempWarning == false)
        {
            HighTempWarning = true;
        }
        if (StabTemp < 300 && HighTempWarning)
        {
            HighTempWarning = false;
        }

        if (StabTemp >= 400 && StabOverHeat == false)
        { 
            StabOverHeat = true;
            ROF();
        }
        if (StabTemp < 400 && StabOverHeat)
        {
            StabOverHeat = false;
        }

        if (RPM >= 370 && HighRPM == false)
        {
            HighRPM = true;
        }
        if (RPM < 370 && HighRPM)
        {
            HighRPM = false;
        }

        if (RPM >= 420 && RotorOverLoad == false)
        {
            RotorOverLoad = true;
        }
        if (RPM < 420 && RotorOverLoad)
        {
            RotorOverLoad = false;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(STABTEMPCHECK());
    }

    public void ROF()
    {
        var RoT = Rotor.gameObject.GetComponent<MeshRenderer>();
        var RoM = Rotor.gameObject.GetComponent<Material>();

        if (StabOverHeat)
        {
            RoT.material.color = Color.Lerp(RoT.material.color, OverHeatCol, 100000f);
        }
        if (StabOverHeat == false)
        {
            RoT.material.color = Color.Lerp(RoT.material.color, NormalCol, 10f);
        }


    }

    IEnumerator STABINTCHECK()
    {
        float integrityLoss = 0;
        if (HighTempWarning && StabOverHeat == false) { integrityLoss += 0.3f; }
        if (HighTempWarning && StabOverHeat) { integrityLoss += 0.7f; }

        if (HighRPM && RotorOverLoad == false) { integrityLoss += 0.2f; }
        if (HighRPM && RotorOverLoad) { integrityLoss += 0.5f; }

        if (StabStatus == "ONLINE")
        {
            StructuralIntegrity -= integrityLoss;
            print(integrityLoss);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(STABINTCHECK());
    }
    private void FixedUpdate()
    {
        if (StabStatus == "ONLINE" | StabStatus == "OVERLOADED")
        {
            float rotationSpeed = RPM * Time.deltaTime;
            Rotor.transform.Rotate(rotationAxis * rotationSpeed);
        }
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

    public void STABESHUTDOWN()
    {
        //Attempting Stab shutdown...
        bool SYes = false;
        if (StructuralIntegrity == 100)
        {

        }
        if (StructuralIntegrity > 100)
        {

        }
    }
}
