using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralMethods 
{
    public static Vector3 DirectionToVector3(Vector2 direction) => new Vector3(direction.x, 0f, direction.y);
    public static Vector3 SupressYComponent(Vector3 vector) => new Vector3(vector.x, 0f, vector.z);

}
