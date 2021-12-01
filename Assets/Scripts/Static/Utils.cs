using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{

    public const float StartTime = 4;

    public static T[] EnumValues<T>()
    {
        return System.Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }

    private static readonly int[] ids = new int[256];

    static Utils()
    {
        var all = EnumValues<ShaderName>();
        for (var i = 0; i < all.Length; i++)
            ids[i] = Shader.PropertyToID(all[i].ToString());
    }

    public static int ShaderID(ShaderName name) => ids[(byte) name];

    public static T Choice<T>(T[] ts)
    {
        if (ts == null)
            return default;
        else
            return ts.Length == 0 ? default : ts[Random.Range(0, ts.Length)];
    }

    public static T Choice<T>(System.Collections.Generic.List<T> ts)
    {
        if (ts == null)
            return default;
        else
            return ts.Count == 0 ? default : ts[Random.Range(0, ts.Count)];
    }

    public static void RotateTowards(this Transform self, Vector3 worldPoint, float degrees, bool saveZ)
    {
        if (saveZ)
        {
            var z = self.eulerAngles.z;
            var rotation = self.rotation;
            rotation.SetLookRotation(worldPoint - self.position);
            self.rotation = Quaternion.RotateTowards(self.rotation, rotation, degrees);
            var e = self.eulerAngles;
            e.z = z;
            self.eulerAngles = e;
        }
        else
        {
            var rotation = self.rotation;
            rotation.SetLookRotation(worldPoint - self.position);
            self.rotation = Quaternion.RotateTowards(self.rotation, rotation, degrees);
        }
    }

    /// <param name="power">Power of engine, 0-10</param>
    public static float EnergyConsumption(float power)
    {
        return Mathf.Sqrt(power) * power;
    }

    public static float ToSadUnits(Rigidbody rb) => ToSadUnits(rb.velocity.magnitude);
    public static float ToSadUnits(Vector3 vec) => ToSadUnits(vec.magnitude);
    public static float ToSadUnits(float value) => value * 24;

    public static Vector3 ShootTo(Rigidbody me, Rigidbody target, float aBulletSpeed, byte quality) =>
        ShootTo(me.position, me.velocity, target.position, target.velocity, aBulletSpeed, quality);

    public static Vector3 ShootTo(Vector3 me, Vector3 mySpeed, Rigidbody target, float aBulletSpeed, byte quality) =>
        ShootTo(me, mySpeed, target.position, target.velocity, aBulletSpeed, quality);

    public static Vector3 ShootTo(Vector3 me, Vector3 mySpeed, Vector3 target, Vector3 targetSpeed, float aBulletSpeed,
        byte quality)
    {
        if (aBulletSpeed == 0)
            return target;
        
        var to = target;

        for (var i = 0; i < quality; i++)
            to = aBulletSpeed * Vector3.Distance(to, me) * (targetSpeed - mySpeed) + target;
        return to;
    }
}