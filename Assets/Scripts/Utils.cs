using UnityEngine;

public static class Utils
{
    public static T Choice<T>(T[] ts)
    {
        return ts.Length == 0 ? default : ts[Random.Range(0, ts.Length)];
    }
    public static T Choice<T>(System.Collections.Generic.List<T> ts)
    {
        return ts.Count == 0 ? default : ts[Random.Range(0, ts.Count)];
    }

    public static void RotateTowards(this Transform self, Vector3 worldPoint, float degreesPerSec)
    {
        var rotation = self.rotation;
        rotation.SetLookRotation(worldPoint - self.position);
        var z = self.eulerAngles.z;
        self.rotation = Quaternion.RotateTowards(self.rotation, rotation, degreesPerSec);
        var e = self.eulerAngles;
        e.z = z;
        self.eulerAngles = e;
    }
}