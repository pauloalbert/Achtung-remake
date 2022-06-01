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

    void FixedUpdate()
    {   
        switch (roundState)
        {
            case GameState.ONGOING:
            {
                if(pressedPause) {
                    pressedPause = false;
                    toggleFreeze();
                }

                if(!frozen)
                {
                    roundState = checkGameState();
                }
            }
            break;
            case GameState.ENDED:
            {
                if(pressedPause){
                    pressedPause = false;
                    nextRound();
                }
            }
            break;
            case GameState.WON: 
            {
                roundState = GameState.ENDED;
                freeze();

                PlayerController winner = getWinner();
                Debug.Log(winner.playerName + " Wins!");

                if(scores[winner.playerNum-1] >= settings.goal)
                {
                    winGame(winner);
                }
            }
            break;
            case GameState.TIED:
            {
                roundState = GameState.ENDED;
                    freeze();

                    Debug.Log("Tied");
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
        for(int i=0; i < settings.numberOfPlayers; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab, playersParentObject.transform) as GameObject;

            PlayerController playerController = playerObject.GetComponent<PlayerController>();
            PlayerInput playerInput = playerController.GetComponent<PlayerInput>();

            // Set player number
            playerController.playerNum = i+1;
            // Set name
            playerObject.name = settings.names[i];
            playerController.playerName = settings.names[i];
            // Set color
            playerController.color = settings.colors[i];
            // Set controls
            playerInput.actions["Left"].ApplyBindingOverride(settings.controlPaths[i][0]);
            playerInput.actions["Right"].ApplyBindingOverride(settings.controlPaths[i][1]);

            activePlayers.Add(playerController);
        }
    }

    // randomizes all player locations
    void scatterPlayers()
    {
        foreach (PlayerController playerController in activePlayers)
        {
            playerController.randomizeLocation();
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
    public void nextRound()
    {
        freeze();
        // TODO: reset relevant timers and values
        roundState = GameState.ONGOING;
        scatterPlayers();
        deleteAllTrails();
        reviveAll();
    }

    // Starts a new game
    public void newGame()
    {
        // delete players and start over
    }

    // Gets the PlayerController that won the game, does winning stuff
    private void winGame(PlayerController winner)
    {

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

    // give all players that are alive a point
    public void giveAllAlivePoints()
    {
        foreach(PlayerController player in activePlayers)
        {
            if(player.isAlive()){
                scores[player.playerNum-1]++;
            }
        }
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        pressedPause = value.performed;
    }
}
