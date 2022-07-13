using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public int playerNum;
    // amount of fat powerups the player is no longer safe from on collision
    private int fatRadiusAway;
    private PlayerController player;
    // array with position of all 4 corners
    private (float, float)[] cornerArray;
    // next distance all 4 corners need to be away from the player to increase fatRadiusAway
    private float nextThreshold;
    // time until the parent player can collide with this trail
    public float activateTime = 1.5f;
    // age in seconds of this trail piece
    private float age = 0f;

    private void FixedUpdate()
    {
        age += Time.deltaTime;
        if (age >= activateTime)
        {
            activateTrail();
        }
    }

    public void activateTrail()
    {
        gameObject.tag = "ActiveTrail";
        Object.Destroy(this);
    }


}
