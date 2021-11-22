using UnityEngine;

public class DefaultShipAI : ShipAIController
{
    public float MinOptimalDistance;
    public float MaxOptimalDistance;
    float Dist;
    [Range(0, 0.99f)] public float AccuracyShooting;

    public override void OnStart()
    {
        Dist = Random.Range(MinOptimalDistance, MaxOptimalDistance);
    }

    public override void OnFixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > Dist)
        {
            if (Vector3.Dot(transform.forward, RB.velocity) < 0)
            {
                Ship.Brake(false);
            }
            else
            {
                Ship.EnginePower = 1;
                Ship.Forward();
            }
        }
        else
        {
            Ship.Brake(false);
        }
        Ship.Fire = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized) > AccuracyShooting;
        Ship.LookAt(Utils.ShootTo(RB, target.RB, Ship.mainWeapon.AntiSpeed));
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MinOptimalDistance);
        Gizmos.DrawWireSphere(transform.position, MaxOptimalDistance);
    }
}