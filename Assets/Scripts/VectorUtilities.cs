using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class VectorUtilities
{
    public static Vector2 CreatePolar(float r, float angle){
        return new Vector2(
            r * Mathf.Cos(angle),
            r * Mathf.Sin(angle)
        );
    }   //Unsure if this angle is upwards
}