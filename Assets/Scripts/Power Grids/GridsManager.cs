using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsManager : MonoBehaviour
{
    [Header("Grids")]
    public ReactorGrid reactorgrid;

    [Header("Value")]
    public int GlobalPower = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GlobalPowerManager()
    {
        int powerAdds = 0;
        if (reactorgrid.GridActive)
        {
            powerAdds += reactorgrid.Power;
        }


        GlobalPower = powerAdds;
        yield break;
    }
}
