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
