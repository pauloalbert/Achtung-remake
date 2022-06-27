using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fat : Powerup
{
    void Awake()
    {
        powerupName = "fat";

        availableTypes.Add(PowerupType.GREEN);
        availableTypes.Add(PowerupType.RED);

    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }

    public override void activate(PlayerController playerController)
    {
        Debug.Log("activate fat");
        switch (powerupType)
        {
            case PowerupType.GREEN:
                {
                    playerController.addPowerupTimer(powerupName, duration);
                }
                break;
            case PowerupType.RED:
                {
                    foreach (PlayerController player in gameManager.getActivePlayers())
                    {
                        if (player != playerController)
                        {
                            player.addPowerupTimer(powerupName, duration);
                        }
                    }
                }
                break;
        }
    }
}
