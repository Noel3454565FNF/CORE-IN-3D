using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPS : MonoBehaviour
{

    [Header("Component")]
    private COREManager CM;
    private STABSLasers stab1;
    private STABSLasers stab2;
    private STABSLasers stab3;
    private STABSLasers stab4;
    private STABSLasers stab5;
    private STABSLasers stab6;
    private List<STABSLasers> stabs = new List<STABSLasers>();
    public AudioClip PowerPurgeAudio;
    public AudioSource AudioPlayer;


    [Header("Value")]
    public bool AllowPurge = false;
    public bool ForcePurgeSucess = false;
    public bool ForcePurgeFailure = false;

    public int PurgeCount = 0;
    public int PurgeFailureChance = 0;




    // Start is called before the first frame update
    void Start()
    {
        CM = COREManager.instance;
        CM.Stab1 = stab1;
        CM.Stab2 = stab2;
        CM.Stab3 = stab3;
        CM.Stab4 = stab4;
        CM.Stab5 = stab5;
        CM.Stab6 = stab6;
        stabs.Add(stab1);
        stabs.Add(stab2);
        stabs.Add(stab3);
        stabs.Add(stab4);
        stabs.Add(stab5);
        stabs.Add(stab6);
        AudioPlayer.clip = PowerPurgeAudio;
    }

    
    public void POWERPURGECALLER()
    {

    }

    IEnumerator POWERPURGE()
    {
        AudioPlayer.Play();
        CM.CanUpdateTemp = false;
        foreach(STABSLasers stab in stabs)
        {
            //stab.
        }
        var hehe = true;
        
        if (ForcePurgeFailure && hehe)
        {
            hehe = false;
        }

        if (ForcePurgeSucess == hehe)
        {
            hehe = false;
        }



        yield return new WaitForSeconds(1);
    }


    IEnumerator SUCCESS()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator FAILURE()
    {
        yield return new WaitForSeconds(1);
    }


}
