using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreen : Powerup
{

    void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType); 
        powerupName = "clearScreen";

        availableTypes.Add(PowerupType.BLUE);

        // get game manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void activate(PlayerController playerController)
    {
        gameManager.deleteAllTrails();
    }
}
