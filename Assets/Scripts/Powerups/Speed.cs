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

        // get settings and game manager
        settings = GameObject.Find("Settings").GetComponent<Settings>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {
        switch(powerupType)
        {
            case PowerupType.GREEN:
            {
                playerController.addPowerupTimer(powerupName, duration);
            }
            break;
            case PowerupType.RED:
            {
                foreach(PlayerController player in gameManager.getActivePlayers())
                {
                    if(player != playerController)
                    {
                        player.addPowerupTimer(powerupName, duration);
                    }
                }
            }
            break;
        }
    }

}
