using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public static EMP instance;
    public GameObject EMPCLONETHING;

    private void Awake()
    {
        instance = this;
    }



    public EMPClone NewEMP()
    {
        GameObject k = GameObject.Instantiate(EMPCLONETHING, COREManager.instance.COREMeshRenderer.gameObject.transform, false);
        EMPClone clone = k.AddComponent<EMPClone>();
        clone.create(k);
        return clone;
        
    }

}

public class EMPClone:MonoBehaviour
{

    public Vector3 EMPnormal = new Vector3(30, 30, 30);
    public Vector3 EMPcontained = new Vector3(15, 15, 15);
    public Vector3 EMPrunAway = new Vector3(60, 60, 60);

    public GameObject WAVE;
    public int EMPStrengh;
    public COREManager CM;

    public int recontainmentChance = 0;
    public int dissipationChance = 0;

    private void Awake()
    {
        
    }

    public void create(GameObject Base, int EMPStrenghOVERRIDE = 0)
    {
        WAVE = Base;
        LeanTween.value(WAVE, WAVE.transform.position, EMPnormal, 0.7f)
            .setEaseInOutQuad()
            .setOnUpdate((Vector3 v)
            =>
            {
                WAVE.transform.position = v;
            });

        if (EMPStrenghOVERRIDE != 0)
        {
            EMPStrengh = EMPStrenghOVERRIDE;
        }
        else
        {
            EMPStrengh = Random.Range(1, 100);
        }

        //math
        
    }
}
