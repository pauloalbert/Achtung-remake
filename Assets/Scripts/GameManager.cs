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
    private GameObject powerupsParentObject;
    private Settings settings;

    private bool pressedPause = false;

    // true if game is frozen, false if not
    [SerializeField] private bool frozen = true;

    [SerializeField] private GameState roundState = GameState.ONGOING;

    // list of Players that were instantiated
    private List<PlayerController> activePlayers;

    // current player scores sorted by player number
    [SerializeField] private List<int> scores;

    private float minPowerupTime;
    private float maxPowerupTime;

    private float nextPowerupTime; // time to next powerup spawn

    // TODO: make range values depend on borders ((system might change))
    public float xRange = 50;
    public float yRange = 40;

    void Awake()
    {
        scores = new List<int>();
        activePlayers = new List<PlayerController>();

        playersParentObject = GameObject.Find("Players");
        powerupsParentObject = GameObject.Find("Powerups");
        settings = GameObject.Find("Settings").GetComponent<Settings>();
    }

    void Start()
    {
        settings.initUsedPowerups();
        minPowerupTime = settings.initialMinPowerupTime;
        maxPowerupTime = settings.initialMaxPowerupTime;
        newGame();
    }

    void FixedUpdate()
    {   
        switch (roundState)
        {
        case GameState.ONGOING: // Mid round
        {
            if(pressedPause) {
                pressedPause = false; // kind of like on press of button maybe change later
                toggleFreeze();
            }

            if(!frozen) // game running and not paused
            {
                powerupHandler();
                roundState = checkGameState();
            }
        }
        break;
        case GameState.ENDED: // Round ended
        {
            if(pressedPause){
                pressedPause = false;
                nextRound();
            }
        }
        break;
        case GameState.WON: // round was won, called once
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
        case GameState.TIED: // round was tied, called once
        {
            roundState = GameState.ENDED;
            freeze();

            Debug.Log("Tied");
        }
        break;
        }
    }


    // Starts a new game
    public void newGame()
    {
        // if players exist, delete them
        Utilities.deleteAllChildren(playersParentObject);

        createPlayers();

        // initialize scores list
        scores.Clear();
        for(int i=0; i < settings.numberOfPlayers; i++) scores.Add(0);

        // start first round
        nextRound();
    }

    // Revives players and sets new locations to start next round
    public void nextRound()
    {
        freeze();
        // TODO: reset relevant timers and values
        roundState = GameState.ONGOING;
        deleteAllPowerups();
        deleteAllTrails();
        removeAllTimers();
        scatterPlayers();
        reviveAll();
    }

    // getters
    public List<PlayerController> getActivePlayers()
    {
        return activePlayers;
    }
    public List<int> getScores()
    {
        return scores;
    }

    // Instantiates players and saves PlayerControllers in activePlayers list
    void createPlayers()
    {
        activePlayers.Clear();

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

    private void createRandomPowerup()
    {
        // create random powerup
        string powerup = settings.usedPowerups[Random.Range(0 , settings.usedPowerups.Count)];
        GameObject powerupObject = Instantiate(settings.PowerupPrefabs[powerup], powerupsParentObject.transform) as GameObject;

        // choose and set powerup type
        Powerup power = powerupObject.GetComponent<Powerup>();
        List<PowerupType> types = power.availableTypes;
        int typeIndex = Random.Range(0,types.Count);
        power.setPowerupType(types[typeIndex]);
        

        // make random location
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, yRange);
        Vector3 location = new Vector3(x,y,0);

        powerupObject.transform.position = location;
    }

    private void newPowerupTimer()
    {
        nextPowerupTime = Random.Range(minPowerupTime, maxPowerupTime);
    }

    private void powerupHandler()
    {
        if(nextPowerupTime <= 0)
        {
            newPowerupTimer();
            createRandomPowerup();
        }
        else
        {
            nextPowerupTime -= Time.fixedDeltaTime;
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

    private void removeAllTimers()
    {
        foreach (PlayerController player in activePlayers)
        {
            player.resetTimers();
        }
        newPowerupTimer();
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

    public void deleteAllPowerups()
    {
        Utilities.deleteAllChildren(powerupsParentObject);
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
