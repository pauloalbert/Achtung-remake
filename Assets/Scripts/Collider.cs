using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{

    public Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.isSpawningTrail())
        {
            player.alive = false;
        }
    }
}
