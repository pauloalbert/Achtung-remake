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

    // color of the powerup
    [SerializeField] protected PowerupType powerupType = PowerupType.GREEN;

    [SerializeField] protected float duration = 5f;

    protected GameManager gameManager;
    protected Settings settings;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // gets PlayerController of the player that activated the powerup, does powerup
    public abstract void activate(PlayerController playerController = null);

    public static void applyPowerup(PlayerController player, string powerName, int amount = 0)
    {
        switch(powerName)
        {
            case "speed": player.speedEffect(amount);
            break;
        }
    }
}

