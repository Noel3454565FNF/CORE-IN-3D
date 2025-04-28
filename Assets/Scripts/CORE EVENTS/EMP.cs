using Cysharp.Threading.Tasks.Triggers;
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
        StartCoroutine(clone.create(k));
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
    public LineClone LC;

    public int recontainmentChance = 0;
    public int dissipationChance = 0;

    private void Awake()
    {
        CM = COREManager.instance;
        LC = CM.ReactorSysLogsScreen;
    }

    public IEnumerator create(GameObject Base, int EMPStrenghOVERRIDE = 0)
    {
        LC.EntryPoint("DANGER: EMP RUNAWAY DETECTED!", Color.red);
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

        LeanTween.value(WAVE, WAVE.transform.localScale, EMPnormal, 0.3f)
            .setEaseInOutQuad()
            .setOnUpdate((Vector3 v)
            =>
            {
                WAVE.transform.localScale = v;
            });

        recontainmentChance = Random.Range(0, 100); dissipationChance = Random.Range(0, 100);

        if (EMPStrenghOVERRIDE != 0)
        {
            EMPStrengh = EMPStrenghOVERRIDE;
        }
        else
        {
            EMPStrengh = Random.Range(1, 100);
        }

        yield return new WaitForSeconds(Random.Range(2, 4));
        
        LC.EntryPoint("ATTEMPTING TO DISSIPATE EMP WAVE", CM.LineOVERRIDEColor);

        foreach(STABSLasers stab in CM.Stablist)
        {
            if (true)
            {
                stab.CanRotate = true;
                //stab.PowerPurge1.Play();
                //stab.PowerPurge2.Play();
                stab.StabRPMCHANGING(900, 2);
            }
        }

        yield return new WaitForSeconds(2f);

        LC.EntryPoint("PURGING EMP WAVE...", Color.yellow);
        LeanTween.value(WAVE, WAVE.transform.position, new Vector3(20, 20, 20), 6)
            .setEaseInBack()
            .setEaseOutBack()
            .setOnUpdate((Vector3 v) =>
            {
                WAVE.transform.position = v;
            });
            

            if (dissipationChance <= 20)
            {
                LC.EntryPoint("EMP WAVE DISSIPATION SUCCESSFUL!", Color.green);
                Vector3 math1 = new Vector3(0, 0, 0);
                math1.x = WAVE.transform.position.x + 4;
                math1.y = WAVE.transform.position.y + 4;
                math1.z = WAVE.transform.rotation.z + 4;
            LeanTween.value(WAVE, WAVE.transform.position, math1, 0.2f)
                .setEaseInBack()
                .setEaseOutBack()
                .setOnUpdate((Vector3 v) =>
                {
                    WAVE.transform.position = v;
                })
                .setOnComplete(DIE);
            }
            else
        {
            LC.EntryPoint("EMP WAVE PERSISTANT!", Color.red);
            yield return new WaitForSeconds(0.3f);
            LC.EntryPoint("EMP WAVE RUNAWAY IMMINENT!", Color.red);
            yield return new WaitForSeconds(Random.Range(3, 10));

            LeanTween.value(WAVE, WAVE.transform.position, WAVE.transform.position, 3)
                .setEaseInBack()
                .setEaseOutBack()
                .setOnUpdate((Vector3 v) =>
                {
                    WAVE.transform.position = v; 
                });
            yield return new WaitForSeconds(3f);
        }
        

            

    }


    public void DIE()
    {
        GameObject.Destroy(this);
    }



    //SCREEN THINGS
    public IEnumerator cinematicBlackBands(float value, float timetovalue, float timetostay, float timefarvalue)
    {



        yield break;
    }
}
