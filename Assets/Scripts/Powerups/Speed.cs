using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : Powerup
{

    void Awake()
    {
        powerupName = "speed";

        availableTypes.Add(PowerupType.GREEN);
        availableTypes.Add(PowerupType.RED);
    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
    
}
