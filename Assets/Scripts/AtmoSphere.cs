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
                q.OnDamaged(1, null);
            else
                q.Warn(q.transform.position - transform.position, new Warning(false, CameraShake * (d - Warn) / (Death - Warn)));
        }
    }
}