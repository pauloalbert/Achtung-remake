using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.invincibleSettings;
    }
    
    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
