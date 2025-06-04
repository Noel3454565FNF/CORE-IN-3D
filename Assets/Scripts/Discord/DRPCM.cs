using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using Unity.VisualScripting;
using Newtonsoft.Json;

public class DRPCM : MonoBehaviour
{

    [Header("value you may touch")]
    public static DRPCM instance;
    public long ClientID;
    public bool AllowDiscordRPCUpdate = true;
    public float DiscordUpateTime = 3f;
    public long currentUserId;
    public string UserName;
    public string UserAvatar;
    [SerializeField]
    public UserManager UM;

    public Discord.Discord discord;
    private Discord.ActivityManager aM;
    private Discord.Activity dA;
    private User currentUser;

    private void Start()
    {
        instance = this;
        if (ClientID == null)
        {
            Debug.LogError("CLIENTID CANNOT BE NULL.");
            //Application.Quit();
        }
        print("RPC loading...");
        discord = new Discord.Discord(ClientID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        UM = discord.GetUserManager();
        UM.OnCurrentUserUpdate += () =>
        {
            currentUser = UM.GetCurrentUser();
            currentUserId = (long)currentUser.Id;
            UserName = currentUser.Username;
            UserAvatar = $"https://cdn.discordapp.com/avatars/{currentUser.Id}/{currentUser.Avatar}.png";

        };

        aM = discord.GetActivityManager();
        dA = new Discord.Activity
        {
            Details = "logging in",
            State = "idle",
        };
        aM.ClearActivity((res) =>
        {
            print(res);
        });
        StartCoroutine(ActivityAutoUpdate());
    }


    IEnumerator ActivityAutoUpdate()
    {
        aM.UpdateActivity(dA, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                //nothing to say everything is fine
                //print("RPC LOADED!");


            }
            else
            {
                print("RPC ERROR!");
            }
        });


        if (AllowDiscordRPCUpdate)
        {
            yield return new WaitForSeconds(DiscordUpateTime);
            StartCoroutine(ActivityAutoUpdate());
        }
    }

    private void Update()
    {
        discord.RunCallbacks();
    }



    /// <summary>
    /// change rpc main infos (details/state).
    /// </summary>
    /// <param name="details">main text</param>
    /// <param name="state">status display</param>
    public void ChangeRPCTextInfo(string details, string state)
    {
        dA.Details = details;
        dA.State = state;
    }

    public void ChangeRPCTextInfo(string details)
    {
        dA.Details = details;
    }

    /// <summary>
    /// change rpc small image and his overlay text.
    /// </summary>
    /// <param name="smallimageurl">small image url(cannot be null)</param>
    /// <param name="smallimagetext">small image text</param>
    public void ChangeRPCSmallImageInfo(string smallimageurl, string smallimagetext)
    {
        if (smallimageurl != null)
        {
            dA.Assets.SmallImage = smallimageurl;
            dA.Assets.SmallText = smallimagetext;
        }
        else
        {
            Debug.LogError("IMAGE URL CANNOT BE NULL. I WARNED YOU MF.");
        }
    }

    /// <summary>
    /// change rpc large(or big whatever) and his overlay text.
    /// </summary>
    /// <param name="bigimageurl">big image url(cannot be null)</param>
    /// <param name="bigimagetext">big image text</param>
    public void ChangeRPCBigImageInfo(string bigimageurl, string bigimagetext)
    {
        if (bigimageurl != null)
        {
            dA.Assets.LargeImage = bigimageurl;
            dA.Assets.LargeText = bigimagetext;
        }
        else
        {
            Debug.LogError("IMAGE URL CANNOT BE NULL. I WARNED YA MF.");
        }
    }
}
