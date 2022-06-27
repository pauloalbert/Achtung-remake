using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : Powerup
{

    /*
      Powerup tutorial:

      first create a prefab from the template prefab and add script to it.
      then add the powerup name to usedPowerups list in settings, and if the player
      behaviour is affected by the powerup add it to playerPowerups list aswell.
      lastly add the prefab to PowerupPrefabs dictionary in settings with the key
      being the powerup name.

      in the powerup script added to the prefab, in Awake() set the powerup name and add to the availableTypes list the types
      that the powerup can get spawned with. ------------ can also be set in prefab
      editor but git resets it for now :|

      in Start() set the object color to be the game type color.

      in Activate() (called once when powerup is taken) do what the powerup does. This might mean adding a function in playerConroller script.

  */


    void Awake()
    {
        powerupName = "Template";

        // add available types for powerup
        availableTypes.Add(PowerupType.GREEN);
        availableTypes.Add(PowerupType.RED);
        availableTypes.Add(PowerupType.BLUE);
    }

    void Start()
    {
        // set sprite color to powerup type
        gameObject.GetComponent<SpriteRenderer>().color = typeToColor(powerupType);
    }
    
    public override void activate(PlayerController playerController)
    {

    }
}
