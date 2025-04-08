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
            StartCoroutine(AlarmLoop());
        }
    }

    public IEnumerator AlarmLoop()
    {
        if (Active)
        {
            audiosource.Play();
            yield return null;
        }
    }

}
