using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squareHelpCollision : MonoBehaviour
{
    private PlayerController playerController;

    void Awake()
    {
        playerController = gameObject.GetComponentInParent<Collide>().getPlayerController();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerController.isAlive())
        {
            switch (other.tag)
            {
                case "Trail":
                    if (playerController.isSpawningTrail() && other.GetComponent<Trail>().playerNum == playerController.playerNum)
                    {
                        playerController.kill();
                    }
                    break;
            }
        }


    }
}
