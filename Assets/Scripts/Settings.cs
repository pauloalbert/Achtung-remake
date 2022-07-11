using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Singleton<Settings>
{
    [Header("=== Game Settings ===")]
    [Space(20)]

    [SerializeField] [Min(2)] public int numberOfPlayers = 2;

    // score to reach to win the game
    public int goal = 10;

    [Space(20)]

    [Header("=== Player Settings ===")]
    [Space(20)]


   

    // array of player names sorted by player number
    public string[] names =
    {
        "Fred",
        "Greenlee",
        "Pinkey",
        "Bluebell",
        "Willem",
        "Greydon"
    };

    // array of the player colors sorted by player number
    public Color[] colors = new Color[]
    {
            Color.red,
            Color.green,
            new Color(1f,0.7f,0.8f),
            Color.cyan,
            new Color(1f,0.6f,0f),
            Color.gray
    };

    // array of player control paths sorted by player number
    public string[][] controlPaths =
    {
        new string[] {"<Keyboard>/#(a)", "<Keyboard>/#(s)"},
        new string[] {"<Keyboard>/LeftArrow", "<Keyboard>/RightArrow"},
        new string[] {"<Keyboard>/#(,)", "<Keyboard>/#(.)"},
        new string[] {"<Keyboard>/#(c)", "<Keyboard>/#(v)"},
        new string[] {"<Keyboard>/#([)", "<Keyboard>/#(])"},
        new string[] {"<Keyboard>/#(`)", "<Keyboard>/#(1)"}
    };

    [Range(0.2f, 20f)] public float initialSize = 1f;
    [Range(1f,50f)] public float initialSpeed = 17f;

    [Range(0.1f,10f)] public float initialTurnSharpness = 2f;
    [Range(0,10f)] public float initialHoleLength = 3.5f;

    [Range(0.3f,4f)] public float initialMinHoleDelay = 1;
    [Range(4f,10f)] public float initialMaxHoleDelay = 6;

    [Space(20)]

    [Header("=== Powerup Settings ===")]
    [Space(20)]

    [Range(0.5f,10f)] public float initialMinPowerupTime = 2f; // min time for powerup to spawn
    [Range(4f,10f)] public float initialMaxPowerupTime = 5f; // max time for powerup to spawn

    [Range(0,1)] public float initialTimerWidth = 0.5f;
    [Range(0,2)] public float initialTimerOffset = 1;

    [Space(10)]
    // list of powerups being used in game
    public List<string> usedPowerups;
    // list of powerups being used in game that should be given to a player
    public List<string> playerPowerups;
    // dictionary with powerup prefabs
    public Dictionary<string,GameObject> PowerupPrefabs;

    [Space(6)]
    [Header("Speed")]
    public bool speedActive = true;
    public PowerupSettings speedSettings = 
        new PowerupSettings("speed", 5f, 1f, true, new List<PowerupType> {PowerupType.GREEN, PowerupType.RED} );
    [Range(0.2f, 5f)] public float speedAdd = 0.5f; // amont that ist aded to speed with pseed peoweru p

    [Space(6)]
    [Header("Slow")]
    public bool slowActive = true;
    public PowerupSettings slowSettings = 
        new PowerupSettings("slow", 5f, 1f, true, new List<PowerupType> {PowerupType.GREEN, PowerupType.RED} );

    [Space(6)]
    [Header("Clear Screen")]
    public bool clearScreenActive = true;
    public PowerupSettings clearScreenSettings = 
        new PowerupSettings("clearScreen", 0f, 1f, true, new List<PowerupType> {PowerupType.BLUE} );

    [Space(6)]
    [Header("Reverse")]
    public bool reverseActive = true;
    public PowerupSettings reverseSettings = 
        new PowerupSettings("reverse", 5f, 1f, true, new List<PowerupType> {PowerupType.RED} );

    [Space(6)]
    [Header("Fat")]
    public bool fatActive = true;
    public PowerupSettings fatSettings = 
        new PowerupSettings("fat", 5f, 1f, true, new List<PowerupType> {PowerupType.GREEN, PowerupType.RED} );

    [Space(6)]
    [Header("Thin")]
    public bool thinActive = true;
    public PowerupSettings thinSettings = 
        new PowerupSettings("thin", 5f, 1f, true, new List<PowerupType> {PowerupType.GREEN, PowerupType.RED} );

    [Range(0.2f, 5f)] public float fatMultiplier = 2f;
    [Range(1f, 5f)] public float holeFatMultiplier = 1.2f;

    [Space(6)]
    [Header("Invincible")]
    public bool invincibleActive = true;
    public PowerupSettings invincibleSettings = 
        new PowerupSettings("invincible", 5f, 1f, true, new List<PowerupType> {PowerupType.GREEN} );

    [Space(6)]
    [Header("Square")]
    public bool squareActive = true;
    public PowerupSettings squareSettings = 
        new PowerupSettings("square", 9f, 1f, true, new List<PowerupType> {PowerupType.GREEN, PowerupType.RED} );
    

    public void initUsedPowerups()
    {
        usedPowerups = new List<string>();
        PowerupPrefabs = new Dictionary<string, GameObject>();

        if(speedActive)
        { 
            usedPowerups.Add("speed");
            playerPowerups.Add("speed");
            PowerupPrefabs["speed"] = Resources.Load<GameObject>("SpeedPowerupPrefab");
        }
        if(slowActive)
        { 
            usedPowerups.Add("slow");
            playerPowerups.Add("slow");
            PowerupPrefabs["slow"] = Resources.Load<GameObject>("SlowPowerupPrefab");
        }
        if(clearScreenActive)
        {
            usedPowerups.Add("clearScreen");
            PowerupPrefabs["clearScreen"] = Resources.Load<GameObject>("ClearScreenPowerupPrefab");
        }
        if(reverseActive)
        {
            usedPowerups.Add("reverse");
            playerPowerups.Add("reverse");
            PowerupPrefabs["reverse"] = Resources.Load<GameObject>("ReversePowerupPrefab");
        }
        if(fatActive)
        {
            usedPowerups.Add("fat");
            playerPowerups.Add("fat");
            PowerupPrefabs["fat"] = Resources.Load<GameObject>("FatPowerupPrefab");
        }
        if(thinActive)
        { 
            usedPowerups.Add("thin");
            playerPowerups.Add("thin");
            PowerupPrefabs["thin"] = Resources.Load<GameObject>("ThinPowerupPrefab");
        }
        if (invincibleActive)
        {
            usedPowerups.Add("invincible");
            playerPowerups.Add("invincible");
            PowerupPrefabs["invincible"] = Resources.Load<GameObject>("InvinciblePowerupPrefab");
        }
        if (squareActive)
        {
            usedPowerups.Add("square");
            playerPowerups.Add("square");
            PowerupPrefabs["square"] = Resources.Load<GameObject>("SquarePowerupPrefab");
        }
    }

}
