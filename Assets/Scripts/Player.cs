using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool rightturn;

    [SerializeField] private float turnSharpness;

    [SerializeField] private float velocityMagnitude;

    // returns perpendicular normazlized vector to current velocity direction.
    // 90 degrees clockwise if r is 1 and counter clockwise if r is -1.
    // precondition: r must be 1 or -1 (NOT ASSERTING PRECONDITION FOR NOW)
    private Vector2 forceDirection(int r)
    {
        float xAxisVelocity = (float) GetComponent<Rigidbody2D>().velocity.x / velocityMagnitude;
        float yAxisVelocity = (float) GetComponent<Rigidbody2D>().velocity.y / velocityMagnitude;
        return new Vector2(r * yAxisVelocity, -r * xAxisVelocity);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sqrt(velocityMagnitude), Mathf.Sqrt(velocityMagnitude));
    }

    // Update is called once per frame. TODO: change the input system
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rightturn = true;
        }
    }

    private void FixedUpdate()
    {
        if (rightturn)
        {
            GetComponent<Rigidbody2D>().AddForce(turnSharpness * forceDirection(1),ForceMode2D.Force);
        }
        rightturn = false;
    }
}
