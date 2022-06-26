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

    [SerializeField] protected float duration = 5f;

    protected GameManager gameManager;
    protected Settings settings;

    // all possible types the powerup can have
    public List<PowerupType> availableTypes;

    // gets PlayerController of the player that activated the powerup, does powerup
    public abstract void activate(PlayerController playerController = null);

    // gets PlayerController of player, powerup name and amount of the powerup the player has,
    // applies the poewrup effect on the player
    public static void applyPowerup(PlayerController player, string powerName, int amount = 0)
    {
        switch(powerName)
        {
            case "speed": player.speedEffect(amount);
                break;
            case "fat": 
                player.fatEffect(amount);
                break;
            case "invincible":
                player.invincibleEffect(amount);
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
            case PowerupType.GREEN: return Color.green;
            case PowerupType.RED: return Color.red;
        }
        return Color.blue;
    }
}

