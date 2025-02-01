using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutdown : MonoBehaviour
{
public static Shutdown instance;





    private void Awake()
    {
        instance = this;    
    }


}
