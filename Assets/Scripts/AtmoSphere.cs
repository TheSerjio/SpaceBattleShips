using UnityEngine;

public class AtmoSphere : COLLECTOR<Ship>
{
    public float Warn;
    public float Death;

    public override void ForEach(Ship q)
    {
        var d = Vector3.Distance(transform.position, q.transform.position);
        if (d < Warn)
        {
            q.Warn(q.transform.position - transform.position, DistanceWarn.CameraShake);
            if (d < Death)
                q.OnDamaged(1, null);
        }
    }
}