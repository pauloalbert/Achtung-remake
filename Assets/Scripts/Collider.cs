using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{

    private PlayerController playerController;

    void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (playerController.isAlive())
        {
            switch(other.tag)
            {
                case "Trail":
                {
                    if (playerController.isSpawningTrail())
                    {
                        playerController.kill();
                    }
                }
                break;
                case "Wall":
                {
                    if (playerController.isSpawningTrail())
                    {
                        playerController.kill();
                    }
                }
                break;
                case "Teleporter":
                {

                }
                break;
                case "Powerup":
                {
                        //TODO
                }
                break;
                
            }
        }
    }
}
