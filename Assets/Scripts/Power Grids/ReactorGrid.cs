using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition.Attributes;

public class ReactorGrid : MonoBehaviour
{
    [Header("Component")]
    [HideInInspector]public static ReactorGrid instance;
    public COREManager CM;
    public MCFS cc;
    private Coroutine gridMathCoroutine;



    [Header("Power things")]
    public int PowerUsage;
    public int CurrentPower;
    public int Power;
    public int ReactorGridIntegrity = 100;
    public bool CanGridUpdate = true;

    [Header("Powe usage thingies")]
    public int StabMaxPowerUsage;
    public int StabTotalPowerUsage;

    public int MCFSMaxPowerUsage;
    public int MCFSTotalPowerUsage;

    public int CoreMaxTempThing;
    public int CoreMaxPowerProduction;
    public int CoreTotalPowerProduction;

    [Header("Auxiliary Power")]
    public int AuxGridPowerUsage;
    public int AuxGridMaxPower;
    public int AuxGridStability;

    [Header("Status")]
    
    [InspectorName("Grid Current Status")]public string GridSTS = GridStatus.ONLINE.ToString();
    
    [SerializeField]
    public enum GridStatus
    {
        ONLINE,
        OFFLINE,
        ERROR,
        DAMAGED,
        OUTAGE,
        OVERUSED,
        SURGE
    };
    public int ReactorGridStability = 100;


    [Header("Grid events")]
    public bool GridAllowSurge = true;




    [Header("DEBUG")]
    public int beansInTheSheinFactory;
    public int totalPower;
    public int deduction;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //GridMath();
        Task.Run(GridStatusUpdate);
        StartGridMath();
    }

    // Update is called once per frame
    public async Task GridStatusUpdate()
    {
        if (RegenHandler.instance.AppRunning)
        {
            if (COREManager.instance.CoreAllowGridEvent)
            {
                if (Power < 0 && GridSTS != GridStatus.OUTAGE.ToString())
                {
                    //power outage
                    GridOutageCaller();
                }
                if (Power > 23000 && GridSTS != GridStatus.SURGE.ToString())
                {
                    //power surge
                    GridSTS = GridStatus.SURGE.ToString();
                    Task.Run(GridSurgeFunc);
                }
                if (Power < 23000 && GridSTS == GridStatus.SURGE.ToString())
                {
                    //bai surge
                    GridSTS = GridStatus.ONLINE.ToString();
                }
                await Task.Delay(0250);
                Task.Run(GridStatusUpdate);
            }
        }
    }


    public void StartGridMath()
    {
        if (gridMathCoroutine == null)
        {
            print("h");
            gridMathCoroutine = StartCoroutine(GridMathCoroutine());
        }
    }

    public void StopGridMath()
    {
        if (gridMathCoroutine != null)
        {
            StopCoroutine(gridMathCoroutine);
            gridMathCoroutine = null;
        }
    }

    private IEnumerator GridMathCoroutine()
    {
        while (RegenHandler.instance.AppRunning && CanGridUpdate)
        {
            // Reset power values
            StabTotalPowerUsage = 0;
            CoreTotalPowerProduction = 0;
            totalPower = 0;
            deduction = 0;

            //print("ReactorGridSys: RUNNING");

            // Get the stabilizers list
            List<STABSLasers> stabilizers = COREManager.instance.Stablist;

            // Calculate stabilizer power usage
            foreach (STABSLasers stab in stabilizers)
            {
                if (stab.Power != 0)
                {
                    print("stab power update in 2028!");
                    StabTotalPowerUsage += (StabMaxPowerUsage * stab.Power)/100;
                }
            }

            // Calculate shield power usage
            if (cc.ShieldPower != 0)
            {
                var r = 0;
                r = MCFSMaxPowerUsage * cc.ShieldPower; //600 * 10
                MCFSTotalPowerUsage = r / 100;
                print($"mf update now! {MCFSTotalPowerUsage} or {cc.ShieldPower} or {r}");
            }

            // Calculate core power production
            if (CM.CoreTemp != 0)
            {
                beansInTheSheinFactory = CoreMaxTempThing / (CM.CoreTemp * 100);

                if (beansInTheSheinFactory != 0)
                {
                    CoreTotalPowerProduction = (beansInTheSheinFactory * 100) / CoreMaxPowerProduction;
                    print($"core update now! {CoreTotalPowerProduction}");
                }
            }

           // print($"StabTotalPowerUsage: {StabTotalPowerUsage} && MCFSTotalPowerUsage: {MCFSTotalPowerUsage}");

            // Calculate total power and deduction
            deduction = StabTotalPowerUsage + MCFSTotalPowerUsage;
            totalPower = deduction - CoreTotalPowerProduction;

            // Debug information
            //print($"Total Power: {totalPower}");
            //print($"Deduction: {deduction}");

            // Update power value
            Power = totalPower;

            // Wait for 250ms before the next loop iteration
            yield return new WaitForSeconds(0.500f);
        }
    }


    //EVENTS
    public IEnumerator GridOutagefunc()
    {
        if (RegenHandler.instance.AppRunning && COREManager.instance.CoreInEvent == false)
        {
            foreach(STABSLasers stab in CM.Stablist)
            {
                stab.StabOutage();
            }
            print("REACTOR GRID OUTAGE DETECTED!");
            GridSTS = GridStatus.OUTAGE.ToString();
            CanGridUpdate = false;
            CM.ReactorSysLogsScreen.EntryPoint("POWER LOSS DETECTED!", Color.red);

            yield return StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM));

            yield return new WaitForSeconds(12); // Replaces Task.Delay(7000)
            CM.ReactorSysLogsScreen.EntryPoint("Redirecting power from auxiliary power grid...", Color.red);

            yield return new WaitForSeconds(7); // Replaces Task.Delay(3000)
            yield return StartCoroutine(LightsManager.GLM.LevelNeg3LightsControl(1, 1000, Negate3roomsName.CORE_CONTROL_ROOM));

            CM.ReactorSysLogsScreen.EntryPoint("ReactorGrid Online!", Color.green);
            GridSTS = GridStatus.ONLINE.ToString();
            foreach (STABSLasers stab in CM.Stablist)
            {
                stab.StabExitOutage();
            }
            print("awe :3");
        }
    }

    public void GridOutageCaller()
    {
        StartCoroutine(GridOutagefunc());
    }


    public async Task GridSurgeFunc()
    {
        while(RegenHandler.instance.AppRunning && GridSTS == GridStatus.SURGE.ToString() && GridAllowSurge)
        {
            ReactorGridIntegrity += -1;
            Debug.LogWarning("ReactorGrid loosing integrity!!!");
            await Task.Delay(Random.Range(3000, 10000));
        }
    }
}
