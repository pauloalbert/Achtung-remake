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
    [SerializeField] private bool reversedDirection = false;

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
    private Dictionary<string, List<float>> activePowerups;

    private Vector3 bodyPosition; // current body position
    private Vector3 lastBodyPosition; // body position last frame


    [Space(10)]

    [Header("Objects")]

    
    public Sprite circleSprite;
    public Sprite squareSprite;
    [Tooltip("Trail piece prefab.")]
    public GameObject trailPiecePrefab;
    [Tooltip("Player's body object.")]
    private GameObject body;
    [Tooltip("Player's trail object.")]
    private GameObject trail;
    [Tooltip("Player's helpre collider when it is a square object.")]
    private GameObject squareHelp;
    [Tooltip("Player's trail color.")]
    public Color color = Color.white;
   

    [Header("Powerup counters and other variables")]
    private int speedCount = 0;
    private int fatCount = 0;
    private bool invincible = false;
    [Tooltip("True in the physics frame a fat powerup was picked up")]
    public bool fatFrame = false;



    [Header("Square Fields")]
    private bool isSquare = false;
    private bool rightSquare = false;
    private bool leftSquare = false;

    



    // Awake is called when script is initalized
    void Awake()
    {
        // find game objects
        body = gameObject.transform.Find("Body").gameObject;
        squareHelp = body.transform.Find("SquareHelpCollider").gameObject;
        trail = gameObject.transform.Find("Trail").gameObject;
        // sets initial size
        // TODO: why here?
        body.transform.localScale = new Vector3(Settings.Instance.initialSize, Settings.Instance.initialSize, 0);
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
        if ((Input.GetKeyDown(KeyCode.RightArrow) && playerName == "Greenlee") || (Input.GetKeyDown(KeyCode.S) && playerName == "Fred"))
        {
            rightSquare = true;
        }
        
        if ((Input.GetKeyDown(KeyCode.LeftArrow)&& playerName == "Greenlee") || (Input.GetKeyDown(KeyCode.A) && playerName == "Fred"))
        {
            leftSquare = true;
        }
        

    }

    private void FixedUpdate()
    {
        // deltaTime is used if fixedupdate doesn't run at the right speed, would make it feel like slowed time.
        // Both with and without are good, what matters is that **both angle and translate depend on deltatime** if any.

        if(alive && !GameManager.Instance.isFrozen()){
            calculateValues();
            calculatePowerups();
            totalPowerupEffect();
            updateTrailTimer();
            Move();
        }
    }

    // gets initial values from settings
    private void setInitialValues()
    {
        newHoleDelay();
        velocityMagnitude = Settings.Instance.initialSpeed;
        turnSharpness = Settings.Instance.initialTurnSharpness;
        holeDuration = Settings.Instance.initialHoleDuration;
        minHoleDelay = Settings.Instance.initialMinHoleDelay;
        maxHoleDelay = Settings.Instance.initialMaxHoleDelay;
        initializeDictionary();
        lastBodyPosition = bodyPosition = body.transform.position;
    }

    private void initializeDictionary()
    {
        activePowerups = new Dictionary<string, List<float>>();  
        foreach (string key in Settings.Instance.playerPowerups)
        {
            activePowerups.Add(key, new List<float>());
        }
    }

    private void calculatePowerups()
    {
        foreach(string powerup in Settings.Instance.playerPowerups)
        {
            List<float> timers = activePowerups[powerup];

            // update timers
            for(int i=0; i < timers.Count; i++){
                timers[i] -= Time.fixedDeltaTime;
                if(timers[i] <= 0){
                    timers.RemoveAt(i);
                    i--; // objects in front move back
                }
            }
            Powerup.applyPowerup(this,powerup,timers.Count);
        }

    }

    private float velMultCalculator(int totalVelCount)
    {
        if (totalVelCount == 0)
        {
            return 1f;
        }
        else
        {
            return (totalVelCount + 0.5f);
        }
    }


    private void totalPowerupEffect()
    {
        // Adding an if statement for the situation where a field needs to remain unchanged to avoid miscalculations
        int totalVelCount = speedCount;
        int totalThickCount = fatCount;
        float totalVelMult = velMultCalculator(totalVelCount);

        if (totalVelCount == 0)
        {
            velocityMagnitude = Settings.Instance.initialSpeed;
        }
        else
        {
            velocityMagnitude = (totalVelMult) * Settings.Instance.initialSpeed;
        }

        if (fatCount == 0 && speedCount == 0)
        {
            holeDuration = Settings.Instance.initialHoleDuration;
        }
        else
        {
            //remember add helper function
            holeDuration = Settings.Instance.initialHoleDuration * (float)(System.Math.Pow(Settings.Instance.holeFatMultiplier, fatCount) / totalVelMult);
        }


        if(totalVelCount == 0)
        {
            turnSharpness = Settings.Instance.initialTurnSharpness;
        }
        else
        {
            turnSharpness = (float) (Settings.Instance.initialTurnSharpness * ((totalVelMult - 1) * 0.5f + 1));
        }

    }

    // applies speed effect for current frame count times
    public void speedEffect(int count)
    {
        speedCount = count;  
    }

    // TODO: change the fattening same way
    public void fatEffect(int count)
    {
        fatCount = count;
        if (count == 0)
        {
            transform.Find("Body").transform.localScale = new Vector3(Settings.Instance.initialSize, Settings.Instance.initialSize, 0);
        }
        else
        {
            float effFatMultiplier = (float)(System.Math.Pow(Settings.Instance.fatMultiplier, count ));
            float scale = Settings.Instance.initialSize * effFatMultiplier;
            transform.Find("Body").transform.localScale = new Vector3(scale, scale, 0);
        }
    }


    // applies reverse effect for current frame count times
    public void reverseEffect(int count)
    {
        if (count > 0)
        {
            // reverse direction
            reversedDirection = true;
            // set body color to blue
            body.GetComponent<SpriteRenderer>().color = new Color(0,0,0.9f);
        }
        else
        {
            reversedDirection = false;
            body.GetComponent<SpriteRenderer>().color = new Color(0.9f,1f,0);
        }
    }

    public void invincibleEffect(int count)
    {
        if (count == 0)
        {
            invincible = false;
        }
        else
        {
            invincible = true;
        }
    }

    public void squareEffect(int count)
    {
        if (count == 0)
        {
            isSquare = false;
            body.GetComponent<CircleCollider2D>().enabled = true;
            body.GetComponent<BoxCollider2D>().enabled = false;
            body.GetComponent<SpriteRenderer>().sprite = circleSprite;
        }
        else
        {
            isSquare = true;
            body.GetComponent<CircleCollider2D>().enabled = false;
            body.GetComponent<BoxCollider2D>().enabled = true;
            body.GetComponent<SpriteRenderer>().sprite = squareSprite;
        }
    }

    // on press right
    public void OnRight(InputAction.CallbackContext value)
    {
        rightPressed = value.ReadValueAsButton();
    }

    // on press left
    public void OnLeft(InputAction.CallbackContext value)
    {
        leftPressed = value.ReadValueAsButton();
    }

    private void getInput()
    {
        turnDirection = (leftPressed ? 1 : 0) - (rightPressed ? 1 : 0);
        if(reversedDirection) turnDirection *= -1;
    }

    //TODO: rename?
    private void calculateValues()
    {
        // calculate angle
        if (!isSquare)
        {
            angle += turnDirection * turnSharpness * Time.deltaTime;
        }
        else
        {
            if (rightSquare && playerName == "Greenlee")
            {
                // calculate angle
                angle -= 90 * Mathf.Deg2Rad;
            }
            rightSquare = false;

            if (leftSquare && playerName == "Greenlee")
            {
                // calculate angle
                angle += 90 * Mathf.Deg2Rad;
            }
            leftSquare = false;
        }
        angle = Utilities.clampAngle(angle);

        // calculate vectors
        velocityVector = Utilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle);
        direction = velocityVector.normalized;

        // update body position values
        lastBodyPosition = bodyPosition;
        bodyPosition = body.transform.position;

    }

    // Moves player
    private void Move()
    {
        body.transform.position += new Vector3(velocityVector.x,velocityVector.y,0); // move body

        Utilities.rotateObject(body,angle); // rotate body to looking angle

        spawnTrail(); // create trail
    }

    // Kills the player (add necessary stuff later)
    public void kill()
    {
        alive = false;
        GameManager.Instance.giveAllAlivePoints();
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

    // spawnTrail instatiates trailPiece in the location:
    // 1 quarter of the radius of player to the opposite direction from the movement and
    // matches the size according to the radius of player
    public void spawnTrail()
    {
        if (isSpawningTrail() && alive)
        {
            GameObject trailPiece = Instantiate(trailPiecePrefab, trail.transform) as GameObject; // create trail piece

            trailPiece.GetComponent<Trail>().playerNum = playerNum;

            float radius = body.transform.localScale.x; // body radius

            // calculate distance of body from last frame
            float dis = Utilities.vectorDistance(new Vector2(bodyPosition.x,bodyPosition.y),
                new Vector2(lastBodyPosition.x,lastBodyPosition.y));

            dis *= 2.3f; // small gap clear

            Vector3 d3 = new Vector3(direction.x, direction.y, 0);

            trailPiece.transform.position = body.transform.position - 0.5f * dis * d3; // move piece

            trailPiece.transform.localScale = new Vector3(radius, dis, 0); // scale piece

            Utilities.rotateObject(trailPiece, angle); // rotate piece
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
        if ((holeTimer > nextHoleDelay && holeTimer < nextHoleDelay + holeDuration) || invincible)
        {
            return false;
        }

        else return true;
    }

    // Delete player trail
    public void deleteTrail()
    {
        Utilities.deleteAllChildren(trail);
    }

    // Rotates and moves player to a random area in the map range
    public void randomizeLocation()
    {
        // make random values for starting rotation and position
        angle = Random.Range(0, 2f * Mathf.PI);
        float x = Random.Range(-GameManager.Instance.xRange, GameManager.Instance.xRange);
        float y = Random.Range(-GameManager.Instance.yRange, GameManager.Instance.yRange);
        Vector3 location = new Vector3(x,y,0);
        // rotate and move
        Utilities.rotateObject(body,angle);
        body.transform.position = location;

        lastBodyPosition = bodyPosition = body.transform.position; // update these values
    }

    // resets all timers of the player
    public void resetTimers()
    {
        foreach (string powerup in Settings.Instance.playerPowerups)
        {
            if(activePowerups != null)
                activePowerups[powerup].Clear();
        }
        newHoleDelay();
    }

    public void setHoleDuration(float duration)
    {
        holeDuration = duration;
    }

    public float getVelocityMagnitude()
    {
        return velocityMagnitude;
    }

    public void setVelocityMagnitude(float velocity)
    {
        velocityMagnitude = velocity;
    }

    // adds count to the value of key in the dictionary
    public void addPowerupTimer(string key, float count)
    {
        activePowerups[key].Add(count);
    }

 
}
