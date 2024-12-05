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
    public AudioSource audioSource;

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

    [Header("Audios")]
    public AudioClip StartupThingi;
    public AudioClip lightsSound;

    public void Start()
    {
        utility = new Utility();
        CoreStarup();
    }

    public async Task CoreStarup()
    {
        await Task.Delay(5000);

        LightsManager.GLM.LevelNeg3LightsControl(0, 1000, Negate3roomsName.CORE_CONTROL_ROOM);

        CM.CoreEvent = "STARTUP";
        audioSource.clip = StartupThingi;
        audioSource.Play();
        Task.Run(Stab1.StabStart);
        Task.Run(Stab2.StabStart);

        await Task.Delay(21000);

        Stab1.Laser.gameObject.SetActive(true);
        Stab2.Laser.gameObject.SetActive(true);

        plrCAM.TriggerScreenShake(5f, 7f);

        CoreShield.gameObject.transform.LeanScale(CoreShieldSize, 5f);
        Core.gameObject.transform.LeanScale(CoreSize, 5f);
        await Task.Delay(9000);

        LightsManager.GLM.LevelNeg3LightsControl(1, 1000, Negate3roomsName.CORE_CONTROL_ROOM);

        CM.CoreStatus = "ONLINE";
        CM.CoreEvent = "none";

    }

}
