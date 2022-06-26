using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Powerup
{

    void Awake()
    {
        powerupName = "invincible";

        // add available types for powerup
        availableTypes.Add(PowerupType.GREEN);

        // Get settings and game manager
        settings = GameObject.Find("Settings").GetComponent<Settings>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        // set sprite color to powerup type
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {
        playerController.addPowerupTimer(powerupName, duration);
    }
}
