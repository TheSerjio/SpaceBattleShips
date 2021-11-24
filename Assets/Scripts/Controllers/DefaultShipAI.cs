using UnityEngine;

public class DefaultShipAI : ShipAIController
{
    public float MinOptimalDistance;
    public float MaxOptimalDistance;
    private float Dist;
    [Range(0, 0.99f)] public float AccuracyShooting;

    public override void OnStart()
    {
        Dist = Random.Range(MinOptimalDistance, MaxOptimalDistance);
    }

    public override void OnFixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, Target.transform.position);
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
        Ship.Target.Fire = Vector3.Dot(transform.forward, (Target.transform.position - transform.position).normalized) > AccuracyShooting;
        Ship.LookAt(Utils.ShootTo(RB, Target.RB, Ship.mainWeapon.AntiSpeed));
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MinOptimalDistance);
        Gizmos.DrawWireSphere(transform.position, MaxOptimalDistance);
    }
}