using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float Map(this ref float value, float min1, float max1, float min2, float max2)
        => value - min1 / (max1 - min1) * (max2 - min2) + min2;
}
