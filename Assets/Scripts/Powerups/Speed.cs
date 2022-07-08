using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.speedSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
    
}
