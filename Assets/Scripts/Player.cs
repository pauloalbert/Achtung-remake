using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input")]

    public bool rightPressed;
    public bool leftPressed;

    [Space(10)]

    [Header("Player Values")]

    [Tooltip("Value for move direction: -1, 0, 1 for left forward and right respectivley.")]
    public int turnDirection;
    [Tooltip("true if player is alive, false if dead.")]
    public bool alive;

    [Range(0.1f, 10f)] public float turnSharpness = 2f;

    [Range(0.1f, 30f)] public float velocityMagnitude = 10f;

    [Tooltip("The angle the player is pointing to in radians.")]
    public float angle;

    [SerializeField] private Vector2 velocityVector;
    [Tooltip("Normalized Vector2 pointing to player moving direction.")]
    [SerializeField] private Vector2 direction;

    [Space(10)]

    [Header("Objects")]

    [Tooltip("Trail piece prefab.")]
    public GameObject trailPiecePrefab;
    [Tooltip("Player's body object.")]
    public GameObject body;
    [Tooltip("Player's trail object.")]
    public GameObject trail;

    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(0, 2f * Mathf.PI);
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

        calculateValues();
        Move();
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
        turnDirection = (rightPressed ? 1 : 0) - (leftPressed ? 1 : 0);
    }

    private void calculateValues()
    {
        // calculate angle
        angle += turnDirection * turnSharpness * Time.deltaTime;
        angle = VectorUtilities.clampAngle(angle);

        // calculate vectors
        velocityVector = VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle);
        direction = velocityVector.normalized;
        }

    private void Move()
        {
        body.transform.Translate(velocityVector); // move body

        spawnTrail(direction, angle); // create trail
        }

    // TODO: worry about this showing up on screen?
    // spawnTrail takes in a direction d and angle a. instatiates trailPiece in the location
    // 1 quarter of the radius of player to the opposite direction from the movement and
    // matches the size according to the radius of player
    public void spawnTrail(Vector2 d, float a)
    {
        Vector3 d3 = new Vector3(d.x, d.y, 0);
        float radius = body.transform.localScale.x;
        GameObject trailPiece = Instantiate(trailPiecePrefab, trail.transform) as GameObject;
        trailPiece.transform.position = body.transform.position - 0.25f* radius * d3;
        trailPiece.transform.localScale = new Vector3(radius , radius/2 , 0);
        trailPiece.transform.Rotate(0, 0, -a * Mathf.Rad2Deg);
    }

}
