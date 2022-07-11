using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.slowSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
