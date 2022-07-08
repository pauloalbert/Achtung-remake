using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour
{
    // A dictionary that maps names of powerups to the amount of times they are active
    private Dictionary<string, List<float>> _activePowerups;

    private int _velocityCount = 0; // speed effect - slow effect
    private int _sizeCount = 0; // fat effect - thin effect

    private PlayerController playerController;


    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        initializePowerupDictionary();
    }

    // Powerups ========================================
    

    public void updateValuesFromEffects()
    {
        foreach(string powerup in Settings.Instance.playerPowerups)
        {
            List<float> timers = _activePowerups[powerup];

            // update timers ----------------- TODO: good way
            for(int i=0; i < timers.Count; i++)
            {
                timers[i] -= Time.fixedDeltaTime;

                if(timers[i] <= 0)
                {
                    timers.RemoveAt(i);
                    i--;
                }
            }

            Powerup.applyPowerup(this,powerup,timers.Count);
        }
        
        totalPowerupEffect();
    }

    // Give player "name" effect for "duration" seconds (assumes effect is available for game)
    public void giveEffect(string name, float duration)
    {
        addPowerupTimer(name, duration);
    }
    
    // Clears all player effects
    public void clearEffects()
    {
        foreach(string powerup in Settings.Instance.playerPowerups)
        {
            _activePowerups[powerup].Clear();
        }
    }

    // Adds count to the value of key in the dictionary
    public void addPowerupTimer(string key, float count)
    {
        _activePowerups[key].Add(count);
    }

    private void totalPowerupEffect()
    {
        // Adding an if statement for the situation where a field needs to remain unchanged to avoid miscalculations
        int totalVelCount = _velocityCount;
        int totalThickCount = _sizeCount;

        float totalVelMult = velMultCalculator(totalVelCount);

        if (totalVelCount == 0)
        {
            playerController.Speed = Settings.Instance.initialSpeed;
            playerController.TurnSharpness = Settings.Instance.initialTurnSharpness;
        }
        else
        {
            playerController.Speed = (totalVelMult) * Settings.Instance.initialSpeed;
            playerController.TurnSharpness = (float) (Settings.Instance.initialTurnSharpness * ((totalVelMult - 1) * 0.5f + 1));
        }

        if (_sizeCount == 0 && _velocityCount == 0)
        {
            playerController.HoleLength = Settings.Instance.initialHoleLength;
        }
        else
        {
            //remember add helper function
            playerController.HoleLength = Settings.Instance.initialHoleLength * (float)(System.Math.Pow(Settings.Instance.holeFatMultiplier, _sizeCount) / totalVelMult);
        }

    }

    // Applies speed effect for current frame count times
    public void velocityEffect()
    {
        _velocityCount = effectCount("speed") - effectCount("slow");

    }

    // TODO: change the fattening same way
    public void sizeEffect()
    {
        _sizeCount = effectCount("fat") - effectCount("thin");

        if (_sizeCount == 0)
        {
            // Defualt size
            playerController.Body.transform.localScale = new Vector3(Settings.Instance.initialSize, Settings.Instance.initialSize, 0);
        }
        else
        {
            float effFatMultiplier = (float)(System.Math.Pow(Settings.Instance.fatMultiplier, _sizeCount ));

            // Scale body
            float scale = Settings.Instance.initialSize * effFatMultiplier;
            playerController.Body.transform.localScale = new Vector3(scale, scale, 0);
        }
    }

    // applies reverse effect for current frame count times
    public void reverseEffect(int count)
    {
        if (count > 0)
        {
            // reverse direction
            playerController.ReversedDirection = true;
            // set body color to blue
            playerController.Body.GetComponent<SpriteRenderer>().color = new Color(0,0,0.9f);
        }
        else
        {
            // set default values
            playerController.ReversedDirection = false;
            playerController.Body.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public void invincibleEffect(int count)
    {
        if (count == 0)
        {
            playerController.Invincible = false;
        }
        else
        {
            playerController.Invincible = true;
        }
    }

    private void initializePowerupDictionary()
    {
        _activePowerups = new Dictionary<string, List<float>>(); 

        foreach (string key in Settings.Instance.playerPowerups)
        {
            _activePowerups.Add(key, new List<float>());
        }
    }


    private float velMultCalculator(int totalVelCount)
    {
        if (totalVelCount == 0)
        {
            return 1f;
        }
        else
        {
            return (totalVelCount + 0.5f);
        }
    }


    // Returns how many effects of effectName are in effect
    private int effectCount(string effectName)
    {
        if (!(_activePowerups.ContainsKey(effectName))) return 0;
        else return _activePowerups[effectName].Count;
    }
}