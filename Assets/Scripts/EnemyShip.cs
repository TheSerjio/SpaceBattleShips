using UnityEngine;

public class EnemyShip : Ship
{
    private Rigidbody target;

    public override bool IsPlayerControlled => false;

    public override void WhenDestroy() { }

    public override void OnStart() { }

    public override void OnUpdate()
    {
        if (target)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            Vector3 targetPos = target.position + (target.velocity * distance);
            LookAt(targetPos - (RB.velocity * distance));
            Fire();
            RB.velocity += speed * Time.deltaTime * transform.forward;
        }
        else
        {
            var q = FindObjectOfType<PlayerShip>();
            if (q)
                target = q.RB;
        }


    }
}