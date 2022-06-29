using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{

    private PlayerController playerController;

    void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    // an idea for handling activation of the trail pieces. Probably problematic
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Trail":
                {
                    other.GetComponent<Trail>().exitedCircle();
                }
                break;
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (playerController.isAlive())
        {
            switch(other.tag)
            {
                case "Trail":
                    if (playerController.isSpawningTrail() && other.GetComponent<Trail>().playerNum != playerController.playerNum)
                    {
                        playerController.kill();
                    }
                    break;
                case "ActiveTrail":
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
                    other.GetComponent<Powerup>().activate(playerController); // activate powerup
                    
                    Destroy(other.gameObject); // destroy powerup
                }
                break;
                
            }
        }

        
    }

    

}
