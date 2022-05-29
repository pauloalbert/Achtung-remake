using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class VectorUtilities
{

    public static Vector2 CreatePolar(float radius, float angle){
        return new Vector2(
            radius * Mathf.Sin(angle),
            radius * Mathf.Cos(angle)
            
        );
    }   //Unsure if this angle is upwards
}