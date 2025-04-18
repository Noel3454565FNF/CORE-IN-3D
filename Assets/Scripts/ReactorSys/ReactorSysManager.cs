using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReactorSysManager : MonoBehaviour
{
    public static ReactorSysManager instance;
    [SerializeField]
    public List<ReactorSysServerClone> ServerArray;
    public enum StatusEnum
    {
        OFFLINE,
        ONLINE,
        MAINTENANCE,
        ADMINLOCK,
        CRASH
    };
    public StatusEnum Status = new StatusEnum();
    public enum EventEnum
    {
        NONE,
        BOOTUP,
        SHUTDOWN,
        CRASH,
        COMPLETE_CRASH,
        MAINTENANCE,
        EXIT_MAINTENANCE,
        ADMINLOCK
    };
    public EventEnum Event = new EventEnum();

    public LineClone Logs;
    public int ReactorSysAvailability = 100;
    
    public bool EventInProgress = false;
    public bool AllowServerEvent = true;
    public bool ShutdownAllowed = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int servern = 0;
        foreach (GameObject game in GameObject.FindGameObjectsWithTag("ReactorSysServer"))
        {
            if (game.GetComponent<ReactorSysServerClone>() != null)
            {
                ServerArray.Add(game.GetComponent<ReactorSysServerClone>());
                game.GetComponent<ReactorSysServerClone>().ServerName = "Server" + servern;
                game.GetComponent<ReactorSysServerClone>().Logs = Logs;
                servern++;
            }
        }
    }



    public void EventManagerCaller(EventEnum bleh)
    {
        if (EventInProgress && AllowServerEvent == false)
        {
            //NOPE
        }
        else if (EventInProgress == false && AllowServerEvent)
        {
            EventInProgress = true;
            StartCoroutine(EventManager(bleh));
        }
    }
    private IEnumerator EventManager(EventEnum Ename)
    {
        if (Ename == EventEnum.BOOTUP)
        {
            if (Status != StatusEnum.ONLINE && Status != StatusEnum.ADMINLOCK && Status != StatusEnum.MAINTENANCE && Status != StatusEnum.CRASH)
            {
                ReactorSysBootupCALLER();
                yield break;
            }
        }
        else if(Ename == EventEnum.SHUTDOWN && ShutdownAllowed)
        {
            if (Status != StatusEnum.OFFLINE &&  Status != StatusEnum.ADMINLOCK && Status != StatusEnum.MAINTENANCE && Status!= StatusEnum.CRASH)
            {

                yield break;
            }
        }
        else if(Ename == EventEnum.COMPLETE_CRASH)
        {
            if (Status != StatusEnum.OFFLINE)
            {
                foreach (ReactorSysServerClone r in ServerArray)
                {
                    r.ChangeStatus(ReactorSysServerClone.Act.CRASH);
                }
                yield break;
            }
        }
        else if(Ename == EventEnum.CRASH)
        {
            if (Status != StatusEnum.OFFLINE)
            {
                foreach(ReactorSysServerClone r in ServerArray)
                {
                    if (Random.Range(0, 100) <= 15)
                    {
                        r.ChangeStatus(ReactorSysServerClone.Act.CRASH);
                    }
                }
            }
        }
        yield break;
    }


    //Loops & logics :3
    private IEnumerator ReactorSysStat()
    {
        int e = 0;
        int count = 0;

        yield return new WaitForSeconds(0.1f);

        foreach (ReactorSysServerClone r in ServerArray)
        {
            e = e + r.ServerPower;
            count++;
        }
        ReactorSysAvailability = e / count;

        if(AllowServerEvent && Status != StatusEnum.OFFLINE)
        {
            if (Status != StatusEnum.MAINTENANCE && ReactorSysAvailability < 30)
            {
                EventManagerCaller(EventEnum.MAINTENANCE);
            }
            if (Status != StatusEnum.MAINTENANCE && ReactorSysAvailability <= 0)
            {
                
            }

                if (Status == StatusEnum.MAINTENANCE && ReactorSysAvailability > 49)
            {
                EventManagerCaller(EventEnum.EXIT_MAINTENANCE);
            }
        }
        else if (Status == StatusEnum.CRASH)
        {
            if (ReactorSysAvailability == 100)
            {
                EventInProgress = false;
                Status = StatusEnum.ONLINE;
                foreach(ReactorSysServerClone r in ServerArray)
                {
                    r.ChangeLedColor(ReactorSysServerClone.StatusEnum.ONLINE);
                }
                Logs.EntryPoint("Main Computer Restored!", Color.green);
            }
        }
        StartCoroutine(ReactorSysStat());
        yield break;
    }

    //EVENTS

    public void ReactorSysBootupCALLER()
    {
        StartCoroutine(BootUp());
    }
    private IEnumerator BootUp()
    {
        foreach(ReactorSysServerClone r in ServerArray)
        {
            r.ChangeStatus(ReactorSysServerClone.Act.BOOTUP);
        }

        yield return new WaitForSeconds(11f);

        Status = StatusEnum.ONLINE;
        EventInProgress = false;
        Logs.EntryPoint("Main Computer ONLINE!", Color.green);
        foreach(ReactorSysServerClone r in ServerArray)
        {
            r.ChangeLedColor(ReactorSysServerClone.StatusEnum.ONLINE);
        }
        StartCoroutine(ReactorSysStat());

        yield break;
    }

    public void ENTERING_MAINTENANCE_Caller()
    {
        StartCoroutine("MAINTENANCE_MODE");
    }
    private IEnumerator MAINTENANCE_MODE()
    {
        Status = StatusEnum.MAINTENANCE;
        foreach (ReactorSysServerClone r in ServerArray)
        {
            r.ChangeStatus(ReactorSysServerClone.Act.ENTER_MAINTENANCE);
            yield return new WaitForSeconds(0.2f);
        }
        Logs.EntryPoint("!ENTERING MAINTENANCE MODE!", COREManager.instance.LineWarnColor);
        EventInProgress = false;

        yield break;
    }

    public void EXIT_MAINTENANCE_Caller()
    {
        StartCoroutine("MAINTENANCE_EXIT");
    }
    private IEnumerator MAINTENANCE_EXIT()
    {
        Status = StatusEnum.ONLINE;
        foreach(ReactorSysServerClone r in ServerArray)
        {
            r.ChangeStatus(ReactorSysServerClone.Act.EXIT_MAINTENANCE);
        }
        Logs.EntryPoint("EXITING MAINTENANCE MODE", Color.green);
        yield break;
    }
}
