using System;
using UnityEngine;

public class MathFUtils : MonoBehaviour
{

    public const float PI = 3.14159265359f;

    public const float Degree2Rad = MathF.PI / 180f;

    public const float Rad2Deg = 57.29578f;

    public const float epsilon = 0.01f;

    public static float ClampValue(float value, float min, float max)
    {
        float returnValue = value;

        if(returnValue < min) returnValue = min;
        else if (returnValue > max) returnValue = max;

        return returnValue;
    }
}
