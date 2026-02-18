using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrosshairFunctions", menuName = "Scriptable Objects/CrosshairFunctions")]
public class CrosshairFunctions : ScriptableObject
{
    private static Vector2 midPos = new Vector2(0,0);

    public static Vector2 GetRandomEdgePoint(RectTransform canvasRect)
    {
        float halfW = canvasRect.rect.width / 2f;
        float halfH = canvasRect.rect.height / 2f;

        int side = UnityEngine.Random.Range(0, 4);

        switch (side)
        {
            case 0:
                return new Vector2(-halfW, UnityEngine.Random.Range(-halfH, halfH));

            case 1:
                return new Vector2(halfW, UnityEngine.Random.Range(-halfH, halfH));

            case 2:
                return new Vector2(UnityEngine.Random.Range(-halfW, halfW), halfH);

            default:
                return new Vector2(UnityEngine.Random.Range(-halfW, halfW), -halfH);
        }
    }

    public static Func<float, float, float> getRandom()
    {
        List<Func<float, float, float>> funcs = new List<Func<float, float, float>> 
        { 
            Linear, 
            Sin 
        };

        return funcs[UnityEngine.Random.Range(0, funcs.Count)];
    }

    public static float Linear(float a, float x)
    {
        return 0;
    }

    public static float Sin(float a, float x)
    {
        return (float) Math.Sin(a * 2 * x * Math.PI);
    }

}
