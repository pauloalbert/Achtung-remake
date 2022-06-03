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
                        StartCoroutine(powerupHandler(other));
                }
                break;
                
            }
        }

        
    }

    // TODO: think about concurrency
    private IEnumerator powerupHandler(Collider2D other)
    {
        PowerupScript powerScript = other.GetComponent<PowerupScript>();
        string powerupName = powerScript.getPowerupName();
        other.GetComponent<CircleCollider2D>().enabled = false;
        other.GetComponent<SpriteRenderer>().enabled = false;

        // counts +1 in dictionary, waits time specified by the powerup script, subtracts 1.
        playerController.addToPowerupCount(powerupName, 1);
        Debug.Log("yes");
        yield return new WaitForSeconds(powerScript.getDuration());
        Debug.Log("done");
        playerController.addToPowerupCount(powerupName, -1);
        Destroy(other);
    }
}
