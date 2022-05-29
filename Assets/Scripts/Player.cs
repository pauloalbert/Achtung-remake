using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static VectorUtilities;

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

    [Range(0.1f, 8f)] public float turnSharpness = 3f;
    
    [Range(0.1f, 8f)] public float velocityMagnitude = 2f;

    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(0, 2f*3.141592653589793238462643383279502f);
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void FixedUpdate()
    {
        //deltaTime is used if fixedupdate doesn't run at the right speed, would make it feel like slowed time.
        //Both with and without are good, what matters is that **both angle and translate depend on deltatime** if any.
        angle += turnDirection * turnSharpness * Time.deltaTime;  
        this.transform.Translate(VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle));
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
        turnDirection = (rightPressed ? 1:0) - (leftPressed ? 1:0);
    }


}
