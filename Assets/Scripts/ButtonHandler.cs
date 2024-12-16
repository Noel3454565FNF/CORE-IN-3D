using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    public float Pos;
    public bool Moving = false;

    [SerializeField]public AnimationClip ClickedAnim;
    public Animator ButtonAnimator;

    public AudioSource ButtonAudioSource;
    //
    //
    //
    void Start()
    {
        if (button == null) { button = this.gameObject; }
        ButtonAnimator.bodyPosition = button.transform.position;
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
            animhere();
            canBePressed = false;
            //ButtonAnimator.SetTrigger("CanClickAnim");
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



    public void animhere()
    {
        var sk = 0;
        var oldY = button.transform.position.y;
        LeanTween.value(0f, 0.01f, 0.4f)
            .setOnUpdate((float t) =>
            {
                button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - t, button.transform.position.z);
            })
            .setOnComplete(() =>
            {
                LeanTween.value(button.transform.position.y, oldY, 0.3f)
                .setOnUpdate((float t) =>
                {
                    button.transform.position = new Vector3(button.transform.position.x, t, button.transform.position.z);
                });
            });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
