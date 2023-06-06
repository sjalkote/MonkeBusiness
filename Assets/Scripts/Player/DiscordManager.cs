using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordManager : MonoBehaviour {

    public Discord.Discord discord;
    private Scene scene;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        scene = SceneManager.GetActiveScene();
    }

    void Start () {
        discord = new Discord.Discord(1115659409360703538, (System.UInt64)CreateFlags.NoRequireDiscord);
        var activityManager = discord.GetActivityManager();
        var userManager = discord.GetUserManager();
        var activity = new Discord.Activity
        {
            State = $"{scene.name}",
            Details = "More Testing"
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Result.Ok)
            {
                Debug.Log("Discord RPC Success!");
            }
            else
            {
                Debug.LogWarning("Discord RPC failed");
            }
        });
    }
	
    void Update () {
        discord.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}