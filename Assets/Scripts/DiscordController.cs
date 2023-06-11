using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    public long applicationID = 1115659409360703538;
    [Space]
    public string details = "Monkeying Around";
    public string state = "";
    [Space]
    public string largeImage = "banana_pile";
    public string largeText = "";

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                details = "In the Main Menu"; state = "Staring at the screen";
                largeImage = "banana_pile"; largeText = "MonkeBusiness";
                break;
            case "MainLevel": 
                details = "Monkeying Around"; state = "Current velocity: ";
                largeImage = "banana_pile"; largeText = "Defending Bananas";
                _character = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
                break;
            default:
                details = "Monkeying Around";
                state = "";
                largeImage = "banana_pile";
                largeText = "";
                Debug.LogWarning($"This scene '{scene.name}' does not have a case in DiscordController.cs");
                break;
        }
    }

    void Start()
    {
        // Log in with the Application ID
        Debug.Log("Starting Discord Status");
        _discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
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
            if (_character != null) state += _character.velocity.ToString();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets = { LargeImage = largeImage, LargeText = largeText },
                Timestamps = { Start = _time }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed to connect to Discord: " + res);
            });
        }
        catch {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit() { _discord.Dispose(); }
}