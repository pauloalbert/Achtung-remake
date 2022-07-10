using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Powerup
{


    void Awake()
    {
        powerupName = "square";
        // add available types for powerup
        availableTypes.Add(PowerupType.GREEN);
        availableTypes.Add(PowerupType.RED);
    }

    void Start()
    {
        // set sprite color to powerup type
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
