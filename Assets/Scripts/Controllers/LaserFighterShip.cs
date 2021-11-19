using UnityEngine;

public class LaserFighterShip : ShipAIController
{
    public float OptimalDistance;

    [Tooltip("more value = less shoot")][Range(-1, 1)] public float FireIf;

    public override void OnFixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > OptimalDistance)
        {
            Ship.EnginePower = 1;
            Ship.Forward();
        }
        else
        {
            Ship.Brake();
        }
        Ship.Fire = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized) > FireIf;
        Ship.LookAt(target.transform.position);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OptimalDistance);
    }
}