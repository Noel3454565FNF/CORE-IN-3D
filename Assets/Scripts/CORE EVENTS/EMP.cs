using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public static EMP instance;
    public GameObject EMPCLONETHING;
    
    public AudioClip ElectricLoop;
    public AudioClip Warn;

    private void Awake()
    {
        instance = this;
    }



    public EMPClone NewEMP()
    {
        GameObject k = GameObject.Instantiate(EMPCLONETHING, COREManager.instance.COREMeshRenderer.gameObject.transform, false);
        EMPClone clone = k.AddComponent<EMPClone>();
        clone.create(k);
        Debug.LogWarning(clone.gameObject.name);
        return clone;
        
    }

}

public class EMPClone:MonoBehaviour
{

    public Vector3 EMPnormal = new Vector3(30, 30, 30);
    public Vector3 EMPcontained = new Vector3(15, 15, 15);
    public Vector3 EMPrunAway = new Vector3(60, 60, 60);

    public GameObject WAVE;
    public AudioSource AS;

    public int EMPStrengh;
    public COREManager CM;

    public int recontainmentChance = 0;
    public int dissipationChance = 0;

    private void Awake()
    {
        
    }

    public void create(GameObject Base, int EMPStrenghOVERRIDE = 0)
    {
        WAVE = Base;
        //WAVE.gameObject.transform.position = new Vector3(0, 0, 0);
        GameObject.Find("Right screen (global warnings)").GetComponent<AudioSource>().clip = EMP.instance.Warn;
        GameObject.Find("Right screen (global warnings)").GetComponent<AudioSource>().volume = 0.6f;
        GameObject.Find("Right screen (global warnings)").GetComponent<AudioSource>().loop = true;
       GameObject.Find("Right screen (global warnings)").GetComponent<AudioSource>().Play();

        AS = WAVE.AddComponent<AudioSource>();
        AS.clip = EMP.instance.ElectricLoop;
        AS.volume = 0.1f;
        AS.loop = true;
        AS.Play();

        LeanTween.value(WAVE, WAVE.transform.localScale, EMPnormal, 0.7f)
            .setEaseInOutQuad()
            .setOnUpdate((Vector3 v)
            =>
            {
                WAVE.transform.localScale = v;
            });

        if (EMPStrenghOVERRIDE != 0)
        {
            EMPStrengh = EMPStrenghOVERRIDE;
        }
        else
        {
            EMPStrengh = Random.Range(1, 100);
        }

        //math
        
    }
}
