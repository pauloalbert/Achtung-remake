using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverse : Powerup
{

    void Awake()
    {
        powerupName = "reverse";

        // add available types for powerup
        availableTypes.Add(PowerupType.RED);
    }

    void Start()
    {
        // set sprite color to powerup type
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {
        // red type
        foreach(PlayerController player in gameManager.getActivePlayers())
        {
            if(player != playerController)
            {
                player.addPowerupTimer(powerupName, duration);
            }
        }
    }
}
