using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MCFS : MonoBehaviour
{
    public bool canUpdate = true;
    [HideInInspector]public static MCFS instance;
    [Header("Important Vars")]
    public int ShieldIntegrity = 0;
    public Color ShieldIntegrityC = new Color(0, 255, 0);
    public int ShieldPower = 0;
    public int ShieldMaxPower;
    public int ShieldEnergyReserve;
    public int ShieldEnergyReserveMax;
    public enum ShieldStatusEnum
    {
        Online,
        Offline,
        Error,
        AdminLock,
        Overload
    }
    public string ShieldStatus = ShieldStatusEnum.Offline.ToString();
    public bool CanShieldDegrad = false;
    public int ShieldDegradationSpeed = 1;

    public int SystemIntegrity = 100;

    public bool canUpdateShieldIntegrityColor = true;
    public bool canUpdateMCFSStabilityColor = true;

    public bool canShieldRegenerate = false;

    public bool FirstTimeDegradation = true;
    public bool FirstTimeRegeneration = true;

    [Header("Component")]
    public GameObject Shield;
    [HideInInspector] public MeshRenderer SMR;

    [Header("Screen Component")]
    public TextMeshProUGUI integrityTxt;
    public TextMeshProUGUI MCFSSystemIntegrityTxt;

    public UnityEngine.UI.Image ShieldImage;
    public TextMeshProUGUI UnknownImage;
    public RawImage WarningSignImage;



    private void Awake()
    {
        instance = this;
    }


    
    public void ShieldCreation(int to, float time)
    {
        var frome = ShieldIntegrity;
        var too = to;
        LeanTween.value(frome, too, time)
            .setOnUpdate((float t) =>
            {
                ShieldIntegrity = Mathf.CeilToInt(t);
            });
    }


    public void ShieldKYS()
    {
        //either using an animation or a can script it
        //idk im lazy :3
        ShieldStatus = ShieldStatusEnum.Error.ToString();

        Shield.transform.LeanScale(new Vector3(10, 10, 10), 0.3f)
            .setOnUpdate((float t) =>
            {
                Vector3 lol = Vector3.Lerp(Shield.transform.localScale, new Vector3(15, 15, 15), t);
                Shield.transform.localScale = lol;
            });

        LeanTween.color(this.gameObject, new Color(0, 0, 0, 0), 0.2f)
            .setOnUpdate((float t) =>
            {
                Color currentC = Color.Lerp(Shield.GetComponent<MeshRenderer>().material.color, new Color(0, 0, 0, 0), t);
                Shield.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", currentC);
            });
    }


    public IEnumerator ShieldDegradationFunc()
    {
        yield return new WaitForSeconds(ShieldDegradationSpeed);
        var t = ShieldIntegrity - 1;
        if (t > -1 && CanShieldDegrad)
        {
            ShieldIntegrity = ShieldIntegrity - 1;
            StartCoroutine(ShieldDegradationFunc());
        }
        else
        {
            StopCoroutine(ShieldDegradationFunc());
        }

    }

    private void Start()
    {
        print(ShieldStatus);
        SMR = Shield.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (canUpdate)
        {
            if (ShieldAvalabilityCheck())
            {
                ShieldPower = 10 * (ShieldMaxPower / (int)ShieldIntegrity);
            }
            if (canUpdateShieldIntegrityColor)
            {
                var ouhe = (ShieldIntegrity * 255) / 100;
                integrityTxt.color = new Color(255 - ouhe, ouhe, 0);

                integrityTxt.text = ShieldIntegrity.ToString() + "%";
            }
            if (canUpdateMCFSStabilityColor)
            {
                var heheheha = (SystemIntegrity * 255) / 100;
                MCFSSystemIntegrityTxt.color = new Color(255 - heheheha, heheheha, 0);
                //print(MCFSSystemIntegrityTxt.color.r);
                MCFSSystemIntegrityTxt.text = SystemIntegrity.ToString() + "%";
            }
        }
    }

    public bool ShieldAvalabilityCheck()
    {
        if (ShieldStatus != ShieldStatusEnum.Offline.ToString() && ShieldStatus != ShieldStatusEnum.Error.ToString() && ShieldStatus != ShieldStatusEnum.Overload.ToString())
        {
            return true;
        }
        return false;
    }



    //SHIELD SCREEN FONCTIONS
    public void ShieldToOffline()
    {
        UnknownImage.enabled = true;
        WarningSignImage.gameObject.active = false;
        ShieldImage.gameObject.active = false;

        UnknownImage.color = Color.white;
    }

    public void ShieldToUnknownThreat()
    {
        UnknownImage.enabled = true;
        WarningSignImage.gameObject.active = false;
        ShieldImage.gameObject.active = false;

        UnknownImage.color = new Color(160, 32, 240);
    }

    public void ShieldToOnline()
    {
        UnknownImage.enabled = false;
        WarningSignImage.gameObject.active = false;
        ShieldImage.gameObject.active = true;
    }

    public void ShieldToWarning()
    {
        UnknownImage.enabled = false;
        WarningSignImage.gameObject.active = true;
        ShieldImage.gameObject.active = false;
    }




    //SHIELD ANIM
    public IEnumerator ShieldCreationV2()
    {
        Shield.transform.localScale = new Vector3(20, 20, 20);
        Shield.transform.LeanScale(new Vector3(10, 10, 10), 4)
            .setEaseInCirc()
            .setOnUpdate((Vector3 v) =>
            {
                Shield.transform.localScale = v;
            });

        yield break;
    }
}
