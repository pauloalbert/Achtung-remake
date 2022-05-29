using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{   
    [Header("Input")]

    public bool right_input;
    public bool left_input;

    [Space(10)]

    [Header("Player Values")]

    [Tooltip("Value for move direction: -1, 0, 1 for left forward and right respectivley.")]
    public int move;
    [Tooltip("true if player is alive, false if dead.")]
    public bool alive;

    [Range(0.1f, 3f)] public float turnSharpness = 1f;
    
    [Range(0.1f, 3f)] public float velocityMagnitude = 1f;

    private Rigidbody2D rigidbod;

    // Start is called before the first frame update
    void Start()
    {
        rigidbod = GetComponent<Rigidbody2D>();
        rigidbod.velocity = new Vector2(Mathf.Sqrt(velocityMagnitude), Mathf.Sqrt(velocityMagnitude));
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void FixedUpdate()
    {
        rigidbod.AddForce(turnSharpness * forceDirection(move),ForceMode2D.Force);
    }

    // returns perpendicular normazlized vector to current velocity direction.
    // 90 degrees clockwise if r is 1 and counter clockwise if r is -1.
    // precondition: r must be 1 or -1 (NOT ASSERTING PRECONDITION FOR NOW)
    private Vector2 forceDirection(int r)
    {
        float xAxisVelocity = (float) GetComponent<Rigidbody2D>().velocity.x / velocityMagnitude;
        float yAxisVelocity = (float) GetComponent<Rigidbody2D>().velocity.y / velocityMagnitude;
        return new Vector2(r * yAxisVelocity, -r * xAxisVelocity);
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        right_input = value.ReadValueAsButton();
    }
    public void OnLeft(InputAction.CallbackContext value)
    {
        left_input = value.ReadValueAsButton();
    }
    private void getInput()
    {
        move = (right_input ? 1:0) - (left_input ? 1:0);
    }
}
