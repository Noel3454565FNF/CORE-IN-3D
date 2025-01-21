using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ReactorGrid : MonoBehaviour
{
    [Header("Component")]
    [HideInInspector]public static ReactorGrid instance;


    [Header("Power things")]
    public int PowerUsage;
    public int CurrentPower;
    public int Power;
    public int ReactorGridIntegrity = 100;
    public bool CanGridUpdate = true;

    [Header("Status")]
    
    [InspectorName("Grid Current Status")]public string GridSTS = GridStatus.ONLINE.ToString();
    [SerializeField]public enum GridStatus
    {
        ONLINE,
        OFFLINE,
        ERROR,
        DAMAGED,
        OUTAGE,
        OVERUSED,
        SURGE
    };




    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RegenHandler.instance.AppRunning && CanGridUpdate)
        {
            Power = CurrentPower - PowerUsage;
            if (Power <= 0 && GridSTS != GridStatus.OUTAGE.ToString())
            {
                //power outage
                GridSTS = GridStatus.OUTAGE.ToString();
                GridOutagefunc();
            }
            if (Power > 7500 && GridSTS != GridStatus.SURGE.ToString())
            {
                //power surge
                GridSTS = GridStatus.SURGE.ToString();
            }
            if (Power < 7500 && GridSTS == GridStatus.SURGE.ToString())
            {
                //bai surge
                GridSTS = GridStatus.ONLINE.ToString();
            }
        }
    }

    public void GridMath()
    {
        while (RegenHandler.instance.AppRunning && CanGridUpdate)
        {
            print("ReactorGridSys: RUNNING");
        }
    }


    //EVENTS
    public async Task GridOutagefunc()
    {
        while (RegenHandler.instance.AppRunning)
        {
            GridSTS = GridStatus.OUTAGE.ToString();
            CanGridUpdate = false;
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("POWER LOSS DETECTED!", COREManager.instance.LineAttentionColor);
            LightsManager.GLM.LevelNeg3LightsControl(0, 0, Negate3roomsName.CORE_CONTROL_ROOM);
            await Task.Delay(7000);
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("Redicting power from auxiliary power grid...", COREManager.instance.LineWarnColor);
            await Task.Delay(3000);
            LightsManager.GLM.LevelNeg3LightsControl(1, 0, Negate3roomsName.CORE_CONTROL_ROOM);
            COREManager.instance.ReactorSysLogsScreen.EntryPoint("ReactorGrid Online!", Color.green);
            GridSTS = GridStatus.ONLINE.ToString();
            break;
        }
    }
    
    public async Task GridSurgeFunc()
    {
        while(RegenHandler.instance.AppRunning && GridSTS == GridStatus.SURGE.ToString())
        {
            ReactorGridIntegrity += -1;
            Debug.LogWarning("ReactorGri loosing integrity!!!");
            await Task.Delay(Random.Range(3000, 10000));
        }
    }
}
