using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour // TODO: make class singleton
{
    [Header("Game settings")]

    [Min(2)] public int numberOfPlayers = 2;

    // score to reach to win the game
    public int goal = 10;

    [Space(20)]

    [Header("Player settings")]

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

    [Range(0.2f, 5f)] public float initialSize = 1f;
    [Range(0.2f, 5f)] public float fatMultiplier = 2.5f;
    [Range(1f, 5f)] public float holeFatMultiplier = 1.2f;
    [Range(1f,50f)] public float initialSpeed = 17f;
    [Range(0.1f,10f)] public float initialTurnSharpness = 2f;
    [Range(0.05f,5f)] public float initialHoleDuration = 0.2f;
    [Range(0.3f,4f)] public float initialMinHoleDelay = 1;
    [Range(4f,10f)] public float initialMaxHoleDelay = 6;


    [Range(0.5f,10f)] public float initialMinPowerupTime = 2f;
    [Range(4f,10f)] public float initialMaxPowerupTime = 5f;

    [Space(20)]

    [Header("Powerups")]
    
    // list of powerups being used in game
    public List<string> usedPowerups;
    // list of powerups being used in game that should be given to a player
    public List<string> playerPowerups;

    public Dictionary<string,GameObject> PowerupPrefabs;

    
    public bool speed = true;

    public bool clearScreen = true;

    public bool fat = true;

    public bool invincibleActive = true;

    public void initUsedPowerups()
    {
        usedPowerups = new List<string>();
        PowerupPrefabs = new Dictionary<string, GameObject>();

        if(speed){ 
            usedPowerups.Add("speed");
            playerPowerups.Add("speed");
            PowerupPrefabs["speed"] = Resources.Load<GameObject>("SpeedPowerupPrefab");
        }
        if(clearScreen){
            usedPowerups.Add("clearScreen");
            PowerupPrefabs["clearScreen"] = Resources.Load<GameObject>("ClearScreenPowerupPrefab");
        }
        if(fat)
        {
            usedPowerups.Add("fat");
            playerPowerups.Add("fat");
            PowerupPrefabs["fat"] = Resources.Load<GameObject>("FatPowerupPrefab");

        }
        if (invincibleActive)
        {
            usedPowerups.Add("invincible");
            playerPowerups.Add("invincible");
            PowerupPrefabs["invincible"] = Resources.Load<GameObject>("InvinciblePowerupPrefab");

        }
    }

}
