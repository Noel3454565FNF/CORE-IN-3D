using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

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
    }

    public void LevelNeg3LightsControl(int intensity, int SwitchTime, Negate3roomsName whatroom)
    {
        if (whatroom == Negate3roomsName.CORE_CONTROL_ROOM)
        {
            var lol = 0;
            foreach (Light light in negate3RoomsL[lol].corecontrolroom)
            {
                light.intensity = Mathf.Lerp(light.intensity, intensity, SwitchTime);
                var sonu = light.gameObject.GetComponent<AudioSource>();
                sonu.clip = LightsSound;
                sonu.Play();
                lol++;
            }
        }
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
