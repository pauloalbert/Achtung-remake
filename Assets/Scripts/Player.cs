using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorUtilities;

public class Player : MonoBehaviour
{
    [SerializeField] private float turnSharpness = 3f;
    [SerializeField] private float velocityMagnitude = 2f;

    [SerializeField] private GameObject trailPiece;

    private float angle;
    private int input_direction;
    private bool hole = false;

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

        if (Input.GetKey(KeyCode.Space))
        {
            hole = true;
        }
        else hole = false;

    }

    private void FixedUpdate()
    {
        // deltaTime is used if fixedupdate doesn't run at the right speed, would make it feel like slowed time.
        // Both with and without are good, what matters is that **both angle and translate depend on deltatime** if any.
        // TODO: delete caps on first letter of direction & velocityVector?
        angle += input_direction * turnSharpness * Time.deltaTime;
        Vector2 velocityVector = VectorUtilities.CreatePolar(velocityMagnitude * Time.deltaTime, angle);
        Vector2 direction = velocityVector.normalized;
        this.transform.Translate(velocityVector);
        if (!hole) spawnTrail(direction, angle);
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
