using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STABSLasers : MonoBehaviour
{
    [Header("Component")]
    [HideInInspector] public MeshRenderer MR;
    public GameObject[] GMstabsParts;
    [HideInInspector] public MeshRenderer[] MRstabsParts;
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
        MR = gameObject.GetComponent<MeshRenderer>();
        var lol = 0;
        foreach(GameObject gm in GMstabsParts)
        {
            MRstabsParts[lol] = gm.GetComponent<MeshRenderer>();
            lol++;
        }
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (StabStatus == "ONLINE" | StabStatus == "OVERLOADED")
        {
            float rotationSpeed = RPM * Time.deltaTime;
            transform.Rotate(rotationAxis * rotationSpeed);
        }
    }
}
