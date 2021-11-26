using UnityEngine;
public class KamikazeAI : ShipAIController
{
    public float BoomDistance;
    
    public override void OnFixedUpdate()
    {
        var dot = Vector3.Dot(transform.forward, RB.velocity.normalized);
        if (dot < 0.5f)
            Ship.Brake(false);

        Ship.EnginePower = Mathf.Clamp(5f * dot, 1, 5);

        Ship.Forward();

        if (Physics.Raycast(transform.position, Target.transform.position - transform.position, out var hit, BoomDistance))
        {
            var q = hit.collider.GetComponentInParent<Ship>();
            if (q == Target)
                Detonate();
        }

        {
            var tar = Target.transform.position;
            tar = (tar - transform.position).normalized;
            if (Vector3.Dot(tar, RB.velocity) < 1f)
                Ship.LookAt(transform.position + tar);
            else
                Ship.LookAt(transform.position +
                            Vector3.SlerpUnclamped(tar, (RB.velocity - Target.RB.velocity).normalized, -1f));
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Ttransform.position, BoomDistance);
    }
}