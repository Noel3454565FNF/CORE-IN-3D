using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;



[System.Serializable]
public class LightsManager : MonoBehaviour
{
    public static LightsManager GLM;


    [Header("Rooms & lights shit")]
    public Negate3roomsL[] negate3RoomsL;


    [Header("SFX")]
    public AudioClip LightsSound;
    public AudioClip LightsDisrupt;

    private void Awake()
    {
        GLM = this;
    }

    

    public IEnumerator LevelNeg3LightsControl(int intensity, int SwitchTime, Negate3roomsName whatroom)
    {
        if (whatroom == Negate3roomsName.CORE_CONTROL_ROOM || whatroom == Negate3roomsName.ALL)
        {
            var lol = 0;
            foreach (Light light in negate3RoomsL[lol].corecontrolroom)
            {
                yield return new WaitForSeconds(0.1f);

                LeanTween.value(gameObject, light.intensity, intensity, SwitchTime)
                    .setOnUpdate((float t) =>
                    {
                        light.intensity = t;
                    });
                LightSoundPlayer(light, LightsSound);
                lol++;
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator LevelNeg3LightsControl(int intensity, int SwitchTime, Color LightColor, float LightColorSwitchTime, Negate3roomsName whatroom)
    {
        if (whatroom == Negate3roomsName.CORE_CONTROL_ROOM || whatroom == Negate3roomsName.ALL)
        {
            var lol = 0;
            foreach (Light light in negate3RoomsL[lol].corecontrolroom)
            {
                yield return new WaitForSeconds(0.1f);
                light.intensity = Mathf.Lerp(light.intensity, intensity, SwitchTime);

                LeanTween.value(gameObject, light.color, LightColor, LightColorSwitchTime)
                    .setOnUpdate((Color t) =>
                    {
                        light.color = t;
                    });
                LightSoundPlayer(light, LightsSound);
                lol++;
            }
            yield return null;
        }
        yield return null;
    }


    private void LightSoundPlayer(Light light, AudioClip sound)
    {
        var sonu = light.gameObject.GetComponent<AudioSource>();
        sonu.volume = 0.3f;
        sonu.clip = sound;
        sonu.Play();
    }


    public void LevelNeg3LightsFlick(int MaxFlicker, Negate3roomsName where)
    {
        var howmanyFLICKERS = 0;
        while (howmanyFLICKERS < MaxFlicker)
        {
            if (where == Negate3roomsName.CORE_CONTROL_ROOM)
            {
                var lol = 0;
                foreach (Light light in negate3RoomsL[lol].corecontrolroom)
                {
                    StartCoroutine(Flickering(light));
                    lol++;
                }
                howmanyFLICKERS++;
            }

        }

    }

    private IEnumerator Flickering(Light light)
    {
        float range = Random.Range(0, 3);
        yield return new WaitForSeconds(range);
        LightSoundPlayer(light, LightsDisrupt);
        light.intensity = 0;
        yield return new WaitForSeconds(0.25f);
        light.intensity = 1;
        yield break;
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

