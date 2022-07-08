using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverse : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.reverseSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
