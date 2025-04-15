using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorSysManager : MonoBehaviour
{
    public static ReactorSysManager instance;
    public List<ReactorSysServerClone> ServerArray;
    public enum StatusEnum
    {
        ONLINE,
        OFFLINE,
        MAINTENANCE,
        ADMINLOCK,
        CRASH
    };
    public StatusEnum Status = new StatusEnum();
    public enum EventEnum
    {
        BOOTUP,
        SHUTDOWN,
        CRASH,
        MAINTENANCE,
        ADMINLOCK
    };
    public EventEnum Event = new EventEnum();

    public LineClone Logs;
    public int ReactorSysAvailability = 100;
    
    public bool EventInProgress = false;
    public bool ShutdownAllowed = false;

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
        if (EventInProgress)
        {
            //NOPE
        }
        else
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
        yield break;
    }



    //EVENTS

    public void ReactorSysBootupCALLER()
    {

    }
    private IEnumerator BootUp()
    {

        yield break;
    }
}
