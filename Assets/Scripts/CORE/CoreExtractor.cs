using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreExtractor : MonoBehaviour
{
    public enum ExtractorStatusEnum
    {
        Online,
        Offline,
        Fault
    };
    public ExtractorStatusEnum status = ExtractorStatusEnum.Offline;

    public enum ExtractorNameEnum
    {
        Extractor1,
        Extractor2,
        Extractor3,
        Extractor4
    }
    public ExtractorNameEnum ExtractorName;
    public int Power = 10;
    public int MinPower = 10;
    public int MaxPower = 100;

    public int Integrity = 100;



    public enum ExtractorVarEnum
    {
        Power,
        MinPower,
        MaxPower,
        Integrity,
        status
    }


    public IEnumerator extractorPOWERLogics()
    {

        yield break;
    }



}
