using UnityEngine;

public class LaserFighterShip : ShipSimpleAIController
{
    public float LaserAccuracy;

    public float OptimalDistance;

    public override void OnFixedUpdate()
    {
        if (target)
        {
            var dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist > OptimalDistance)
            {
                Ship.EnginePower = 1;
                Ship.Forward();
            }
            else
            {
                Ship.BrakePower = 1;
                Ship.Brake();
            }
            Ship.Fire = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized) > 0;
            LookAt(target.transform.position + Random.insideUnitSphere * Vector3.Distance(transform.position, target.transform.position) / LaserAccuracy);
        }
    }
}