using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolantMAIN : MonoBehaviour
{
    [Header("Components")]
    public static CoolantMAIN instance;

    [Header("Value")]
    public int GlobalPumpsInput = 0;
    
    public int FlowToReactor = 0;
    public int MaxFlowToReactor = 3000;

    public int CoolantPoolFullness = 60000;
    public int CoolantPoolMax = 60000;
    public int CoolantPoolUsage = 0;
    public int CoolantPoolMaxUsage = 900;
    public bool CanCoolantPoolDecrease = false;

    public bool CoolantPoolEmpty = false;
    public bool CoolantNetworkFault = false;

    [Header("Pumps")]
    public int pump1Flow = 75;
    public int pump1MaxFlow = 100;
    public int pump1MaxRawFlow = 2500;
    public bool pump1Active = true;

    public int pump2Flow = 75;
    public int pump2MaxFlow = 100;
    public int pump2MaxRawFlow = 2500;
    public bool pump2Active = true;

    public int pump3Flow = 75;
    public int pump3MaxFlow = 100;
    public int pump3MaxRawFlow = 2500;
    public bool pump3Active = true;

    public int pump4Flow = 75;
    public int pump4MaxFlow = 100;
    public int pump4MaxRawFlow = 2500;
    public bool pump4Active = true;



    private void Awake()
    {
        instance = this;
        gameObject.AddComponent<CoreCoolantFlowManager>();
    }



    public IEnumerator MainCoolantLogicLoop()
    {
        //logic


        yield return new WaitForSeconds(1f);
        StartCoroutine(MainCoolantLogicLoop());
        yield break;
    }
}







public class CoreCoolantFlowManager:MonoBehaviour
{
    [Header("Components")]
    public static CoreCoolantFlowManager instance;

    [Header("Value")]
    public int AmountOfCoolantAvailable;
    public int MaximumCoolantStored = 2000;

    [Header("Condition")]
    public bool CoolantLogicOn = true;
    public bool CoolantFlowFault = false;





    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
     if (CoolantLogicOn)
        {

        }
    }
}