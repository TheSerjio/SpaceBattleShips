using UnityEngine;

public class DistanceLeash : COLLECTOR<Ship>
{
    public float WarnDistance;
    public float DeathDistance;

    public override void ForEach(Ship q)
    {
        var d = Vector3.Distance(transform.position, q.transform.position);
        if (d > WarnDistance)
        {
            q.Warn(transform.position, DistanceWarn.Text);
            if (d > DeathDistance)
                q.OnDamaged(1, null);
        }
    }
}