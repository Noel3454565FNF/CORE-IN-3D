using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorGrid : MonoBehaviour
{
    [Header("Component")]
    [HideInInspector]public static ReactorGrid instance;


    [Header("Power things")]
    public int PowerUsage;
    public int PowerInput;
    public int PowerOutput;





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
        
    }
}
