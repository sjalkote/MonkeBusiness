using System;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
    public long applicationID = 1115659409360703538;
    [Space]
    // TODO: Add status when in Main Menu, make sure to move the GameObject to that scene instead of this one
    public string details = "Monkeying Around";
    public string state = "Current velocity: ";
    [Space]
    public string largeImage = "banana_pile";
    public string largeText = "Defending Bananas";

    private CharacterController _character;
    private long _time;

    private static bool _instanceExists;
    private Discord.Discord _discord;

    void Awake() 
    {
        // Transition the GameObject between scenes, destroy any duplicates
        if (!_instanceExists)
        {
            _instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Log in with the Application ID
        _discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        _character = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        _time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try { _discord.RunCallbacks(); }
        catch { Destroy(gameObject); }
    }

    void LateUpdate() 
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        // Update Status every frame
        try
        {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state + _character.velocity,
                Assets = { LargeImage = largeImage, LargeText = largeText },
                Timestamps = { Start = _time }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed to connect to Discord: " + res);
            });
        }
        catch { // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit() { _discord.Dispose(); }
}