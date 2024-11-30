using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Startup : MonoBehaviour
{
    [Header("Stuff")]
    public COREManager CM;
    private Utility utility;

    [Header("Plr")]
    public CameraFollowAndControl plrCAM;

    [Header("Stabs")]
    public STABSLasers Stab1;
    public STABSLasers Stab2;
    public System.Action test;

    [Header("Core")]
    public MeshRenderer Core;
    public MeshRenderer CoreShield;

    public Vector3 CoreSize;
    public Vector3 CoreShieldSize;

    [Header("Lights")]
    public Light light;

    public void Start()
    {
        utility = new Utility();
        CoreStarup();
    }

    public async Task CoreStarup()
    {
        await Task.Delay(5000);

        Mathf.Lerp(light.intensity, 0f, Time.deltaTime);

        CM.CoreEvent = "STARTUP";
        Task.Run(Stab1.StabStart);
        await Task.Run(Stab2.StabStart);
        Stab1.Laser.gameObject.SetActive(true);
        Stab2.Laser.gameObject.SetActive(true);

        plrCAM.TriggerScreenShake(1.7f, 7f);

        CoreShield.gameObject.transform.LeanScale(CoreShieldSize, 1.5f);
        Core.gameObject.transform.LeanScale(CoreSize, 1.5f);

        CM.CoreStatus = "ONLINE";
        CM.CoreEvent = "none";

    }

}
