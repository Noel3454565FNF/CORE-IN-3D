using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CPM : MonoBehaviour
{
    public ColorBind[] CB;

    void start()
    {
        if (CB != null)
        {
            foreach (ColorBind cb in CB)
            {
                if (cb.MR != null && cb.color != null)
                {
                    cb.MR.material.color = cb.color;
                }
            }
        }
    }
}

[System.Serializable]
public class ColorBind
{
    public string Name;
    public MeshRenderer MR;
    public Color color;
}
