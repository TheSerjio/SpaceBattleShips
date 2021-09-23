using UnityEngine;

public static class Utils
{
    public static T Choice<T>(T[] ts) => ts[Random.Range(0, ts.Length)];
}