using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thin : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.thinSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
