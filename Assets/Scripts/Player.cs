using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorUtilities;

public class Player : MonoBehaviour
{
    [SerializeField] private float turnSharpness = 0.05f;
    [SerializeField] private float velocityMagnitude = 0.05f;
    public Rigidbody2D rb;

    private float angle;
    private int input_direction;

    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(-180.0f, 180.0f);
        FixedUpdate();      //Not sure if better with or without TODO: check
    }

    // Update is called once per frame. TODO: change the input system
    void Update()
    {
        input_direction = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input_direction += 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input_direction -= 1;
        }
    }

    private void FixedUpdate()
    {
        angle += input_direction * turnSharpness;
        rb.MovePosition(rb.position + VectorUtilities.CreatePolar(velocityMagnitude, angle));
    }


}
