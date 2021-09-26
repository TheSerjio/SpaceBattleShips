using UnityEngine;

public class EnemyBulletShip : Ship
{
    private Rigidbody target;

    public SimpleWeapon mainWeapon;

    public override bool IsPlayerControlled => false;

    public override void WhenDestroy() { }

    public override void OnStart() { }

    public void FixedUpdate()
    {
        if (target)
        {
            float a = 0;
            if (mainWeapon)
                a = 1f / mainWeapon.bulletSpeed;
            else
                mainWeapon = GetWeapon<SimpleWeapon>();
            float distance = Vector3.Distance(target.position, transform.position);
            Vector3 targetPos = target.position + (target.velocity * a);
            var lookAt = targetPos - (RB.velocity * distance);
    //        if (Vector3.Dot(transform.forward, lookAt - transform.position) >= 0)
                LookAt(lookAt);
//            else
  //              LookAt(targetPos);
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

    public override void OnUpdate() { }
}