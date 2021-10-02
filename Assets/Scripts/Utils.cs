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

    public static void RotateTowards(this Transform self, Vector3 worldPoint, float degrees)
    {
        var z = self.eulerAngles.z;
        var rotation = self.rotation;
        rotation.SetLookRotation(worldPoint - self.position);
        self.rotation = Quaternion.RotateTowards(self.rotation, rotation, degrees);
        var e = self.eulerAngles;
        e.z = z;
        self.eulerAngles = e;
    }

    /// <param name="power">Power of engine, 0-10</param>
    public static float EnergyConsumption(float power)
    {
        return Mathf.Pow(power, 1.5f);
    }

    public static float ToSadUnits(Rigidbody rb) => ToSadUnits(rb.velocity.magnitude);
    public static float ToSadUnits(Vector3 vec) => ToSadUnits(vec.magnitude);
    public static float ToSadUnits(float value) => value / 24;
}