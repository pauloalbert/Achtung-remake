using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]

    [SerializeField] private bool rightPressed;
    [SerializeField] private bool leftPressed;

    [Space(10)]

    [Header("Player Values")]

    [Tooltip("Value for move direction: 1, 0, -1 for left forward and right respectivley.")]
    [SerializeField] private int turnDirection = 0;

    [Tooltip("true if player is alive, false if dead.")]
    [SerializeField] private bool alive = true;

    [SerializeField] private float turnSharpness;
    [SerializeField] private float velocityMagnitude;

    [Tooltip("The angle the player is pointing to in radians.")]
    [SerializeField] private float angle;

    [SerializeField] private Vector2 velocityVector;
    [Tooltip("Normalized Vector2 pointing to player moving direction.")]
    [SerializeField] private Vector2 direction;

    [Tooltip("Time from last hole")]
    private float holeTimer = 0;
    [Tooltip("after this time passes from last hole a new hole will be made")]
    private float nextHoleDelay;
    [Tooltip("When randomizing how much time till the next hole, this is the maximium value it can take")]
    [SerializeField] private float maxHoleDelay;
    [Tooltip("When randomizing how much time till the next hole, this is the minimum value it can take")]
    [SerializeField] private float minHoleDelay;
    [Tooltip("Time duration of a hole")]
    [SerializeField] private float holeDuration;

    public string playerName = ""; // needs to be setup in game manager
    public int playerNum; // player number

    [Tooltip("A dictionary that maps names of powerups to the amount of times they are active")]
    private Dictionary<string, int> activePowerups = new Dictionary<string, int>();

    [Space(10)]

    [Header("Objects")]

    [Tooltip("Trail piece prefab.")]
    public GameObject trailPiecePrefab;
    [Tooltip("Player's body object.")]
    private GameObject body;
    [Tooltip("Player's trail object.")]
    private GameObject trail;
    [Tooltip("Player's trail color.")]
    public Color color;

    // refrence to GameManager and Settings
    private GameManager gameManager;
    private Settings settings;

    

    // Awake is called when script is initalized
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        settings = settings = GameObject.Find("Settings").GetComponent<Settings>();
        body = gameObject.transform.Find("Body").gameObject;
        trail = gameObject.transform.Find("Trail").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        setInitialValues();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void FixedUpdate()
    {


        // deltaTime is used if fixedupdate doesn't run at the right speed, would make it feel like slowed time.
        // Both with and without are good, what matters is that **both angle and translate depend on deltatime** if any.

        if(alive && !gameManager.isFrozen()){
            calculateValues();
            updateTrailTimer();
            Move();
        }
    }

    // gets initial values from settings
    private void setInitialValues()
    {
        newHoleDelay();
        velocityMagnitude = settings.initialSpeed;
        turnSharpness = settings.initialTurnSharpness;
        holeDuration = settings.initialHoleDuration;
        minHoleDelay = settings.initialMinHoleDelay;
        maxHoleDelay = settings.initialMaxHoleDelay;
        initializeDictionary();
    }

    private void initializeDictionary()
    {
        foreach (string key in settings.powerups)
        {
            activePowerups.Add(key, 0);
        }
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        rightPressed = value.ReadValueAsButton();
    }

    public void OnLeft(InputAction.CallbackContext value)
    {
        leftPressed = value.ReadValueAsButton();
    }

    private void getInput()
    {
        turnDirection = (leftPressed ? 1 : 0) - (rightPressed ? 1 : 0);
    }

    //TODO: rename?
    private void calculateValues()
    {
        // calculate angle
        angle += turnDirection * turnSharpness * Time.deltaTime;
        angle = VectorUtilities.clampAngle(angle);

        // calculate vectors
        velocityVector = VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle);
        direction = velocityVector.normalized;

    }

    // Moves player
    private void Move()
    {
        body.transform.position += new Vector3(velocityVector.x,velocityVector.y,0); // move body

        VectorUtilities.rotateObject(body,angle); // rotate body to looking angle

        spawnTrail(); // create trail
    }

    // Kills the player (add necessary stuff later)
    public void kill()
    {
        alive = false;
        gameManager.giveAllAlivePoints();
    }

    // Bring player back to life
    public void revive()
    {
        alive = true;
    }

    // getter for alive value
    public bool isAlive()
    {
        return alive;
    }

    // TODO: worry about this showing up on screen?
    // spawnTrail takes in a direction d and angle a. instatiates trailPiece in the location
    // 1 quarter of the radius of player to the opposite direction from the movement and
    // matches the size according to the radius of player
    public void spawnTrail()
    {
        if (isSpawningTrail() && alive)
        {
            GameObject trailPiece = Instantiate(trailPiecePrefab, trail.transform) as GameObject; // create trail piece
            Vector3 d3 = new Vector3(direction.x, direction.y, 0);
            float radius = body.transform.localScale.x;
            trailPiece.transform.position = body.transform.position - 0.25f * radius * d3; // move piece
            trailPiece.transform.localScale = new Vector3(radius, radius / 2, 0); // scale piece
            VectorUtilities.rotateObject(trailPiece, angle); // rotate piece
            trailPiece.GetComponent<SpriteRenderer>().color = color; // set color
        }
        else return;
    }

    // randomizes the hole daly and changes the nextHoleDelay accordingly. Also resets the holeTime Counter
    public void newHoleDelay()
    {
        nextHoleDelay = Random.Range(minHoleDelay,maxHoleDelay);
    }

    // updates all fields that have to do with timing holes. if holeTimer > nextHoleDelay + holeDuration,
    // sets holeTimer to 0, and calls newHoleDelay(). otherwise only adds deltaTime to holeTimer
    public void updateTrailTimer()
    {
        if (holeTimer > nextHoleDelay + holeDuration)
        {
            holeTimer = 0;
            newHoleDelay();
        }
        else holeTimer += Time.deltaTime;
    }

    // returns true if currently need to spawn trail false otherwise
    public bool isSpawningTrail()
    {
        if (holeTimer > nextHoleDelay && holeTimer < nextHoleDelay + holeDuration)
        {
            return false;
        }

        else return true;
    }

    // Delete player trail
    public void deleteTrail()
    {
        foreach (Transform child in trail.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Rotates and moves player to a random area in the map range
    public void randomizeLocation()
    {
        // make random values for starting rotation and position
        angle = Random.Range(0, 2f * Mathf.PI);
        float x = Random.Range(-gameManager.xRange, gameManager.xRange);
        float y = Random.Range(-gameManager.yRange, gameManager.yRange);
        Vector3 location = new Vector3(x,y,0);
        // rotate and move
        VectorUtilities.rotateObject(body,angle);
        body.transform.position = location;
    }

   

    public void setHoleDuration(float duration)
    {
        holeDuration = duration;
    }

    public void setVelocityMagnitude(float velocity)
    {
        velocityMagnitude = velocity;
    }
     
    // TODO: think about concurency issues in items!
    public void setActivePowerup(string key, int value)
    {
        activePowerups[key] = value;
    }

    public int getActivePowerup(string key)
    {
        return activePowerups[key];
    }
}
