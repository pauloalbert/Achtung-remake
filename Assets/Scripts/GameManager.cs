using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerParent;
    public int numberOfPlayers = 1;

    public bool paused = false; // currently does nothing

    public const int maxPlayers = 6;

    public Color[] colors = new Color[]
    {
            Color.red,
            Color.green,
            new Color(1f,0.7f,0.8f),
            Color.cyan,
            new Color(1f,0.6f,0f),
            Color.gray
    };

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
    private List<Player> activePlayers;

    void Start()
    {
        SetupPlayers();
    }

    void SetupPlayers()
    {

        activePlayers = new List<Player>();

        for(int i=0; i < numberOfPlayers; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerParent.transform) as GameObject;
            Player player = playerObject.GetComponent<Player>();
            PlayerInput playerInput = player.GetComponent<PlayerInput>();

            // make random values for starting rotation and position
            player.angle = Random.Range(0, 2f * Mathf.PI);
            float x = Random.Range(-xRange, xRange);
            float y = Random.Range(-yRange, yRange);
            Vector3 spawnLocation = new Vector3(x,y,0);
            // rotate and move
            player.rotateObject(player.body,player.angle);
            player.body.transform.position = spawnLocation;

            // TODO: add stuff to player (like controls and stuff)
            playerInput.actions["left"].ApplyBindingOverride(controlPaths[i][0]);
            playerInput.actions["Right"].ApplyBindingOverride(controlPaths[i][1]);
            player.color = colors[i];

            activePlayers.Add(player);
        }
    }

}
