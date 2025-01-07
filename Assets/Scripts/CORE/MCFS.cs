using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCFS : MonoBehaviour
{
    [HideInInspector]public static MCFS instance;
    [Header("Important Vars")]
    public int ShieldIntegrity = 0;
    public int ShieldPower = 0;
    public int ShieldMaxPower;
    public int ShieldEnergyReserve;
    public int ShieldEnergyReserveMax;
    public enum ShieldStatusEnum
    {
        Online,
        Offline,
        Error,
        AdminLock,
        Overload
    }
    public string ShieldStatus = ShieldStatusEnum.Offline.ToString();

    [Header("Component")]
    public GameObject Shield;
    [HideInInspector] public MeshRenderer SMR;


    private void Awake()
    {
        instance = this;
    }


    //use for startup or idk wtv
    public void ShieldCreation(int to, float time)
    {
        var frome = ShieldIntegrity;
        var too = to;
        LeanTween.value(frome, too, time)
            .setOnUpdate((float t) =>
            {
                ShieldIntegrity = Mathf.CeilToInt(t);
            });
    }

    private void Start()
    {
        print(ShieldStatus);
        SMR = Shield.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (ShieldAvalabilityCheck())
        {
            ShieldPower = ShieldMaxPower / ShieldIntegrity;
        }
    }

    public bool ShieldAvalabilityCheck()
    {
        if (ShieldStatus != ShieldStatusEnum.Offline.ToString() && ShieldStatus != ShieldStatusEnum.Error.ToString() && ShieldStatus != ShieldStatusEnum.Overload.ToString())
        {
            return true;
        }
        return false;
    }

}
