using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralMethods 
{
    public static float RoundTo2DecimalPlaces(float number) => Mathf.Round(number * 100f) / 100f;

    public static Vector2 Vector3ToVector2(Vector3 vector) => new Vector2(vector.x, vector.z);

    public static Vector3 Vector2ToVector3(Vector2 vector) => new Vector3(vector.x, 0f, vector.y);
    public static Vector3 SupressYComponent(Vector3 vector) => new Vector3(vector.x, 0f, vector.z);


}
