using UnityEngine;

public class AtmoSphere : COLLECTOR<Ship>
{
    public float Warn;
    public float Death;
    public float CameraShake;

    public override void ForEach(Ship q)
    {
        var d = Vector3.Distance(transform.position, q.transform.position);
        if (d < Warn)
        {
            if (d <= Death)
                q.DeathDamage();
            else
                q.Warn(q.transform.position * 2 - transform.position, new Ship.Warning(false, CameraShake * (d - Warn) / (Death - Warn)));
        }
    }
}