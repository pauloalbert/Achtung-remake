using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Settings settings;
    public GameManager gameManager;
    public GameObject canvas;
    public TextMeshProUGUI scoreboard;
    public TextMeshProUGUI goal;

    void Awake()
    {
        settings = GameObject.Find("Settings").GetComponent<Settings>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvas = GameObject.Find("UICanvas");
        scoreboard = GameObject.Find("Scoreboard").GetComponent<TextMeshProUGUI>();
        goal = GameObject.Find("Goal").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        updateGoalText();
    }
    // Update is called once per frame
    void Update()
    {
        updateScoreboard();
    }

    void updateScoreboard() // TODO: sort by score
    {
        string board = "";
        foreach (PlayerController player in gameManager.getActivePlayers())
        {
            board += ("<color=#"+ColorUtility.ToHtmlStringRGBA(player.color)+">"
            +player.name+" "+gameManager.getScores()[player.playerNum-1]+"</color>\n");
        }
        scoreboard.SetText(board);
    }
    void updateGoalText()
    {
        goal.SetText("Goal\n<size=240%>"+settings.goal);
    }
}
