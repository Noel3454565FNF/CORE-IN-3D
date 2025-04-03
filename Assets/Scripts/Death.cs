using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update


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

    public int SdDeathCount;
    public int KillDeathCount;
    //[Header("Limbo related")]



    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
        instance = this;
    }


    public IEnumerator TeleportToLimbo(DeathReason where)
    {
        SceneManager.LoadSceneAsync("Limbo");
        SceneManager.UnloadSceneAsync("Main");

        yield return new WaitForSeconds(1f);

        yield break;
    }






    //!!!!!ONLY USE IN LIMBO!!!!!


}


[SerializeField]
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

public class LimboTalkingObjectAlt
{

    public LimboTalkingObject[] ObjAlt;

}