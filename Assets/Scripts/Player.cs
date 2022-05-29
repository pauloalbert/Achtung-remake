using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorUtilities;

public class Player : MonoBehaviour
{
    [SerializeField] private float turnSharpness = 3f;
    [SerializeField] private float velocityMagnitude = 2f;

    
    private float angle;
    private int input_direction;

    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(-180.0f, 180.0f);
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
        //deltaTime is used if fixedupdate doesn't run at the right speed, would make it feel like slowed time.
        //Both with and without are good, what matters is that **both angle and translate depend on deltatime** if any.
        angle += input_direction * turnSharpness * Time.deltaTime;  
        this.transform.Translate(VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle));
    }


}
