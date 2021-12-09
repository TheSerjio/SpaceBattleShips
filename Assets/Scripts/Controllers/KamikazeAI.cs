using UnityEngine;

public class KamikazeAI : ShipAIController
{
    public float BoomDistance;

    public override void OnFixedUpdate()
    {
        var dot = Vector3.Dot(transform.forward, RB.velocity);
        if (dot < -1f)
            Ship.Brake(false);
        else
            Ship.Forward();

        if (Physics.Raycast(transform.position, Target.transform.position - transform.position, out var hit,
            BoomDistance))
        {
            var q = hit.collider.GetComponentInParent<Ship>();
            if (q == Target)
                Detonate();
        }

        {
            var tar = Target.transform.position;
            tar = (tar - transform.position).normalized;
            var tarDot = Vector3.Dot(tar, RB.velocity);
            if (tarDot < 1f)
            {

                Ship.EnginePower = (Ship.MaxEngine + Ship.MinEngine) / 2f;
                Ship.LookAt(transform.position + tar);
            }
            else
            {
                Ship.EnginePower = Vector3.Dot(tar, RB.velocity.normalized) * Ship.MaxEngine;
                var velocity = (RB.velocity - Target.RB.velocity).normalized;
                var q = -1f / Vector3.Distance(tar, velocity);
                Ship.LookAt(transform.position + Vector3.SlerpUnclamped(tar, velocity, q));
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Ttransform.position, BoomDistance);
    }
}