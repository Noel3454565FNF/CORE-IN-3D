using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarnsPanelIcon : MonoBehaviour
{
    public float BlinkTime;
    public bool Active = false;
    public AudioClip AlarmSound;
    public AudioSource audiosource;
    public RawImage BG;
    public Color BGBlink = new Color(255, 255, 255);

    /// <summary>
    /// in second
    /// </summary>
    public float TimeToBlink = 1f;
    /// <summary>
    /// also in second
    /// </summary>
    public float TimeToWait = 1.5f;
    public Color BGBlack = new Color(0, 0, 0);
    
    
    
    
    public void Start()
    {
        if (audiosource == null)
        {
            if (gameObject.GetComponent<AudioSource>() == null)
            {
                audiosource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                audiosource = gameObject.GetComponent<AudioSource>();
            }
        }
        audiosource.clip = AlarmSound;
        audiosource.playOnAwake = false;
        audiosource.loop = false;
        audiosource.volume = 0.2f;
        audiosource.Stop();
    }


    public void SwitchState(bool state)
    {
        Active = state;

        if (Active == false)
        {
            StopCoroutine(AlarmLoop());
        }
        else
        {
            StopCoroutine(AlarmLoop());
            StartCoroutine(AlarmLoop());
        }
    }

    public IEnumerator AlarmLoop()
    {
        if (Active)
        {
            if (audiosource != null && AlarmSound != null)
            {
                audiosource.Play();
            }

            BG.color = BGBlink;

            yield return new WaitForSeconds(TimeToBlink);

            BG.color = BGBlack;

            yield return new WaitForSeconds(TimeToWait);

            StartCoroutine(AlarmLoop());
        }
    }

}
