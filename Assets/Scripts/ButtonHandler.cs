using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHandler : MonoBehaviour
{

    [Header("Component")]
    public GameObject button;
    public UnityEvent CallOnPress;

    [Header("Value")]
    public bool canBePressed = true;

    public bool haveTrigger = false;
    public bool TriggerUsed = false;
    
    public bool haveCooldown = true;
    public bool onCooldown = false;
    /// <summary>
    /// in milliseconds btw
    /// </summary>
    public int CooldownTime = 3000;

    public AnimationClip ClickedAnim;
    public Animator ButtonAnimator;

    public AudioSource ButtonAudioSource;
    //
    //
    //
    void Start()
    {
        if (button == null) { button = this.gameObject; }
    }


    private void OnMouseOver()
    {
        //Cursor.SetCursor()
        //Debug.LogError("Overed");

    }
    private void OnMouseDown()
    {
        if (canBePressed && onCooldown == false && TriggerUsed == false)
        {
            canBePressed = false;
            ButtonAnimator.SetTrigger("CanClickAnim");
            CallOnPress.Invoke();
            if (haveTrigger)
            {
                TriggerUsed = true;
            }
            if(ButtonAudioSource.clip != null)
            {
                ButtonAudioSource.Play();
            }
            if (haveCooldown)
            {
                Task.Run(CooldownRegen);
            }
            else if (!haveCooldown)
            {
                canBePressed = true;
            }

        }
    }

    public async Task CooldownRegen()
    {
        await Task.Delay(CooldownTime);
        onCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
