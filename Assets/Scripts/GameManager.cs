using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum GameState
{
    ONGOING, // mid round
    WON, // dunno
    TIED, // dunno 
    ENDED // dunno
}

public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;

    private GameObject playersParentObject; // object containing all players in game
    private GameObject powerupsParentObject; // object containing all powerups in the map

    private bool paused = false;
    // true if game is frozen, false if not (not sure why i did frozen and paused will change)
    private bool frozen = true;

    // Current state of game
    private GameState roundState = GameState.ONGOING;

    // list of instantiated players' playerControllers
    private List<PlayerController> activePlayers = new List<PlayerController>();

    // current player scores sorted by player number
    private List<int> scores = new List<int>();

    private float minPowerupTime;
    private float maxPowerupTime;

    private float nextPowerupTime; // time to next powerup spawn

    // TODO: make range values depend on borders ((system might change))
    public float xRange = 50;
    public float yRange = 40;

    void Start()
    {
        playersParentObject = GameObject.Find("Players");
        powerupsParentObject = GameObject.Find("Powerups");

        Settings.Instance.initUsedPowerups();
        minPowerupTime = Settings.Instance.initialMinPowerupTime;
        maxPowerupTime = Settings.Instance.initialMaxPowerupTime;
        newGame();
    }

    void FixedUpdate()
    {   
        switch (roundState)
        {
        case GameState.ONGOING: // Mid round
        {
            if(paused)
            {
                paused = false; // kind of like on press of button maybe change later
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
            if(paused){
                paused = false;
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

            if(scores[winner.playerNum-1] >= Settings.Instance.goal)
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
        for(int i=0; i < Settings.Instance.numberOfPlayers; i++) scores.Add(0);

        // start first round
        nextRound();
    }

    // Revives players and sets new locations to start next round
    public void nextRound()
    {
        freeze();

        deleteAllPowerups();
        deleteAllTrails();
        removeAllTimers();

        scatterPlayers();
        reviveAll();

        foreach (PlayerController player in activePlayers)
        {
            player.setDefaultValues();
        }
        roundState = GameState.ONGOING;
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
        for(int i=0; i < Settings.Instance.numberOfPlayers; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab, playersParentObject.transform) as GameObject;

            PlayerController playerController = playerObject.GetComponent<PlayerController>();
            PlayerInput playerInput = playerController.GetComponent<PlayerInput>();

            // Set player number
            playerController.playerNum = i+1;
            // Set name
            playerObject.name = Settings.Instance.names[i];
            playerController.playerName = Settings.Instance.names[i];
            // Set color
            playerController.color = Settings.Instance.colors[i];
            // Set controls
            playerInput.actions["Left"].ApplyBindingOverride(Settings.Instance.controlPaths[i][0]);
            playerInput.actions["Right"].ApplyBindingOverride(Settings.Instance.controlPaths[i][1]);

            activePlayers.Add(playerController);
        }
    }

    private void createRandomPowerup()
    {
        if(Settings.Instance.usedPowerups.Count == 0) return; // no powerups

        // create random powerup
        string powerup = Settings.Instance.usedPowerups[Random.Range(0 , Settings.Instance.usedPowerups.Count)]; // choose powerup
        GameObject powerupObject = Instantiate(Settings.Instance.PowerupPrefabs[powerup], powerupsParentObject.transform) as GameObject;

        // choose and set powerup type
        Powerup power = powerupObject.GetComponent<Powerup>();
        List<PowerupType> types = power.powerupSettings.AvailableTypes;
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

    // called when pause button is pressed
    public void OnPause(InputAction.CallbackContext value)
    {
        paused = value.performed;
    }
}
