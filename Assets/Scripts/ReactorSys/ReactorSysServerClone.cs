using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorSysServerClone : MonoBehaviour
{
    public enum StatusEnum
    {
        ONLINE,
        OFFLINE,
        CRASH,
        MAINTENANCE,
        ADMINLOCK
    };
    public StatusEnum Status = new StatusEnum();

    public enum Act
    {
        SHUTDOWN,
        BOOTUP,
        CRASH,
        REBOOT,
        ENTER_MAINTENANCE,
        EXIT_MAINTENANCE
    };

    public Color OnlineColor;
    public Color OfflineColor;
    public Color CrashColor;
    public Color MaintenanceColor;
    public Color AdminlockColor;

    public Color CurrentBlinkColor;
    public bool CanBlink = false;

    public ButtonHandler RebootButton;
    public MeshRenderer Led;

    public int ServerIntegrity = 100;
    public int ServerPower = 0;
    public string ServerName;
    public bool SystemBusy = false;

    public LineClone Logs;

    public AudioClip BootSound;
    public AudioClip CrashSound;
    public AudioClip AmbiantSound;

    public AudioSource AS;
    private void Start()
    {
        if (AS == null)
        {
            if (gameObject.GetComponent<AudioSource>() != null)
            {
                AS = gameObject.GetComponent<AudioSource>();
            }
            else
            {
                AS = gameObject.AddComponent<AudioSource>();
            }
            AS.volume = 0.1f;
            Status = StatusEnum.OFFLINE;
            Led.material.color = OfflineColor;
        }
    }


    public void ChangeStatus(Act action)
    {
        if (SystemBusy == false)
        {
            SystemBusy = true;
            bool actionUsed = false;

            if (action == Act.SHUTDOWN)
            {
                if (Status != StatusEnum.OFFLINE && Status != StatusEnum.CRASH && Status != StatusEnum.MAINTENANCE && Status != StatusEnum.ADMINLOCK)
                {
                    actionUsed = true;
                    BootupCaller();
                }
            }


            if (actionUsed == false)
            {
                SystemBusy = false;
            }
        }
    }





    //Events
    /// <summary>
    /// USE ChangeStatus() UNLESS YOU WANT TO FORCE THE SERVER EVENT
    /// </summary>
    public void BootupCaller()
    {
        StartCoroutine(Bootup());
    }
    public IEnumerator Bootup()
    {
        Logs.EntryPoint(ServerName + " booting up...", Color.blue);
        StartBlink(1f, 0.5f, OnlineColor);
        
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        
        StopBlink();
        Logs.EntryPoint(ServerName + " online!", Color.green);
        Status = StatusEnum.ONLINE;
        SystemBusy = false;

        yield break;
    }





    public void StartBlink(float BlinkTime, float BlackTime)
    {
        StopBlink();
        CanBlink = true;
        CurrentBlinkColor = Led.material.color;
        StartCoroutine(BlinkFunc(BlinkTime, BlackTime));
    }
    public void StartBlink(float BlinkTime, float BlackTime, Color BlinkTo)
    {
        StopBlink();
        CanBlink = true;
        CurrentBlinkColor = BlinkTo;
        StartCoroutine(BlinkFunc(BlinkTime, BlackTime, BlinkTo));
    }
    public void StopBlink()
    {
        CanBlink = false;
        StopCoroutine("BlinkFunc");
        Led.material.color = CurrentBlinkColor;
        CurrentBlinkColor = Color.clear;
    }

    private IEnumerator BlinkFunc(float BlinkTime, float BlackTime)
    {
        Led.material.color = CurrentBlinkColor;
        yield return new WaitForSeconds(BlinkTime);
        Led.material.color = Color.black;
        yield return new WaitForSeconds(BlackTime);
        if (CanBlink)
        {
            StartCoroutine(BlinkFunc(BlinkTime, BlackTime));
        }
        else
        {
            yield break;
        }
    }
    private IEnumerator BlinkFunc(float BlinkTime, float BlackTime, Color BlinkTo)
    {
        Led.material.color = BlinkTo;
        yield return new WaitForSeconds(BlinkTime);
        Led.material.color = Color.black;
        yield return new WaitForSeconds(BlackTime);
        if (CanBlink)
        {
            StartCoroutine(BlinkFunc(BlinkTime, BlackTime, BlinkTo));
        }
        else
        {
            yield break;
        }
    }

}