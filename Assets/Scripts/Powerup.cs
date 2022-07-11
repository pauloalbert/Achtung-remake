using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    GREEN,RED,BLUE
}

[System.Serializable]
public class PowerupSettings
{
    [SerializeField] private string _name; // powerup name
    [SerializeField] private float _duration; // duration of powerup
    [SerializeField] private float _frequency; // spawn frequency
    [SerializeField] private bool _hasTimer; // true if timer is enabled for this powerup
    [SerializeField] private List<PowerupType> _availableTypes; // list of available types for powerup

    public string Name{ get => _name; set => _name = value; }
    public float Duration{ get => _duration; set => _duration = value; }
    public float Frequency{ get => _frequency; set => _frequency = value; }
    public bool HasTimer{ get => _hasTimer; set => _hasTimer = value; }
    public List<PowerupType> AvailableTypes{ get => _availableTypes; set => _availableTypes = value; }

    // constructor
    public PowerupSettings(string name, float duration, float frequency, bool hasTimer, List<PowerupType> availableTypes)
    => (_name,_duration,_frequency,_hasTimer,_availableTypes) = (name,duration,frequency,hasTimer,availableTypes);
}

public abstract class Powerup : MonoBehaviour
{
    public PowerupType powerupType; // current powerup type

    [SerializeField] public PowerupSettings powerupSettings;

    void Start()
    {
        // set powerup color
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }

    // gets PlayerController of the player that activated the powerup, does powerup
    public abstract void activate(PlayerController playerController = null);

    // apply powerup on player (according to powerup type)
    protected void giveEffects(PlayerController playerController)
    {
        switch (powerupType)
        {
            case PowerupType.GREEN:
            {
                playerController.powerupHandler.giveEffect(powerupSettings.Name, powerupSettings.Duration);
                if(powerupSettings.HasTimer)
                {
                    playerController.timerHandler.addTimer(powerupSettings.Duration);
                }
            }
            break;
            case PowerupType.RED:
            {
                foreach (PlayerController player in GameManager.Instance.getActivePlayers())
                {
                    if (player != playerController && player.isAlive())
                    {
                        player.powerupHandler.giveEffect(powerupSettings.Name, powerupSettings.Duration);
                        if(powerupSettings.HasTimer)
                        {
                            player.timerHandler.addTimer(powerupSettings.Duration);
                        }
                    }
                }
            }
            break;
            case PowerupType.BLUE:
            {
                foreach (PlayerController player in GameManager.Instance.getActivePlayers())
                {
                    if(player.isAlive())
                    {
                        player.powerupHandler.giveEffect(powerupSettings.Name, powerupSettings.Duration);
                        if(powerupSettings.HasTimer)
                        {
                            player.timerHandler.addTimer(powerupSettings.Duration);
                        }
                    }
                }
            }
            break;
        }
    }


    // gets PlayerController of player, powerup name and amount of the powerup the player has,
    // applies the poewrup effect on the player
    public static void applyPowerup(PowerupHandler powerupHandler, string powerName, int amount = 0)
    {
        switch(powerName)
        {
            case "speed": case "slow":
                powerupHandler.velocityEffect();
                break;
            case "reverse":
                powerupHandler.reverseEffect(amount);
                break;
            case "fat": case "thin":
                powerupHandler.sizeEffect();
                break;
            case "invincible":
                powerupHandler.invincibleEffect(amount);
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
        return new Color(0,0,0.9f); // blue
    }
}

