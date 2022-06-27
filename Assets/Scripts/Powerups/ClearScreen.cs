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
    }

    public override void activate(PlayerController playerController)
    {
        GameManager.Instance.deleteAllTrails();
    }
}
