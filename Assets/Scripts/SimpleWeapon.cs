using UnityEngine;

public sealed class SimpleWeapon : ShipWeapon
{
    public GameObject Projectile;

    public float bulletLifeTime;

    public float bulletSpeed;

    public override float AntiSpeed => 1 / bulletSpeed;

    protected override void DoFire()
    {
        var q = ProjectilePool.Self.Get();
        q.transform.SetPositionAndRotation(transform.position + transform.forward, transform.rotation);
        var p = q.GetComponent<Projectile>();
        p.team = Parent.team;
        p.parent = Parent;
        p.LifeTime = bulletLifeTime;
        var rb = q.GetComponent<Rigidbody>();
        rb.velocity = Parent.RB.velocity + (transform.forward * bulletSpeed);
        var trail = q.GetComponent<TrailRenderer>();
        if (trail)
            trail.AddPosition(transform.position);
    }
}