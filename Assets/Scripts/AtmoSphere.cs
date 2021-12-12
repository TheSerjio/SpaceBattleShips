using UnityEngine;

public class AtmoSphere : COLLECTOR<Ship>
{
    public float Warn;
    public float Death;
    public float CameraShake;

    protected override void ForEach(Ship q)
    {
        var d = Vector3.Distance(transform.position, q.transform.position);
        if (d < Warn)
        {
            if (d <= Death)
                q.DeathDamage();
            var power = (d - Warn) / (Death - Warn);
            q.Warn(q.transform.position * 2 - transform.position,
                new Ship.Warning(false, CameraShake * power));
            q.SomeDamage(power * power);
        }
    }
}