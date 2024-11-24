using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STABSLasers : MonoBehaviour
{
    [Header("Component")]
    public GameObject[] GMstabsParts;
    public MeshRenderer[] MRstabsParts;
    [HideInInspector] public GameObject Rotor;
    public COREManager Cm;


    [Header("Vars")]
    public string StabStatus = "OFFLINE";
    public int StabTemp = 26;
    public float RPM = 0f;
    public string PendingEvent = "none";
    public int Power = 0;


    [Header("Optional Vars")]
    public Vector3 rotationAxis = Vector3.up;


    void Start()
    {
        var lol = 0;
        foreach(GameObject gm in GMstabsParts)
        {
            MRstabsParts[lol] = gm.GetComponent<MeshRenderer>();
            lol++;
            if (gm.name == "Rotor")
            {
                Rotor = gm;
            }
        }
    }

    void Update()
    {

    }


    public IEnumerator STABTEMPCHECK()
    {


        yield return new WaitForSeconds(1f);
    }

    private void FixedUpdate()
    {
        if (StabStatus == "ONLINE" | StabStatus == "OVERLOADED")
        {
            float rotationSpeed = RPM * Time.deltaTime;
            Rotor.transform.Rotate(rotationAxis * rotationSpeed);
        }
    }
}
