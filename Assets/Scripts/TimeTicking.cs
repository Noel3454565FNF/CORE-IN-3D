using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TimeTicking : MonoBehaviour
{
    [Header("Components")]
    public TMPro.TextMeshPro timetext;


    [Header("Values")]
    public bool QUITTING = false;
    public bool CanUpdate = false;
    public int Milliseconds = 0;
    public int Seconds = 0;
    public int Minutes = 0;


    private void OnApplicationQuit()
    {
        QUITTING = true;
    }




    public async Task Millisecond()
    {
        if (QUITTING == false && CanUpdate == true)
        {



            Task.Delay(0001);



        }
        else
        {
            //FORCE TO QUIT SO NO SHITTY AAAAH UPDATE.
        }
    }


    private void Update()
    {
        if (QUITTING == false && CanUpdate == true)
        {
            timetext.text = Minutes + ":" + Seconds + ":" + Milliseconds;
        }
    }


}
