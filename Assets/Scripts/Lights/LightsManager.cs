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
                yield return new WaitForSeconds(0.05f);
                light.intensity = intensity;
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
                yield return new WaitForSeconds(0.05f);
                light.intensity = Mathf.Lerp(light.intensity, intensity, SwitchTime);

                LeanTween.value(gameObject, light.color, LightColor, LightColorSwitchTime)
                    .setOnUpdate((Color t) =>
                    {
                        print("aaa");
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
        sonu.volume = 0.1f;
        sonu.clip = sound;
        sonu.Play();
    }


    //public void LevelNeg3LightsFlick(int MaxFlicker, Negate3roomsName where)
    //{
    //    var howmanyFLICKERS = 0;
    //    while (howmanyFLICKERS < MaxFlicker)
    //    {
    //        if (where == Negate3roomsName.CORE_CONTROL_ROOM || where == Negate3roomsName.ALL)
    //        {
    //            var lol = 0;
    //            foreach (Light light in negate3RoomsL[lol].corecontrolroom)
    //            {
    //                StartCoroutine(Flickering(light));
    //                lol++;
    //            }
    //            howmanyFLICKERS++;
    //        }

    //    }

    //}

    private IEnumerator Flickering(Light light, int flickers = 4, float minDelay = 0.05f, float maxDelay = 0.2f)
    {
        // Optional: Wait before starting flicker
        yield return new WaitForSeconds(Random.Range(0f, 1.5f));

        // Optional: Play sound once
        if (light != null)
            LightSoundPlayer(light, LightsDisrupt);

        for (int i = 0; i < flickers; i++)
        {
            if (light == null) yield break;

            light.intensity = 0f;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            light.intensity = 1f;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }

        // Ensure light is left on
        if (light != null)
            light.intensity = 1f;
    }


    public void LevelNeg3LightsFlick(int maxFlickers, Negate3roomsName where)
    {
        print("called");
        StartCoroutine(FlickerRoutine(maxFlickers, where));
    }

    private IEnumerator FlickerRoutine(int maxFlickers, Negate3roomsName where)
    {
        for (int i = 0; i < maxFlickers; i++)
        {
            if (where == Negate3roomsName.CORE_CONTROL_ROOM || where == Negate3roomsName.ALL)
            {
                foreach (Light light in negate3RoomsL[i].corecontrolroom)
                {
                    print("bleh");
                    StartCoroutine(Flickering(light));
                }
            }

            // Wait a bit before next flicker round (adjust time as needed)
        }
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

