using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : Powerup
{

    /*
      Powerup tutorial:

      First create a prefab from the template prefab and add script to it.
      then add the powerup name to usedPowerups list in settings, and if the player
      behaviour is affected by the powerup add it to playerPowerups list aswell.
      lastly add the prefab to PowerupPrefabs dictionary in settings with the key
      being the powerup name.

      Create a powerupSettings in Settings class and give settings in Awake().

      In activate() (called once when powerup is taken) do what the powerup does.
      if the powerup is a player effect, call give effects for the powerup name.
      for the effect to have an effect on the player, player values should be changed
      in PowerupHandler and if new functionality is needed should be added to
      PlayerController.

  */


    void Awake()
    {
        // powerupSettings = Settings.Instance.--name--Settings;
    }
    
    public override void activate(PlayerController playerController)
    {

    }
}
