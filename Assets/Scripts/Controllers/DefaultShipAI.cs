using UnityEngine;

public class DefaultShipAI : ShipAIController
{
    public float MinOptimalDistance;
    public float MaxOptimalDistance;
    private float Dist;
    [Range(0, 0.99999f)] public float AccuracyShooting;
    private float LastTargetFound;
    [Range(0, 4)] public byte AimingQuality;
    
    public override void OnStart()
    {
        Dist = Random.Range(MinOptimalDistance, MaxOptimalDistance);
    }

    public override void OnFixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, Target.transform.position);
        if (dist > Dist + Target.size)
        {
            if (Vector3.Dot(transform.forward, RB.velocity) < 0)
            {
                Ship.Brake(false);
            }
            else
            {
                if (Time.time > LastTargetFound)
                {
                    if (Ship.mainWeapon.IsOutOfRange(Vector3.Distance(transform.position, Target.transform.position)))
                    {
                        var newTar = GameCore.Self.FindTargetShip(Ship.team, false, TargetFinding, transform);
                        if (newTar)
                            if (newTar != Target)
                            {
                                Target = newTar;
                                LastTargetFound = Time.time + 1;
                                return;
                            }
                    }
                }

                Ship.EnginePower = 1;
                Ship.Forward();
            }
        }
        else
        {
            Ship.Brake(false);
        }

        var to = Utils.ShootTo(RB, Target.RB, Ship.mainWeapon.AntiSpeed, AimingQuality);

        target.Fire =
            (Vector3.Dot(transform.forward, (Target.transform.position - transform.position).normalized) >
             AccuracyShooting) &&
            !Ship.mainWeapon.IsOutOfRange(Vector3.Distance(transform.position, to));
        
        Ship.LookAt(to);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Ttransform.position, MinOptimalDistance);
        Gizmos.DrawWireSphere(Ttransform.position, MaxOptimalDistance);

    }
}