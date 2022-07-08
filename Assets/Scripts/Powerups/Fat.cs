using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fat : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.fatSettings;
    }

    public override void activate(PlayerController playerController)
    {
        giveEffects(playerController);
    }
}
