using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReactorSysServerClone : MonoBehaviour
{
    public enum StatusEnum
    {
        OFFLINE,
        ONLINE,
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

    public Color PastBlinkColor;
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

        UnityEvent a = new UnityEvent();
        a.AddListener(RebootCaller);
        RebootButton.ScriptEventCall = a;
    }


    public void ChangeStatus(Act action)
    {
        if (SystemBusy == false)
        {
            SystemBusy = true;
            bool actionUsed = false;

            if (action == Act.SHUTDOWN && actionUsed == false)
            {
                if (Status != StatusEnum.OFFLINE && Status != StatusEnum.CRASH && Status != StatusEnum.MAINTENANCE && Status != StatusEnum.ADMINLOCK)
                {
                    actionUsed = true;
                }
            }
            else if (action == Act.BOOTUP && actionUsed == false)
            {
                if (Status != StatusEnum.ONLINE && Status != StatusEnum.CRASH && Status != StatusEnum.MAINTENANCE && Status != StatusEnum.ADMINLOCK)
                {
                    actionUsed = true;
                    BootupCaller();
                }
            }
            else if (action == Act.CRASH && actionUsed == false)
            {
                actionUsed = true;
                CrashCaller();
            }


            if (actionUsed == false)
            {
                SystemBusy = false;
            }
        }
    }

    public void ChangeLedColor(StatusEnum ledcolor)
    {
        if (ledcolor == StatusEnum.ONLINE)
        {
            Led.material.color = OnlineColor;
        }
        
        if (ledcolor == StatusEnum.OFFLINE)
        {
            Led.material.color = OfflineColor;
        }

        if (ledcolor == StatusEnum.CRASH)
        {
            Led.material.color = CrashColor;
        }
    }

    public Color ReturnSupposedColor()
    {
        if (Status == StatusEnum.ONLINE)
        {
            return OnlineColor;
        }
        else if (Status == StatusEnum.MAINTENANCE)
        {
            return MaintenanceColor;
        }
        else if (Status == StatusEnum.CRASH)
        {
            return CrashColor;
        }
        else if (Status != StatusEnum.ADMINLOCK)
        {
            return AdminlockColor;
        }
        else
        {
            return OfflineColor;
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
        ServerPower = 100;
        SystemBusy = false;
        ChangeLedColor(StatusEnum.ONLINE);


        yield break;
    }

    public void CrashCaller()
    {
        StartCoroutine(Crash());
    }
    public IEnumerator Crash()
    {
        Logs.EntryPoint(ServerName + " ERROR", Color.red);
        Status = StatusEnum.CRASH;
        ServerPower = 0;
        ChangeLedColor(StatusEnum.CRASH);
        StartBlink(0.5f, 0.5f, Color.red);
        RebootButton.canBePressed = true;

        yield break;
    }

    public void RebootCaller()
    {
        if (Status == StatusEnum.CRASH)
        {
            StopBlink();
            Status = StatusEnum.ONLINE;
            ServerPower = 100;
            ServerIntegrity = 100;
            Logs.EntryPoint(ServerName + " REBOOTED!", Color.green);
        }
    }


    //BLINK FUNC'S
    public void StartBlink(float BlinkTime, float BlackTime)
    {
        StopBlink();
        CanBlink = true;
        PastBlinkColor = ReturnSupposedColor();
        CurrentBlinkColor = Led.material.color;
        StartCoroutine(BlinkFunc(BlinkTime, BlackTime));
    }
    public void StartBlink(float BlinkTime, float BlackTime, Color BlinkTo)
    {
        StopBlink();
        CanBlink = true;
        PastBlinkColor = ReturnSupposedColor();
        CurrentBlinkColor = BlinkTo;
        StartCoroutine(BlinkFunc(BlinkTime, BlackTime, BlinkTo));
    }
    public void StopBlink()
    {
        CanBlink = false;
        StopCoroutine("BlinkFunc");
        Led.material.color = PastBlinkColor;
        CurrentBlinkColor = Color.clear;
    }

    private IEnumerator BlinkFunc(float BlinkTime, float BlackTime)
    {
        if (CanBlink)
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
    }
    private IEnumerator BlinkFunc(float BlinkTime, float BlackTime, Color BlinkTo)
    {
        if (CanBlink)
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

}