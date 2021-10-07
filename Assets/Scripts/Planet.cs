using UnityEngine;

public class Planet : COLLECTOR<Ship>
{
    public float WarnDistance;
    public float DeathDistance;

    public void FixedUpdate()
    {
        foreach (var q in All)
            if (q)
            {
                var d = Vector3.Distance(transform.position, q.transform.position);
                if (d > WarnDistance)
                {
                    q.Warn(transform.position);
                    if (d > DeathDistance)
                        q.OnDamaged(1, null);
                }
            }
            else
            {
                RemoveNull();
                break;
            }
    }
}