using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
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

    // Gets GameObject and angle in radians, points object to given angle
    public static void pointObject(GameObject obj, float deg)
    {
        obj.transform.rotation = Quaternion.Euler(0,0,deg*Mathf.Rad2Deg);
    }

    // Gets GameObject, deletes all children
    public static void deleteAllChildren(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static float vectorDistance(Vector2 v, Vector2 w)
    {
        return Mathf.Sqrt(Mathf.Pow(v.x-w.x,2)+Mathf.Pow(v.y-w.y,2));
    }
}