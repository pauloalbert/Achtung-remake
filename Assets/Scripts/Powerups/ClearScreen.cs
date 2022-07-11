using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreen : Powerup
{

    void Awake()
    {
        powerupSettings = Settings.Instance.clearScreenSettings;
    }

    public override void activate(PlayerController playerController)
    {
        GameManager.Instance.deleteAllTrails();
    }
}
