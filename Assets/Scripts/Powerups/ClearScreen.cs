using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreen : Powerup
{
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerupName = "clearScreen";
    }
    public override void activate(PlayerController playerController)
    {
        gameManager.deleteAllTrails();
    }
}
