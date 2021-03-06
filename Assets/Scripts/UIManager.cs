using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    private GameObject canvas;
    private TextMeshProUGUI scoreboard;
    private TextMeshProUGUI goal;

    void Awake()
    {
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
        foreach (PlayerController player in GameManager.Instance.getActivePlayers())
        {
            board += ("<color=#"+ColorUtility.ToHtmlStringRGBA(player.color)+">"
            +player.name+" "+GameManager.Instance.getScores()[player.playerNum-1]+"</color>\n");
        }
        scoreboard.SetText(board);
    }
    void updateGoalText()
    {
        goal.SetText("Goal\n<size=240%>"+Settings.Instance.goal);
    }
}
