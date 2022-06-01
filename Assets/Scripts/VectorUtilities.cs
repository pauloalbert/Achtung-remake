using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class VectorUtilities
{
    // Gets magnitude and angle, returns Vector2 pointing to angle with radius magnitude
    public static Vector2 CreatePolar(float radius, float angle){
        return new Vector2(
            radius * Mathf.Cos(angle + Mathf.PI/2),
            radius * Mathf.Sin(angle + Mathf.PI/2)
            
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

    // Gets GameObject and angle in radians, rotates object to given angle
    public static void rotateObject(GameObject obj, float deg)
    {
        obj.transform.rotation = Quaternion.Euler(0,0,deg*Mathf.Rad2Deg);
    }
}