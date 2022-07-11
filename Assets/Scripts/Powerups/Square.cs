using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Powerup
{


    void Awake()
    {
        powerupSettings = Settings.Instance.squareSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
