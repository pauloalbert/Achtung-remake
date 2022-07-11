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

    // ID
    public string playerName;
    public int playerNum;

    // Value for move direction: 1, 0, -1 for left forward and right respectivley
    private int _turnDirection = 0;
    // true if turnDirection should be reversed
    private bool _reversedDirection = false;

    // true if player is alive, false if dead
    private bool _alive = true;
    // true if player is invincible
    private bool _invincible = false;

    // Movement values - affect next move calculations ---------------------------
    
    // turn sharpness
    private float _turnSharpness;
    // movement speed
    private float _speed;

    // length of player holes
    [SerializeField] private float _holeLength;
    // current hole length
    private float _currentHoleLength;

    // Tuple with range of time in seconds that is selected for nextHoleDelay
    private (float,float) _holeDelayRange;
    // After this time passes from last hole a new hole will be made
    private float _nextHoleDelay;

    // angle that player is looking at
    private float _angle;

    // Vector2 of location to move player next frame
    private Vector2 _velocityVector;
    // Normalized velocityVector
    private Vector2 _direction;

    // Public members
    public int TurnDirection
    {
        get => _turnDirection;
    }
    public bool ReversedDirection
    {
        get => _reversedDirection;
        set => _reversedDirection = value;
    }
    public bool Alive
    {
        get => _alive;
        set => _alive = value;
    }
    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }
    public float TurnSharpness
    {
        get => _turnSharpness;
        set => _turnSharpness = value;
    }
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public float HoleLength
    {
        get => _holeLength;
        set => _holeLength = value;
    }



    [Space(10)]

    [Header("Objects")]

    
    public Sprite circleSprite;
    public Sprite squareSprite;
    [Tooltip("Trail piece prefab.")]
    public GameObject trailPiecePrefab;
    [Tooltip("Player's trail color.")]
    public Color color = Color.white;
    [Tooltip("Player's body object.")]
    public GameObject _body;
    [Tooltip("Player's trail object.")]
    public GameObject _trail;
    [Tooltip("Player's helper collider when it is a square object.")]
    public GameObject squareHelp;
    

    // Public members
    public GameObject Body
    {
        get => _body;
    }
    
    [Header("Square Fields")]
    private bool isSquare = false;
    private bool rightSquare = false;
    private bool leftSquare = false;
    [Tooltip("True in the physics frame a fat powerup was picked up")]
    public bool fatFrame = false;



    [Space(10)]

    [Header("Sub Behaviours")]
    public TimerHandler timerHandler;
    public PowerupHandler powerupHandler;

   

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

        if(_alive && !GameManager.Instance.isFrozen())
        {
            powerupHandler.updateValuesFromEffects();
            calculateMovementValues();
            Move();
        }
    }



    // sets initial values for player and sets player default values
    private void setInitialValues()
    {
        setDefaultValues();
    }

    // sets player default values
    public void setDefaultValues()
    {
        powerupHandler.clearEffects();
        _body.GetComponent<SpriteRenderer>().color = Color.yellow;                                                 // body color
        _body.transform.localScale = new Vector3(Settings.Instance.initialSize, Settings.Instance.initialSize, 0); // size
        _speed = Settings.Instance.initialSpeed;                                                                   // speed
        _turnSharpness = Settings.Instance.initialTurnSharpness;                                                   // turn sharpness
        _holeLength = Settings.Instance.initialHoleLength;                                                         // hole length
        _holeDelayRange = (Settings.Instance.initialMinHoleDelay, Settings.Instance.initialMaxHoleDelay);          // hole delay range
        resetTimers();
    }

    public void squareEffect(int count)
    {
        if (count == 0)
        {
            isSquare = false;
            _body.GetComponent<CircleCollider2D>().enabled = true;
            _body.GetComponent<BoxCollider2D>().enabled = false;
            _body.GetComponent<SpriteRenderer>().sprite = circleSprite;
        }
        else
        {
            isSquare = true;
            _body.GetComponent<CircleCollider2D>().enabled = false;
            _body.GetComponent<BoxCollider2D>().enabled = true;
            _body.GetComponent<SpriteRenderer>().sprite = squareSprite;
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
        _turnDirection = (leftPressed ? 1 : 0) - (rightPressed ? 1 : 0);
        if(_reversedDirection) _turnDirection *= -1;
    }

    // updates movement values according to current speed, intput etc.
    private void calculateMovementValues()
    {
        // calculate angle
        if (!isSquare)
        {
            _angle += _turnDirection * _turnSharpness * Time.deltaTime;
        }
        else
        {
            if (rightSquare && playerName == "Greenlee")
            {
                // calculate angle
                _angle -= 90 * Mathf.Deg2Rad;
            }
            rightSquare = false;

            if (leftSquare && playerName == "Greenlee")
            {
                // calculate angle
                _angle += 90 * Mathf.Deg2Rad;
            }
            leftSquare = false;
        }
        _angle = Utilities.clampAngle(_angle);

        // calculate vectors
        _velocityVector = Utilities.CreatePolar(_speed * Time.deltaTime, _angle);
        _direction = _velocityVector.normalized;
    }

    // Moves player
    private void Move()
    {
        _body.transform.position += new Vector3(_velocityVector.x,_velocityVector.y,0); // move body

        Utilities.pointObject(_body,_angle); // rotate body to looking angle

        spawnTrail(); // create trail
    }

    // Kills the player (add necessary stuff later)
    public void kill()
    {
        _alive = false;
        GameManager.Instance.giveAllAlivePoints();
    }

    // Bring player back to life
    public void revive()
    {
        _alive = true;
    }

    // getter for alive value
    public bool isAlive()
    {
        return _alive;
    }

    // spawnTrail instatiates trailPiece in the location:
    // 1 quarter of the radius of player to the opposite direction from the movement and
    // matches the size according to the radius of player
    public void spawnTrail()
    {   
        // calculate distance of body from last frame
            float dis = _velocityVector.magnitude;
        
        // update hole timer
        if (_nextHoleDelay <= 0)
        {
            if (_currentHoleLength >= _holeLength)
            {
                _currentHoleLength = 0;
                newHoleDelay();
            }
            else
            {
                _currentHoleLength += dis;
            }
        }
        else
        {
            _nextHoleDelay -= Time.fixedDeltaTime;
        }


        // Spawn trail piece if not creating hole
        if (isSpawningTrail())
        {
            GameObject trailPiece = Instantiate(trailPiecePrefab, _trail.transform) as GameObject; // create trail piece
            
            trailPiece.GetComponent<Trail>().playerNum = playerNum;

            float radius = _body.transform.localScale.x; // body radius

            dis *= 2.3f; // small gap clear

            Vector3 d3 = new Vector3(_direction.x, _direction.y, 0);

            trailPiece.transform.position = _body.transform.position - 0.5f * dis * d3; // move piece

            trailPiece.transform.localScale = new Vector3(radius, dis, 0); // scale piece

            Utilities.pointObject(trailPiece, _angle); // rotate piece
            trailPiece.GetComponent<SpriteRenderer>().color = color; // set color
        }
    }

    // randomizes the hole daly and changes the nextHoleDelay accordingly. Also resets the holeTime Counter
    public void newHoleDelay()
    {
        _nextHoleDelay = Random.Range(_holeDelayRange.Item1, _holeDelayRange.Item2);
    }

    // Returns true if currently need to spawn trail false otherwise
    public bool isSpawningTrail()
    {   
        // if should spawn trail
        if ((_currentHoleLength <= _holeLength && _nextHoleDelay > 0) && !_invincible && _alive)
        {
            return true;
        }

        else return false;
    }

    // Delete player trail
    public void deleteTrail()
    {
        Utilities.deleteAllChildren(_trail);
    }

    // Rotates and moves player to a random area in the map range
    public void randomizeLocation()
    {
        // make random values for starting rotation and position
        _angle = Random.Range(0, 2f * Mathf.PI);
        float x = Random.Range(-GameManager.Instance.xRange, GameManager.Instance.xRange);
        float y = Random.Range(-GameManager.Instance.yRange, GameManager.Instance.yRange);
        Vector3 position = new Vector3(x,y,0);
        // rotate and move
        Utilities.pointObject(_body,_angle);
        _body.transform.position = position;
    }

    // resets all timers of the player
    public void resetTimers()
    {
        timerHandler.deleteTimers();
        newHoleDelay();
    }

}
