using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerupScript : MonoBehaviour
{
    [SerializeField] private string powerupName = "INSERT NAME OF POWERUP";
    [SerializeField] private int positivity = 0;
    [SerializeField] private float duration = 5;

    public string getPowerupName()
    {
        return powerupName;
    }

    public float getDuration()
    {
        return duration;
    }
}

