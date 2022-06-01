using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum GameState
{
    ONGOING,
    WON,
    TIED,
    ENDED
}

public class GameManager : MonoBehaviour // TODO: make class singleton
{
    public GameObject playerPrefab;
    private GameObject playersParentObject;
    private Settings settings;

    private bool pressedPause = false;

    // true if game is frozen, false if not
    [SerializeField] private bool frozen = true;

    [SerializeField] private GameState roundState = GameState.ONGOING;

    // list of Players that were instantiated
    private List<PlayerController> activePlayers;

    // current player scores sorted by player number
    [SerializeField] private int[] scores = {0,0,0,0,0,0};

    // TODO: make range values depend on borders ((system might change))
    public float xRange = 40;
    public float yRange = 30;

    void Awake()
    {
        playersParentObject = GameObject.Find("Players");
        settings = GameObject.Find("Settings").GetComponent<Settings>();
    }

    void Start()
    {
        createPlayers();
        scatterPlayers();
    }

    void Update()
    {   
        if(!frozen)
        {
            GameState state = checkGameState();
            switch(state)
            {
                case GameState.WON:
                {
                    PlayerController winner = getWinner();
                    Debug.Log(winner.playerName + " Wins!");
                    winner.wins += 1;
                    freeze();
                }
                break;
                case GameState.TIED:
                {
                    Debug.Log("Tied");
                    freeze();
                }
                break;
            }
        }

    // getters
    public List<PlayerController> getActivePlayers()
    {
        return activePlayers;
    }
    public int[] getScores()
    {
        return scores;
    }

    // Instantiates players and saves PlayerControllers in activePlayers list
    void createPlayers()
    {

        activePlayers = new List<PlayerController>();

        // Instantiate players
        for(int i=0; i < numberOfPlayers; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerParent.transform) as GameObject;

            PlayerController playerController = playerObject.GetComponent<PlayerController>();
            PlayerInput playerInput = playerController.GetComponent<PlayerInput>();

            // Set name
            playerController.playerName = names[i];
            // Set color
            playerController.color = colors[i];
            // Set controls
            playerInput.actions["Left"].ApplyBindingOverride(controlPaths[i][0]);
            playerInput.actions["Right"].ApplyBindingOverride(controlPaths[i][1]);

            activePlayers.Add(playerController);
        }
    }

    // randomizes all player locations
    void scatterPlayers()
    {
        foreach (PlayerController playerController in activePlayers)
        {
            randomizeLocation(playerController);
        }
    }

    // returns current game state
    private GameState checkGameState()
    {
        switch(aliveCount())
        {
            case 0: return GameState.TIED;
            case 1: return GameState.WON;
        }
        return GameState.ONGOING;
    }

    // Returns amount of players alive currently
    public int aliveCount()
    {
        int alive = 0;
        foreach (PlayerController playerController in activePlayers)
        {
            if (playerController.isAlive()) alive++;
        }
        return alive;
    }

    // Returns ref to PlayerController that won, null if no winner
    public PlayerController getWinner()
    {
        if(aliveCount() == 1)
        {
            foreach (PlayerController playerController in activePlayers)
            {
                if(playerController.isAlive()) return playerController;
            }
            return null; // unreachable
        }
        else return null;
    }

    // Revives all active players
    public void reviveAll()
    {
        foreach (PlayerController playerController in activePlayers)
        {
            playerController.revive();
        }
    }

    // delete all player trails
    public void deleteAllTrails()
    {
        foreach (PlayerController playerController in activePlayers)
        {
            playerController.deleteTrail();
        }
    }

    // Revives players and sets new locations 
    public void resetGame()
    {
        // TODO: reset relevant timers and values
        scatterPlayers();
        deleteAllTrails();
        reviveAll();
    }

    public bool isFrozen()
    {
        return frozen;
    }

    public void freeze()
    {
        frozen = true;
    }

    public void toggleFreeze()
    {
        if(!frozen) freeze();
        else frozen = false;
    }
}
