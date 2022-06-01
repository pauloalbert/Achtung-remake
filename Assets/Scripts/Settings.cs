using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    [Min(2)] public int numberOfPlayers = 2;
    public const int maxPlayers = 6;
    // score to reach to win the game
    public int goal = 10;

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
        new string[] {"<Keyboard>/#(`)", "<Keyboard>/#(1)"},
    };

    [Range(1f,50f)] public float initialSpeed = 17f;
    [Range(0.1f,10f)] public float initialTurnSharpness = 2f;
    [Range(0.05f,5f)] public float initialHoleDuration = 0.2f;
    [Range(0.3f,4f)] public float initialMinHoleDelay = 1;
    [Range(4f,10f)] public float initialMaxHoleDelay = 6;

}
