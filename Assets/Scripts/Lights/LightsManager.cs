using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using Unity.VisualScripting;



[System.Serializable]
public class LightsManager : MonoBehaviour
{
    public static LightsManager GLM;


    [Header("Rooms & lights shit")]
    public new Negate3roomsL[] negate3RoomsL;


    [Header("SFX")]
    public AudioClip LightsSound;
    public AudioClip LightsDisrupt;

    private void Awake()
    {
        GLM = this;
        var lol = 0;
        foreach(Light gm in negate3RoomsL[lol].corecontrolroom)
        {
            gm.gameObject.AddComponent<LampManager>();
        }
    }

    

    public IEnumerator LevelNeg3LightsControl(int intensity, int SwitchTime, Negate3roomsName whatroom)
    {
        if (whatroom == Negate3roomsName.CORE_CONTROL_ROOM || whatroom == Negate3roomsName.ALL)
        {
            var lol = 0;
            foreach (Light light in negate3RoomsL[lol].corecontrolroom)
            {
                yield return new WaitForSeconds(0.1f);
                light.intensity = Mathf.Lerp(light.intensity, intensity, SwitchTime);
                var sonu = light.gameObject.GetComponent<AudioSource>();
                sonu.clip = LightsSound;
                sonu.Play();
                lol++;
            }
            yield return null;
        }
        yield return null;
    }


    public async Task LevelNeg3LightsFlick(int MaxFlicker, Negate3roomsName where)
    {
        var howmanyFLICKERS = 0;
        while (howmanyFLICKERS < MaxFlicker)
        {
            if (where == Negate3roomsName.CORE_CONTROL_ROOM)
            {
                var lol = 0;
                foreach (Light light in negate3RoomsL[lol].corecontrolroom)
                {
                    await light.gameObject.GetComponent<LampManager>().canIFlicker();
                    lol++;
                }
                howmanyFLICKERS++;
            }

        }

    }

    public async Task flickering()
    {

    }

}


public enum Negate3roomsName
{
    ALL,
    CORE_CONTROL_ROOM,
    CORE_CHAMBER,
}

[System.Serializable]
public class Negate3roomsL
{
    public Light[] corecontrolroom;
    public Light[] corechamber;
}

public class LampManager : MonoBehaviour
{
    public Light light;
    public AudioSource AS;


    private void Start()
    {
        if (AS == null)
        {
            if (gameObject.GetComponent<AudioSource>() == null)
            {
                AS = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                AS = gameObject.GetComponent<AudioSource>();
            }
        }
    }

    public async Task canIFlicker()
    {
        float rand = Random.Range(0f, 3000f);
        await Task.Delay((int)rand);
        AS.clip = LightsManager.GLM.LightsDisrupt;
        light.intensity = 0;
        await Task.Delay(0250);
        light.intensity = 1;
    }
}