using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum GameState
{
    ONGOING,
    WON,
    TIED
}

public class GameManager : MonoBehaviour // TODO: make class singleton
{
    public GameObject playerPrefab;
    public GameObject playerParent;
    public int numberOfPlayers = 1;

    // true if game is frozen, false if not
    [SerializeField] private bool frozen = true;

    public const int maxPlayers = 6;

    public string[] names =
    {
        "Fred",
        "Greenlee",
        "Pinkey",
        "Bluebell",
        "Willem",
        "Greydon"
    };

    // array ordered by player number of their respective colors
    public Color[] colors = new Color[]
    {
            Color.red,
            Color.green,
            new Color(1f,0.7f,0.8f),
            Color.cyan,
            new Color(1f,0.6f,0f),
            Color.gray
    };

    // array ordered by player number of their respective control paths
    public string[][] controlPaths =
    {
        new string[] {"<Keyboard>/#(a)", "<Keyboard>/#(s)"},
        new string[] {"<Keyboard>/LeftArrow", "<Keyboard>/RightArrow"},
        new string[] {"<Keyboard>/#(,)", "<Keyboard>/#(.)"},
        new string[] {"<Keyboard>/#(c)", "<Keyboard>/#(v)"},
        new string[] {"<Keyboard>/#([)", "<Keyboard>/#(])"},
        new string[] {"<Keyboard>/#(`)", "<Keyboard>/#(1)"},
    };

    // TODO: make range values depend on borders
    public float xRange = 40;
    public float yRange = 30;

    // list of Players that were instantiated
    private List<PlayerController> activePlayers;

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
        else{ // temp way to unfreeze game
            if(activePlayers[0].rightPressed)
            {
                frozen = false;
            }
        }
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

    // Gets a PlayerController, rotates and moves player to a random area in the settings range
    void randomizeLocation(PlayerController player)
    {
        // make random values for starting rotation and position
        player.angle = Random.Range(0, 2f * Mathf.PI);
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, yRange);
        Vector3 location = new Vector3(x,y,0);
        // rotate and move
        player.rotateObject(player.body,player.angle);
        player.body.transform.position = location;
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
}
