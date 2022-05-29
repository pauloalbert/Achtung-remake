using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class VectorUtilities
{
    // does something??
    public static Vector2 CreatePolar(float radius, float angle){
        return new Vector2(
            radius * Mathf.Sin(angle),
            radius * Mathf.Cos(angle)
            
        );
    }   //Unsure if this angle is upwards

    // Gets float value of an angle in radians, returns clamped angle
    public static float clampAngle(float angle)
    {
        angle = angle % (2*Mathf.PI);

        if (angle < 0)
        {
            return angle + (2f * Mathf.PI);
        }
        return angle;
    }
}