using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class RegenHandler : MonoBehaviour
{

    /// <summary>
    /// no regen yet sorry
    /// </summary>
    public static RegenHandler instance;

    /// <summary>
    /// register every task currently running to kill them cuz unity engine is not doing it smh...
    /// </summary>
    public List<Task> TaskRunning;

    private Application apl;


    [Header("IMPORTANT")]
    public bool AppRunning;



    //private void OnApplicationQuit()
    //{
    //    Time.timeScale = 0;
    //}


    private void Awake()
    {
        instance = this;
        AppRunning = true;
    }

    private void OnApplicationQuit()
    {
        AppRunning = false;
    }


    private void Start()
    {
        
    }
}


public class tweeningF : MonoBehaviour
{

    public int dan = 0;
    public bool alreadytweening = false;

    public static tweeningF instance;


    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// use the "dan" variable present in this class to get the returned int :p
    /// you CAN'T RUN THIS FONCTION AT THE SAME TIME. clone this class first :p.
    /// </summary>
    /// <param name="from">value to start</param>
    /// <param name="to">value to go</param>
    /// <param name="time">how much time to from -> to</param>
    /// <returns></returns>
    public async Task TweenNumbersValue(float from, float to, float time)
    {
        if (alreadytweening == false && RegenHandler.instance.AppRunning)
        {
            alreadytweening = true;
            LeanTween.value(from, to, time)
                .setOnUpdate((float t) =>
                {
                    dan = Mathf.FloorToInt(t);
                })
                .setOnComplete(() =>
                {
                    dan = 0;
                    alreadytweening = false;
                });
        }
        else
        {
            UnityEngine.Debug.LogError("I WAS ALREADY TWEENING SMT DUMBASS.");
        }

    }

}