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
    
    [Description("caca")]public bool haveCooldown = true;
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
        Debug.LogError("Overed");

    }
    private void OnMouseDown()
    {
        Debug.LogError("Button Pressed");
        if (canBePressed && onCooldown == false && TriggerUsed)
        {
            canBePressed = false;
            if (haveTrigger)
            {
                TriggerUsed = true;
            }
            if (haveCooldown)
            {
                Task.Run(CooldownRegen);
            }
            if(ButtonAudioSource.clip != null)
            {
                ButtonAudioSource.Play();
            }
            ButtonAnimator.Play("Clicked");
            CallOnPress.Invoke();

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
