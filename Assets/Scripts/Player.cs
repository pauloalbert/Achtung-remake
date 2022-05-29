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
        // TODO: delete caps on first letter of direction & velocityVector?
        angle += turnDirection * turnSharpness * Time.deltaTime;
        angle = clampAngle(angle);
        Vector2 velocityVector = VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle);
        Vector2 direction = velocityVector.normalized;
        this.transform.Translate(velocityVector);
        spawnTrail(direction, angle);
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

    // Gets float value of an angle in radians, returns clamped angle
    private float clampAngle(float angle)
    {
        if (angle > 2 * Mathf.PI)
        {
            return clampAngle(angle - 2f * Mathf.PI);
        }
        else if (angle < 0)
        {
            return clampAngle(angle + 2f * Mathf.PI);
        }
        return angle;
    }
    // TODO: worry about this showing up on screen?
    // spawnTrail takes in a direction d and angle a. instatiates trailPiece in the location
    // 1 quarter of the radius of player to the opposite direction from the movement and
    // matches the size according to the radius of player
    public void spawnTrail(Vector2 d, float a)
    {
        Vector3 d3 = new Vector3(d.x, d.y, 0);
        float radius = this.transform.localScale.x;
        GameObject trail = Instantiate(trailPiece) as GameObject;
        trail.transform.position = this.transform.position - 0.25f* radius * d3;
        trail.transform.localScale = new Vector3(radius , radius/2 , 0);
        trail.transform.Rotate(0, 0, -a * Mathf.Rad2Deg);
    }

}
