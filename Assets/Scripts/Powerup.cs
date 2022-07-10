using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    GREEN,RED,BLUE
};

public abstract class Powerup : MonoBehaviour
{

    protected string powerupName = "~";

    [SerializeField] protected PowerupType powerupType = PowerupType.GREEN;

    protected float duration = 5f;

    // all possible types the powerup can have
    public List<PowerupType> availableTypes;

    // gets PlayerController of the player that activated the powerup, does powerup
    public abstract void activate(PlayerController playerController = null);

    // apply powerup on player (according to powerup type)
    protected void giveEffects(PlayerController playerController)
    {
        switch (powerupType)
        {
            case PowerupType.GREEN:
            {
                playerController.addPowerupTimer(powerupName, duration);
            }
            break;
            case PowerupType.RED:
            {
                foreach (PlayerController player in GameManager.Instance.getActivePlayers())
                {
                    if (player != playerController)
                    {
                        player.addPowerupTimer(powerupName, duration);
                    }
                }
            }
            break;
            case PowerupType.BLUE:
            {
                foreach (PlayerController player in GameManager.Instance.getActivePlayers())
                {
                    player.addPowerupTimer(powerupName, duration);
                }
            }
            break;
        }
    }


    // gets PlayerController of player, powerup name and amount of the powerup the player has,
    // applies the poewrup effect on the player
    public static void applyPowerup(PlayerController player, string powerName, int amount = 0)
    {
        switch(powerName)
        {
            case "speed":
                player.speedEffect(amount);
                break;
            case "reverse":
                player.reverseEffect(amount);
                break;
            case "fat": 
                player.fatEffect(amount);
                break;
            case "invincible":
                player.invincibleEffect(amount);
                break;
            case "square":
                player.squareEffect(amount);
                break;
                // Add here cases for effect powerups
        }
    }

    public void setPowerupType(PowerupType type)
    {
        powerupType = type;
    }

    public static Color typeToColor(PowerupType type)
    {
        switch(type)
        {
            case PowerupType.GREEN: return new Color(0,0.9f,0);
            case PowerupType.RED: return new Color(0.9f,0,0);
        }
        return new Color(0,0,0.9f);
    }
}

