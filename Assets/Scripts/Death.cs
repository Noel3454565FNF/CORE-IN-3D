using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{


    [Header("Death related")]
    public static Death instance;
    public enum DeathReason
    {
        sd,
        meltdown,
        freezedown,
        freeze,
        kill
    };

    [Header("Death count")]
    public int SdDeathCount;
    public int KillDeathCount;

    [Header("Limbo related")]
    public bool Confirm = false;
    public LimboTalkingObjectAlt[] Sdv1;

    [Header("Text machine & stuff")]
    public TMPro.TextMeshProUGUI txt;
    public RawImage BG;
    public AudioSource AS;
    public AudioClip Ambiance;
    public int CurrentDiag;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        instance = this;
    }


    private IEnumerator TeleportToLimbo(DeathReason where, string useless = "a")
    {
        SceneManager.LoadSceneAsync("Limbo");
        SceneManager.UnloadSceneAsync("Main");

        Debug.LogError("LIMBOOOO");

        yield return new WaitForSeconds(1f);

        txt = GameObject.Find("text").GetComponent<TMPro.TextMeshProUGUI>();
        BG = GameObject.Find("BG").GetComponent<RawImage>();
        AS = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        if (where == DeathReason.sd)
        {
            Next(Sdv1[SdDeathCount]);
        }

        yield break;
    }

    public void TeleportToLimbo(DeathReason where)
    {
        StartCoroutine(TeleportToLimbo(where, ""));
    }




    //!!!!!ONLY USE IN LIMBO!!!!!


    private IEnumerator TextMachine(LimboTalkingObject thing, LimboTalkingObjectAlt Cache)
    {
        string text = thing.Text;
        txt.text = "";

        foreach (char c in text)
        {
            yield return new WaitForSeconds(thing.WaitingTimePerCharacter);
            txt.text += c;
        }
        print("Finish");
        yield return new WaitForSeconds(thing.WaitingTime);

        if (thing.TheEnd)
        {
            //MAKE IT SO IT CHECK IF ITS IN THE EDITOR OR NORMAL
            //i'll do it later: Noel
            //you wont do it.....: Grey
            //gosh i'll do it: Thread
            //Nooooooooo i wanted to do it :( Noel345
            //too slow~ Grey
            //GRRRRRRRR: Noel345
            SceneManager.LoadSceneAsync("Main");
            SceneManager.UnloadSceneAsync("Limbo");
        }
        else
        {
            CurrentDiag++;
            Next(Cache);
        }

        yield break;
    }


    private void Next(LimboTalkingObjectAlt t)
    {
        StartCoroutine(TextMachine(t.variant[CurrentDiag], t));
    }
}


[System.Serializable]
public class LimboTalkingObject
{
    /// <summary>
    /// Text that will be say
    /// </summary>
    public string Text;

    /// <summary>
    /// The color of the text
    /// </summary>
    public Color TextColor = new Color(255, 255, 255);

    /// <summary>
    /// Time to wait until new text (timer start when text writing is over)
    /// </summary>
    public float WaitingTime = 3f;

    /// <summary>
    /// Time to wait for script to write another character
    /// </summary>
    public float WaitingTimePerCharacter = 0.1f;

    /// <summary>
    /// The end, aka: tick if its the last dialogue
    /// </summary>
    public bool TheEnd = false;
}


[System.Serializable]
public class LimboTalkingObjectAlt
{

    [SerializeField]
    public LimboTalkingObject[] variant;

}